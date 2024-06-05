
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;

namespace WindyFarm.Gin.Network.Protocol
{
    public abstract class MessageHandler : IMessageHandler
    {
        public virtual bool handleKeyRequest(RequestKeyMessage message) => false;

        public virtual bool handleKeyReceived(SendKeyMessage message) => false;

        public virtual bool handleKeyConfirm(ConfirmKeyMessage message) => false;
        public virtual bool handleConnetionResult(ConnectionResultMessage message) => false;

        public virtual bool handleTextMessage(TextMessage message) => false;
    }
}
