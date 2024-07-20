using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class EnterMapResultMessage : Message
    {
        public override MessageTag Tag => MessageTag.EnterMapResult;
        public int EnteredMapId { get; set; }
        public bool Accepted {  get; set; }
        public string ExtraMessage {  get; set; } = string.Empty;

        public override bool Execute(IMessageHandler handler) => handler.handleEnterMapResult(this);

        protected override void DecodeJson(string json)
        {
            EnterMapResultMessage? m = JsonHelper.ParseObject<EnterMapResultMessage>(json);
            if (m is not null)
            {
                EnteredMapId = m.EnteredMapId;
                Accepted = m.Accepted;
                ExtraMessage = m.ExtraMessage;
            }
        }
    }
}
