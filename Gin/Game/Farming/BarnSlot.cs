using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Farming
{
    public class BarnSlot
    {
        public int SlotIndex => _barnSlotData.SlotIndex;

        public int SpawnerId => _barnSlotData.SpawnerId;
        public int AnimalHealth => _barnSlotData.AnimalHealth;
        public DateTime GrowAt => _barnSlotData.GrowAt;

        public DateTime LastFeedAt => _barnSlotData.LastFeedAt;

        public DateTime GiveProductAt => _barnSlotData.GiveProductAt;

        public readonly BarnDat _barnSlotData;

        public BarnSlot(BarnDat poultryDat)
        { 
            _barnSlotData = poultryDat;
        }

        public void LazyUpdateTimeMarker()
        {
            var item = ItemReplicator.Get(SpawnerId);
            if (item is not CreatureSpawner ) return;

            var spawner = (CreatureSpawner) item;
            var updateAt = DateTime.Now;
            var feedingLateInSecond = Math.Max(0, (updateAt - LastFeedAt.AddSeconds(spawner.FeedTime)).TotalSeconds);
            var lastUpdateAt = _barnSlotData.LastTimeMarkerUpdate;
            var lastFeedingLateInSecond = Math.Max(0, (lastUpdateAt - LastFeedAt.AddSeconds(spawner.FeedTime)).TotalSeconds);

            var changeInSecond = feedingLateInSecond - lastFeedingLateInSecond;

            _barnSlotData.GrowAt = GrowAt.AddSeconds(changeInSecond);
            _barnSlotData.GiveProductAt = GiveProductAt.AddSeconds(changeInSecond);
            
            _barnSlotData.LastTimeMarkerUpdate = DateTime.Now;
        }

        public void SpawnCreature(CreatureSpawner spawner)
        {
            var currentSpawner = ItemReplicator.Get(SpawnerId);
            if (currentSpawner is CreatureSpawner) return;

            _barnSlotData.SpawnerId = spawner.Id;
            _barnSlotData.AnimalHealth = spawner.Quality;
            _barnSlotData.LastFeedAt = DateTime.Now;
            _barnSlotData.GrowAt = DateTime.Now.AddSeconds(spawner.StageGrowingTimes.Sum());
            _barnSlotData.GiveProductAt = GrowAt.AddSeconds(spawner.ProduceTime);
        }

        public void Feed()
        {
            var item = ItemReplicator.Get(SpawnerId);
            if (item is not CreatureSpawner) return;

            var spawner = (CreatureSpawner)item;

            if (DateTime.Now < LastFeedAt.AddSeconds(spawner.FeedTime)) return;

            LazyUpdateTimeMarker();

            _barnSlotData.LastFeedAt = DateTime.Now;
        }

        public Item GetHarvestProduct()
        {
            var spawner = ItemReplicator.Get(SpawnerId) as CreatureSpawner;
            if (spawner is null) return ItemReplicator.Get(ItemId.VOID_ITEM);
            LazyUpdateTimeMarker();
            var harvestLate = (DateTime.Now - GiveProductAt).TotalSeconds;
            GinLogger.Debug($"harvestLate : {harvestLate}");
            if (harvestLate < -5) return ItemReplicator.Get(ItemId.VOID_ITEM);
            
            var product = spawner.HarvestProduct;
            product.AssignMetaData(new ItemDat() { Id = Guid.NewGuid(), ItemType = product.Id, Quality = 1 });

            return product;
        }

        public void Harvest()
        {
            var item = ItemReplicator.Get(SpawnerId);
            if (item is not CreatureSpawner) return;
            var spawner = (CreatureSpawner) item;
            _barnSlotData.GiveProductAt = DateTime.Now.AddSeconds(spawner.ProduceTime);
        }
    }
}
