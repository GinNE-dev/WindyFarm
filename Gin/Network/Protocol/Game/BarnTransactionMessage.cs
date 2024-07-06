using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BarnTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.BarnTransaction;
        public int BarnSlotIdx { get; set; }
        public int UsedItemId { get; set; }
        public Guid UsedItemDataId { get; set; }
        public BarnAction Action { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleBarnTransaction(this);
        protected override void DecodeJson(string json)
        {
            BarnTransactionMessage? message = JsonHelper.ParseObject<BarnTransactionMessage>(json);
            if (message is not null) 
            { 
                UsedItemId = message.UsedItemId;
                UsedItemDataId = message.UsedItemDataId;
                BarnSlotIdx = message.BarnSlotIdx;
                Action = message.Action;
            }
        }
    }
    public enum BarnAction
    {
        SpawnAnimal,
        Feed,
        Harvest
    }
}
