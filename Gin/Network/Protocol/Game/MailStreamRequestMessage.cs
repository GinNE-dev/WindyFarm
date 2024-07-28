using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class MailStreamRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.MailStreamRequest;
        public Guid MailId { get; set; }
        public override bool Execute(IMessageHandler handler) => handler.handleMailStreamRequest(this);

        protected override void DecodeJson(string json)
        {
            MailStreamRequestMessage? m = JsonHelper.ParseObject<MailStreamRequestMessage>(json);
            if(m is not null)
            {
                MailId = m.MailId;
            }
        }
    }
}
