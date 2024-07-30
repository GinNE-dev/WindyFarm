using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Shop
{
    public class ShopManager
    {
       
        public FarmingShop? FarmingShop;
        public Bakery? Bakery;
        private static ShopManager? instance;
        public static ShopManager Instance 
        {
            get 
            {
                instance ??= new ShopManager();
                return instance;
            }
        }

        private ShopManager()
        {
            instance = this;
        }

        public bool Initialized { get; private set; }
        public void Init(WindyFarmDatabaseContext dbContext)
        {
            if(Initialized)
            {
                GinLogger.Warning("[ShopManager] Shopmanager initialized");
                return;
            }

            instance = this;
            FarmingShop = new FarmingShop([.. dbContext.FarmShops], [..dbContext.ItemSellPrices]);
            Bakery = new Bakery([.. dbContext.CakeShops], [.. dbContext.ItemSellPrices]);
        }
    }
}
