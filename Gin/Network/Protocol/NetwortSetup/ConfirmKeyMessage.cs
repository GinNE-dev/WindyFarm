using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.NetwortSetup
{
    public class ConfirmKeyMessage : SendKeyMessage
    {
        public override MessageTag Tag { get;} = MessageTag.KeyConfirm;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleKeyConfirm(this);
        }
    }
}
