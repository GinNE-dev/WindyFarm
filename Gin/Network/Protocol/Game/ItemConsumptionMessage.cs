using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class ItemConsumptionMessage : Message
    {
        public override MessageTag Tag => MessageTag.ItemConsumption;
        public int InvSlotIndex { get; set; }
        public override bool Execute(IMessageHandler handler) => handler.handleItemConnsumption(this);

        protected override void DecodeJson(string json)
        {
            ItemConsumptionMessage? msg = JsonHelper.ParseObject<ItemConsumptionMessage>(json);
            if (msg is not null)
            {
                InvSlotIndex = msg.InvSlotIndex;
            }
        }
    }
}
