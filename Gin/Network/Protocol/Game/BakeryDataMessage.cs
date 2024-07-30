using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BakeryDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.BakeryResponse;
        public List<int> ItemIds { get; set; } = new();
        public List<ItemDat> MetadataList { get; set; } = new();
        public List<int> BuyPrices { get; set; } = new();
        public Dictionary<int, int> SellPrices { get; set; } = new();

        public override bool Execute(IMessageHandler handler) => handler.handleBakeryResponse(this);

        protected override void DecodeJson(string json)
        {
            BakeryDataMessage? m = JsonHelper.ParseObject<BakeryDataMessage>(json);
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
