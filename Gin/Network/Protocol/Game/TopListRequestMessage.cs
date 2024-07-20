using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class TopListRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.TopListRequest;
        public TopField TopField { get; set; }
        public override bool Execute(IMessageHandler handler) => handler.handleTopListRequest(this);

        protected override void DecodeJson(string json)
        {
            TopListRequestMessage? m = JsonHelper.ParseObject<TopListRequestMessage>(json);
            if(m is not null)
            {
                TopField = m.TopField;
            }
        }
    }

    public enum TopField
    {
        Gold,
        Level
    }
}
