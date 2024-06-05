using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Protocol;

namespace WindyFarm.Gin.Network.Protocol
{
    public interface IMessage
    {
        public MessageTag Tag { get; }
        public String Endcode();
        public void Decode(String json);

        public bool Execute(IMessageHandler messageHandler);
    }
}
