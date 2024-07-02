using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Shop
{
    public class ShopBalancer
    {
        public static double PriceFactorByQuality(int quality)
        {
            if(quality < 1) return 1;
            switch(quality)
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
            //return 1.1 * Enumerable.Range(1, quality - 1).Aggregate(1.0, (acc, k) => acc * (1.1 + 0.1 * k));
        }
    }
}
