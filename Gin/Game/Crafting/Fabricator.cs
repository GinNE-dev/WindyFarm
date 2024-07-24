using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Farming;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Crafting
{
    public class Fabricator
    {
        protected int CraftingSlotCapacity = 5;
        private readonly Player _owner;
        private readonly WindyFarmDatabaseContext _dbContext;
        private readonly ConcurrentDictionary<int, CraftingSlot> CraftingSlots;
        public Fabricator(Player owner, WindyFarmDatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _owner = owner;
            CraftingSlots = new();
            InitSlots();
        }

        public void InitSlots()
        {
            var slotDataList = _dbContext.CraftingSlotDats.Where(d => d.OwnerId.Equals(_owner.Id));
            for (int slotIdx = 0; slotIdx < CraftingSlotCapacity; slotIdx++)
            {
                var slotData = slotDataList.FirstOrDefault(s => s.SlotIndex.Equals(slotIdx));
                if (slotData is null)
                {
                    slotData = new CraftingSlotDat() { OwnerId = _owner.Id, SlotIndex = slotIdx };
                    _dbContext.CraftingSlotDats.Add(slotData);
                }

                CraftingSlots.TryAdd(slotIdx, new CraftingSlot(slotData));
            }
        }

        public CraftingSlot? FirstEmpty() => CraftingSlots.Values.FirstOrDefault(cs => cs.IsEmpty());

        public void Craft(int materialId, Guid materialMetaId, int qty)
        {
            var emptySlot = FirstEmpty();
            if (emptySlot is null) return;

            var item = _owner.Inventory.TryTakeItem<Item>(materialId, materialMetaId, qty);

            if (item is null) return;

            if(item is not CraftingMaterial)
            {
                _owner.Inventory.TryPutItem(item, qty);
                return;
            }

            _owner.SendInventory();

            var material = (CraftingMaterial) item;

            emptySlot.StartCraft(material, qty);

            SendCraftSlots();
        }

        public void TakeProduct(int slotIdx)
        {
            var slot = CraftingSlots.Values.FirstOrDefault(cs=>cs.SlotIndex.Equals(slotIdx));
            
            if (slot is null) return;
            slot.GetProduct(out Item? product, out int qty);

            if (product is null) return;
            if (!_owner.Inventory.TryPutItem(product, qty)) return;
            _owner.SendInventory();

            slot.CompleteCraft();

            SendCraftSlots();
        }

        public void SendCraftSlots()
        {
            Message? m = MessagePool.Instance.Get(MessageTag.CraftingData);
            if(m is null or not CraftingDataMessage) return;

            CraftingDataMessage cdm = (CraftingDataMessage)m;
            cdm.MaterialIds.Clear();
            cdm.CraftingTimers.Clear();
            cdm.InputQuantities.Clear();
            cdm.OutputQuantities.Clear();
            cdm.Qualities.Clear();

            var slots = CraftingSlots.Values.OrderBy(cs=>cs.SlotIndex);
            foreach (var slot in slots)
            {
                cdm.MaterialIds.Add(slot.MaterialId);
                cdm.InputQuantities.Add(slot.InputAmount);
                cdm.OutputQuantities.Add(slot.GetOutputAmount());
                cdm.Qualities.Add(slot.MaterialQuality);
                cdm.CraftingTimers.Add(Math.Max(0, (slot.CompleteAt - DateTime.Now).TotalSeconds));
            }

            _owner.SendMessageAsync(cdm);
        }
    }
}
