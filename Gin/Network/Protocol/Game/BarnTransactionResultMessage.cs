using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BarnTransactionResultMessage : Message
    {
        public override MessageTag Tag => MessageTag.BarnTransactionResult;
        public BarnAction Action { get; set; }
        public int SlotIndex { get; set; }
        public int UsedItemId { get; set; }
        public bool Success { get; set; }
        public override bool Execute(IMessageHandler handler) => handler.handleBarnTransactionResult(this);

        protected override void DecodeJson(string json)
        {
            BarnTransactionResultMessage? m = JsonHelper.ParseObject<BarnTransactionResultMessage>(json);
            if (m is not null)
            {
                Success = m.Success;
                SlotIndex = m.SlotIndex;
                Action = m.Action;
                UsedItemId = m.UsedItemId;
            }
        }
    }
}
