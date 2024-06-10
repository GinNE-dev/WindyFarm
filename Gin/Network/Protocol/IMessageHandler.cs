using WindyFarm.Gin.Network.Protocol.Account;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;

namespace WindyFarm.Gin.Network.Protocol
{
    public interface IMessageHandler
    {
        bool handleKeyReceived(SendKeyMessage message);
        bool handleKeyRequest(RequestKeyMessage message);
        bool handleKeyConfirm(ConfirmKeyMessage message);
        bool handleConnetionResult(ConnectionResultMessage message);
        bool handleTextMessage(TextMessage message);
        bool handleLogin(LoginMessage message);
        bool handleLoginResult(LoginResultMessage message);
        bool handleRegister(RegisterMessage message);   
        bool handleRegisterResult(RegisterResultMessage message);
    }
}
