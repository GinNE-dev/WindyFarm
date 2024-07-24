using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FabricatorTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.FabricatorTransaction;
        public FabricatorAction Action { get; set; }
        public int SlotIndex { get; set; }
        public int MaterialId { get; set; }
        public Guid MaterialMetaId { get; set; }
        public int Amount { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleFabricatorTransaction(this);

        protected override void DecodeJson(string json)
        {
            FabricatorTransactionMessage? m = JsonHelper.ParseObject<FabricatorTransactionMessage>(json);
            if(m is not null)
            {
                Action = m.Action;
                SlotIndex = m.SlotIndex;
                MaterialId = m.MaterialId;
                MaterialMetaId = m.MaterialMetaId;
                Amount = m.Amount;
            }
        }
    }

    public enum FabricatorAction
    {
        Craft,
        Complete
    }
}
