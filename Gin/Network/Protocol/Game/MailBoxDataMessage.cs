using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class MailBoxDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.MailBoxResponse;
        public List<Guid> MailIds { get; set; } = new();
        public List<string> ParnerNames { get; set; } = new();
        public List<DateTime> LastSentTimes { get; set; } = new();
        public List<string> LastMessages { get; set; } = new();
        public List<bool> IsNews { get; set; } = new();

        public override bool Execute(IMessageHandler handler) => handler.handleMailBoxResponse(this);

        protected override void DecodeJson(string json)
        {
            MailBoxDataMessage? m = JsonHelper.ParseObject<MailBoxDataMessage>(json);
            if(m is not null)
            {
                MailIds = m.MailIds;
                ParnerNames = m.ParnerNames;
                LastSentTimes = m.LastSentTimes;
                LastMessages = m.LastMessages;
                IsNews = m.IsNews;
            }    
        }
    }
}
