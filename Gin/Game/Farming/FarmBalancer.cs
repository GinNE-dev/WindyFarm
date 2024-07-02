using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Farming
{
    public class FarmBalancer
    {
        public static int CalProductQuality(int cropQuality, int extraChange)
        {
            if (cropQuality < 0) return 0;
            if (cropQuality > 5) return 5;

            return new Random().Next(100) < (1+ extraChange/100.0) * 50.0/(cropQuality+1) ? cropQuality + 1 : cropQuality;  
        }

        public static int TillPlotEnergyConsumtion => 2;
        public static int SeedPlantEnergyConsumtion => 1;
        public static int HarvestEnergyConsumtion => 1;
    }
}
