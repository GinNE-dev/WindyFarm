using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.ServerLog;

namespace WindyFarm.Gin.Network.Protocol
{
    public abstract class Message : IMessage
    {
        public abstract MessageTag Tag { get; }

        public void Decode(string json)
        {
            try
            {
                DecodeJson(json);
            }
            catch (Exception ex)
            {
                GinLogger.Fatal(ex);
            }
        }

        public string Endcode()
        {
            try
            {
                return EncodeJson();
            }
            catch (Exception ex)
            {
                GinLogger.Fatal(ex);
                return String.Empty;
            }
        }

        protected abstract void DecodeJson(string json);
        protected abstract string EncodeJson();
        public abstract bool Execute(IMessageHandler handler);
        public virtual Message Clone()
        {
            return (Message) MemberwiseClone();
        }
    }
}
