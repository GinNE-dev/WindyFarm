using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class InventoryTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.InventoryTransaction;
        public int OriginalSlotIndex { get; set; }
        public int DestinationSlotIndex { get; set; }

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleInventoryTransaction(this);
        }

        protected override void DecodeJson(string json)
        {
            InventoryTransactionMessage? m = JsonHelper.ParseObject<InventoryTransactionMessage>(json);
            if (m is not null)
            {
                OriginalSlotIndex = m.OriginalSlotIndex;
                DestinationSlotIndex = m.DestinationSlotIndex;
            }
        }
    }
}
