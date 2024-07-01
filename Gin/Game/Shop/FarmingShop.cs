using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Shop
{
    public class FarmingShop
    {
        public readonly List<FarmShop> ShopData;
        public readonly List<ItemSellPrice> ItemSellPrices;
        public float PriceFactor { get; private set; } = 1f;
        public FarmingShop(List<FarmShop> farmingShopData, List<ItemSellPrice> itemSellPrices)
        { 
            ShopData = farmingShopData;
            ItemSellPrices = itemSellPrices;
        }

        public int GetBuyPrice(int itemId)
        {
            var slot = ShopData.FirstOrDefault(slot=>slot.ItemId.Equals(itemId));
            return slot is null ? 0 : slot.BuyPrice;
        }

        public int GetSellPrice(int itemId)
        {
            var data = ItemSellPrices.FirstOrDefault(d=>d.ItemId.Equals(itemId));
            return data is null ? 0 : (int) Math.Round(data.BasePrice* PriceFactor);
        }

        public Item GetItemAtSellingPlot(int slotIndex)
        {
            var data = ShopData.FirstOrDefault(d=>d.SlotIndex.Equals(slotIndex));
            return ItemReplicator.Get(data is not null ? data.ItemId : 0);
        }

        public List<FarmShop> GetSellData() => [..ShopData.OrderBy(d=>d.SlotIndex)];

        public Dictionary<int, int> GetSellPrices()
        {
            Dictionary<int, int> sellPrices = new();
            foreach (var data in ItemSellPrices)
            {
                sellPrices.Add(data.ItemId, (int)Math.Round(data.BasePrice * PriceFactor));
            }

            return sellPrices;
        }

        public Message? PrepareShopDataMessage()
        {
            Message? m = MessagePool.Instance.Get(MessageTag.FarmingShopData);

            if (m is null || m is not FarmingShopDataMessage) return null;

            var data = (FarmingShopDataMessage)m;
            data.ItemIds = new();
            data.MetadataList = new();
            data.BuyPrices = new();
            data.SellPrices = new();


            data.SellPrices = GetSellPrices();
            foreach (var slot in GetSellData())
            {
                data.ItemIds.Add(slot.ItemId);
                data.MetadataList.Add(new Data.ItemDat() { Id = Guid.NewGuid(), ItemType = slot.ItemId, Quality = 1 });
                data.BuyPrices.Add(slot.BuyPrice);
            }

            return m;
        }
    }
}
