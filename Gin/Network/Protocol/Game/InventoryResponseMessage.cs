using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class InventoryResponseMessage : Message
    {
        public override MessageTag Tag => MessageTag.InventoryDataResponse;
        public List<int> ItemIds { get; set; } = new();
        public List<int> StackCounts { get; set; } = new();
        public List<ItemDat> MetaDataList { get; set; } = new();
        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleInventoryResponse(this);
        }

        protected override void DecodeJson(string json)
        {
            InventoryResponseMessage?  msg = JsonHelper.ParseObject<InventoryResponseMessage>(json);
            if (msg is not null)
            {
                ItemIds = msg.ItemIds;
                StackCounts = msg.StackCounts;
                MetaDataList = msg.MetaDataList;
            }
        }
    }
}
