using System;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.SystemLog;

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

        public string Encode()
        {
            try
            {
                return EncodeJson();
            }
            catch (Exception ex)
            {
                GinLogger.Fatal(ex);
                return string.Empty;
            }
        }

        protected abstract void DecodeJson(string json);
        protected virtual string EncodeJson() 
        { 
            return JsonHelper.ParseString(this);
        }

        public abstract bool Execute(IMessageHandler handler);
        public virtual Message Clone()
        {
            return (Message) MemberwiseClone();
        }
    }
}
