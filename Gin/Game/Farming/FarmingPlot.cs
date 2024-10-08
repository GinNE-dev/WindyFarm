﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Farming
{
    public class FarmingPlot
    {
        public Guid OwnerId => PlotData.OwnerId;
        public int PlotIndex => PlotData.PlotIndex;
        public int Seed => PlotData.Seed;
        public bool Fertilized => PlotData.Fertilized;
        public int CropQualityRiseChange => PlotData.CropQualityRiseChange;
        public int CropQuality => PlotData.CropQuality;
        public string PlotState => PlotData.PlotState;
        public DateTime PlantedAt => PlotData.PlantedAt;

        public readonly FarmlandDat PlotData;
        public FarmingPlot(FarmlandDat plotData)
        {
            PlotData = plotData;
        }

        public bool Buy()
        {
            if(!PlotState.ToLower().Equals("buyable")) return false;

            PlotData.PlotState = "Messed";
            return true;
        }

        public void Till()
        {
            if (!PlotState.ToLower().Equals("messed")) return;

            PlotData.PlotState = "Tilled";
        }

        public bool IsAllowPlant()
        {
            return PlotState.ToLower().Equals("tilled");
        }

        public void Plant(Seed seed)
        {
            if (!IsAllowPlant()) return;
            PlotData.Seed = seed.Id;
            PlotData.CropQuality = seed.Quality;
            PlotData.PlantedAt = DateTime.Now;
            PlotData.PlotState = "Planted";
        }

        public void AllowBuy()
        {
            PlotData.PlotState = "Buyable";
        }

        public void ClearPlot()
        {
            PlotData.PlotState = "Messed";
            PlotData.Seed = 0;
            PlotData.CropQuality = 1;
            PlotData.CropQualityRiseChange = 0;
            PlotData.Fertilized = false;
        }

        public bool AllowFertilize()
        {
            if (!PlotState.ToLower().Equals("planted")) return false;
            if(!Fertilized) return false;

            return true;
        }

        public void Fertilize(Fertilizer fertilizer)
        {
            if(!AllowFertilize()) return;
            PlotData.CropQualityRiseChange = fertilizer.QualityRiseChange;
            PlotData.Fertilized = true;
        }

        public Item GetHarvestProduct()
        {
            if (!PlotState.ToLower().Equals("planted"))
                return ItemReplicator.Get(ItemId.VOID_ITEM);
            var item = ItemReplicator.Get(this.Seed);
            if (item is null || item is not Items.Seed)
                return ItemReplicator.Get(ItemId.VOID_ITEM);
            var seed = (Seed)item;

            if (DateTime.Now < PlantedAt.AddSeconds(seed.StageGrowingTimes.Sum()))
                return ItemReplicator.Get(ItemId.VOID_ITEM);

            var product = seed.HarvestProduct;

            var productQuality = FarmBalancer.CalProductQuality(CropQuality, CropQualityRiseChange);
            var productData = new ItemDat() { Id = Guid.NewGuid(), ItemType = product.Id, Quality = productQuality };
            product.AssignMetaData(productData);

            return product;
        }
    }
}
