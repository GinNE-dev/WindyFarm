using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;

namespace WindyFarm.Gin.Game.Farming
{
    public class FarmingPlot
    {
        public Guid OwnerId => PlotData.OwnerId;
        public int PlotIndex => PlotData.PlotIndex;
        public int Seed => PlotData.Seed;
        public bool Fertilized => PlotData.Fertilized;
        public int CropQuality => PlotData.CropQuality;
        public string PlotState => PlotData.PlotState;
        public DateTime HavestAt => PlotData.HarvestAt;

        public readonly FarmlandDat PlotData;
        public FarmingPlot(FarmlandDat plotData)
        {
            PlotData = plotData;
        }

        public void BuySlot()
        {
            if(!PlotState.ToLower().Equals("wild")) return;

            PlotData.PlotState = "Messed";
        }

        public void Till()
        {
            if (!PlotState.ToLower().Equals("messed")) return;

            PlotData.PlotState = "Tilled";
        }

        public void Plant(Seed seed)
        {
            if (!PlotState.ToLower().Equals("Tiled")) return;
            PlotData.Seed = seed.Id;
            PlotData.CropQuality = seed.Quality;
            PlotData.HarvestAt = DateTime.Now.AddSeconds(seed.StageGrowingTimes.Sum());
            PlotData.PlotState = "Planted";
        }
    }
}
