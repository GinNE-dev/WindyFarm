using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class MailStreamDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.MailStreamResponse;
        public Guid MailID { get; set; }
        public List<string> MessageText { get; set; } = new();
        public List<DateTime> SentAts { get; set; } = new();
        public List<bool> SentBySelfs { get; set; } = new();
        public override bool Execute(IMessageHandler handler) => handler.handleMailStreamResponse(this);

        protected override void DecodeJson(string json)
        {
            MailStreamDataMessage? m = JsonHelper.ParseObject<MailStreamDataMessage>(json);
            if(m is not null)
            {
                MailID = m.MailID;
                MessageText = m.MessageText;
                SentAts = m.SentAts;
                SentBySelfs = m.SentBySelfs;
            }
        }
    }
}
