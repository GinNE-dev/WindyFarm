using WindyFarm.Gin.Network.Protocol.Account;
using WindyFarm.Gin.Network.Protocol.Game;
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
        bool handleCreateCharacter(CreateCharacterMessage message);
        bool handleCreateCharacterResult(CreateCharacterResultMessage message);
        bool handlePlayerDataRequest(RequestPlayerMessage message);
        bool handlePlayerDataResponse(PlayerDataMessage message);
        bool handlePlayerMovement(PlayerMovementMessage message);
        bool handlePing(PingMessage message);
        bool handlePingReply(PingReplyMessage message);
        bool handleInventoryRequest(InventoryRequestMessage message);
        bool handleInventoryResponse(InventoryResponseMessage message);
        bool handleInventoryTransaction(InventoryTransactionMessage message);
        bool handleFarmlandRequest(FarmlandRequestMessage message);
        bool handleFarmlandResponse(FarmlandResponseMessage message);
    }
}
