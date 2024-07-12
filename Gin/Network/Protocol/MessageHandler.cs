using WindyFarm.Gin.Network.Protocol.Account;
using WindyFarm.Gin.Network.Protocol.Game;
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
        public virtual bool handleLogin(LoginMessage message) => false;
        public virtual bool handleLoginResult(LoginResultMessage message) => false;
        public virtual bool handleRegister(RegisterMessage message) => false;
        public virtual bool handleRegisterResult(RegisterResultMessage message) => false;
        public virtual bool handleCreateCharacter(CreateCharacterMessage message) => false;
        public virtual bool handleCreateCharacterResult(CreateCharacterResultMessage message) => false;
        public virtual bool handlePlayerDataRequest(RequestPlayerMessage message) => false;
        public virtual bool handlePlayerDataResponse(PlayerDataMessage message) => false;
        public virtual bool handlePlayerMovement(PlayerMovementMessage message) => false;
        public virtual bool handlePing(PingMessage message) => false;
        public virtual bool handlePingReply(PingReplyMessage message) => false;
        public virtual bool handleInventoryRequest(InventoryRequestMessage message) => false;
        public virtual bool handleInventoryResponse(InventoryResponseMessage message) => false;
        public virtual bool handleInventoryTransaction(InventoryTransactionMessage message) => false;
        public virtual bool handleFarmlandRequest(FarmlandRequestMessage message) => false;
        public virtual bool handleFarmlandResponse(FarmlandResponseMessage message) => false;
        public virtual bool handleFarmlandTransaction(FarmlandTransactionMessage message) => false;
        public virtual bool handleFarmlandTransactionResult(FarmlandTransactionResultMessage message) => false;
        public virtual bool handleFarmingShopRequest(FarmingShopRequestMessage message) => false;
        public virtual bool handleFarmingShopResponse(FarmingShopDataMessage message) => false;
        public virtual bool handleFarmingShopTransaction(FarmingShopTransactionMessage message) => false;
        public virtual bool handleItemConnsumption(ItemConsumptionMessage message) => false;
        public virtual bool handleBarnDataResquest(BarnDataRequestMessage message) => false;
        public virtual bool handleBarnDataResponse(BarnDataResponseMessage message) => false;
        public virtual bool handleBarnTransaction(BarnTransactionMessage message) => false;
        public virtual bool handleBarnSpawnAnimal(BarnSpawnAnimalMessage message) => false;
        public virtual bool handleBarnTransactionResult(BarnTransactionResultMessage message) => false;
    }
}
