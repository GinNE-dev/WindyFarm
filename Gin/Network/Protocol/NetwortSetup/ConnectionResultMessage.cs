﻿using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.NetwortSetup
{
    public class ConnectionResultMessage : Message
    {
        public override MessageTag Tag { get; }  = MessageTag.ConnectionResult;
        public bool Granted = false;
        public string ExtraMessage = string.Empty;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleConnetionResult(this);
        }

        protected override void DecodeJson(string json)
        {
            ConnectionResultMessage? message = JsonHelper.ParseObject<ConnectionResultMessage>(json);
            if (message != null)
            {
                Granted = message.Granted;
                ExtraMessage = message.ExtraMessage;
            }
        }
    }
}
