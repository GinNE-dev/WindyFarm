using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FarmingShopDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.FarmingShopData;
        public List<int> ItemIds { get; set; } = new();
        public List<ItemDat> MetadataList { get; set; } = new();
        public List<int> BuyPrices { get; set; } = new();
        public Dictionary<int, int> SellPrices { get; set; } = new();

        public override bool Execute(IMessageHandler handler) => handler.handleFarmingShopResponse(this);

        protected override void DecodeJson(string json)
        {
            FarmingShopDataMessage? m = JsonHelper.ParseObject<FarmingShopDataMessage>(json);
            if (m is not null)
            {
                ItemIds = m.ItemIds;
                MetadataList = m.MetadataList;
                BuyPrices = m.BuyPrices;
                SellPrices = m.SellPrices;
            }
        }
    }
}
