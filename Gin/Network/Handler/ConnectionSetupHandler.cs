using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;

namespace WindyFarm.Gin.Network.Handler
{
    public class ConnectionSetupHandler(Server server, Session seesion) : MessageHandler
    {
        private readonly Server _server = server;
        private readonly Session _session = seesion;

        public override bool handleKeyRequest(RequestKeyMessage message)
        {
            _session.SendKey();
            return true;
        }

        public override bool handleKeyConfirm(ConfirmKeyMessage message)
        {
            _session.VerifyKey(message.Key, message.IV);
            return true;
        }
    }
}
