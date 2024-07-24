using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Farming
{
    public class FarmBalancer
    {
        public static int CalProductQuality(int cropQuality, int extraChange)
        {
            if (cropQuality < 0) return 0;
            if (cropQuality > 5) return 5;
            int r = new Random().Next(100);
            double p = (1 + extraChange / 100.0) * 50.0 / (cropQuality + 1);
            //GinLogger.Print($"R:{r} P: {p} for q:{cropQuality + 1}");
            return r < p ? cropQuality + 1 : cropQuality;  
        }

        public static int TillPlotEnergyConsumtion => 2;
        public static int SeedPlantEnergyConsumtion => 1;
        public static int HarvestEnergyConsumtion => 1;
        public static int FeedAnimalEnergyConsumtion => 2;
        public static int HarvestAnimalEnergyConsumtion => 2;

        public static int TillPlotExp => 1;
        public static int SeedPlatExp => 1;

        public static int GetProductExp(Item item)
        {
            if(item is null) return 0;

            return (int) Math.Floor(5 * ExpFactorByQuality(item.Quality));
        }

        public static double ExpFactorByQuality(int quality)
        {
            if (quality < 1) return 1;
            switch (quality)
            {
                case 1:
                    return 1;
                case 2:
                    return 1.1;
                case 3:
                    return 1.32;
                case 4:
                    return 1.716;
                case 5:
                    return 2.5;
            }

            return 2.5;
        }
    }
}
