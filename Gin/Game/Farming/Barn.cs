using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Farming
{
    public class Barn
    {
        public readonly int BARN_CAPACITY = 16;

        public readonly Player _owner;
        public readonly WindyFarmDatabaseContext _dbContext;
        private ConcurrentDictionary<int, BarnSlot> BarnSlots;
        public Barn(Player owner, WindyFarmDatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _owner = owner;
            BarnSlots = new();
            InitBarn();
        }

        public void InitBarn()
        {
            var slotDataList = _dbContext.BarnDats.Where(d=>d.OwnerId.Equals(_owner.Id));
            for(int slotIdx = 0; slotIdx < BARN_CAPACITY; slotIdx++)
            {
                var slotData = slotDataList.FirstOrDefault(s=>s.SlotIndex.Equals(slotIdx));
                if (slotData is null)
                {
                    slotData = new BarnDat() { OwnerId = _owner.Id, SlotIndex = slotIdx };
                    _dbContext.BarnDats.Add(slotData);
                    //_dbContext.SaveChangesAsync();
                }

                BarnSlots.TryAdd(slotIdx, new BarnSlot(slotData));
            }
        }

        public BarnSlot? FirstEmptySlot => BarnSlots.Values.OrderBy(s => s.SlotIndex).FirstOrDefault(s=>s.SpawnerId.Equals(0));

        public void FeedAnimal(int slotIdx, int usedItemId, Guid spawnerDataId)
        {
            var slot = BarnSlots.Values.FirstOrDefault(s => s.SlotIndex.Equals(slotIdx));
            if (slot is null) return;

            if (!slot.AlowFeed()) return;

            if (!_owner.TryConsumeEnergy(FarmBalancer.FeedAnimalEnergyConsumtion)) return;
            _owner.SendStats();

            var food = _owner.Inventory.TryTakeOne<AnimalFood>(usedItemId, spawnerDataId);
            if (food is null) return;

            slot.Feed();

            _owner.SendInventory();
            SendTransactionResult(slotIdx, BarnAction.Feed, true, usedItemId);
        }

        public void SpawnAnimal(int spawnerId, Guid spawnerDataId)
        {
            var candateSlot = FirstEmptySlot;
            if (candateSlot is null) return;

            var spawner = _owner.Inventory.TryTakeOne<CreatureSpawner>(spawnerId, spawnerDataId);
            _owner.SendInventory();
            if (spawner is null) return;

            candateSlot.SpawnCreature(spawner);

            SendTransactionResult(candateSlot.SlotIndex, BarnAction.SpawnAnimal, true, spawnerId);
        }

        public void HarvestAnimal(int slotIdx)
        {
            var slot = BarnSlots.Values.FirstOrDefault(s => s.SlotIndex.Equals(slotIdx));
            if (slot is null) return;

            if (!_owner.TryConsumeEnergy(FarmBalancer.HarvestAnimalEnergyConsumtion)) return;

            var product = slot.GetHarvestProduct();
            if(product is VoidItem) return;

            _owner.GainExp(FarmBalancer.GetProductExp(product));
            _owner.SendStats();

            if (!_owner.Inventory.TryPutOne(product)) return;
            _owner.SendInventory();

            slot.Harvest();

            SendTransactionResult(slotIdx, BarnAction.Harvest, true);
        }

        private void SendTransactionResult(int slotIdx, BarnAction action, bool success, int usedItemId = 0)
        {
            BarnTransactionResultMessage m = new();
            m.Action = action;
            m.Success = success;
            m.UsedItemId = usedItemId;
            m.SlotIndex = slotIdx;

            _owner.SendMessageAsync(m);
        }

        public Message? SendBarnData()
        {
            var m = MessagePool.Instance.Get(MessageTag.BarnDataResponse);
            if (m is null or not BarnDataResponseMessage) return null;
            var data = (BarnDataResponseMessage)m;

            data.SpawnerIdList.Clear();
            data.HealthList.Clear();
            data.FeedTimers.Clear();
            data.HarvestTimers.Clear();
            data.GrowTimers.Clear();

            var slots = BarnSlots.Values.OrderBy(s => s.SlotIndex);
            foreach (var slot in slots)
            {
                data.SpawnerIdList.Add(slot.SpawnerId);
                data.HealthList.Add(slot.AnimalHealth);

                var item = ItemReplicator.Get(slot.SpawnerId);
                if (item is CreatureSpawner)
                {
                    var spawner = (CreatureSpawner) item;
                    slot.LazyUpdateTimeMarker();
                    GinLogger.Print(slot.GrowAt.ToString());
                    data.FeedTimers.Add(Math.Max(spawner.FeedTime - (int)(DateTime.Now - slot.LastFeedAt).TotalSeconds, 0));
                    data.HarvestTimers.Add(Math.Max((int) (slot.GiveProductAt - DateTime.Now).TotalSeconds, 0));
                    data.GrowTimers.Add(Math.Max((int)(slot.GrowAt - DateTime.Now).TotalSeconds, 0));
                }
                else
                {
                    data.FeedTimers.Add(0);
                    data.HarvestTimers.Add(0);
                    data.GrowTimers.Add(0);
                }
            }

            _owner.SendMessageAsync(data);

            return data;
        }
    }
}
