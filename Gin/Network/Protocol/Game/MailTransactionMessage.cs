using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class MailTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.MailTransaction;
        public Guid MailId { get; set; }
        public MailAction Action { get; set; }
        public string TextMessage { get; set; } = string.Empty;

        public override bool Execute(IMessageHandler handler) => handler.handleMailTransaction(this);

        protected override void DecodeJson(string json)
        {
            MailTransactionMessage? m = JsonHelper.ParseObject<MailTransactionMessage>(json);
            if (m is not null)
            {
                MailId = m.MailId;
                Action = m.Action;
                TextMessage = m.TextMessage;
            }
        }
    }

    public enum MailAction
    {
        Delete,
        AddMessage
    }
}
