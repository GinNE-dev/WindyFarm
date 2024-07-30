using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BakeryTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.BakeryTransaction;
        public BakeryAction Action { get; set; }
        public int SlotIndex { get; set; }
        public int Quantity { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleBakeryTransaction(this);

        protected override void DecodeJson(string json)
        {
            BakeryTransactionMessage? m = JsonHelper.ParseObject<BakeryTransactionMessage>(json);
            if (m is not null)
            {
                Action = m.Action;
                SlotIndex = m.SlotIndex;
                Quantity = m.Quantity;
            }
        }
    }

    public enum BakeryAction
    {
        Buy,
        Sell
    }
}
