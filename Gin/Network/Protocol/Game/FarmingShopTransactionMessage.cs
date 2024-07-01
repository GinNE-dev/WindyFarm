using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FarmingShopTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.FarmingShopTransaction;
        public FarmingShopTransaction Transaction { get; set; }
        public int SlotIndex { get; set; }
        public int Quantity { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleFarmingShopTransaction(this);

        protected override void DecodeJson(string json)
        {
            FarmingShopTransactionMessage? m = JsonHelper.ParseObject<FarmingShopTransactionMessage>(json);
            if(m is not null)
            {
                Transaction = m.Transaction;
                SlotIndex = m.SlotIndex;
                Quantity = m.Quantity;
            }
        }
    }

    public enum FarmingShopTransaction
    {
        Buy,
        Sell
    }
}
