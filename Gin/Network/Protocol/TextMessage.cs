using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol
{
    public class TextMessage : Message
    {
        public override MessageTag Tag { get; } = MessageTag.TextMessage;

        public string Text = string.Empty;
        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleTextMessage(this);
        }

        protected override void DecodeJson(string json)
        {
            TextMessage? message = JsonHelper.ParseObject<TextMessage>(json);
            if (message != null)
            {
                Text = message.Text;
            }
        }

        protected override string EncodeJson()
        {
            return JsonHelper.ParseString(this);
        }
    }
}
