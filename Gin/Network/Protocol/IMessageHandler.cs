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
        bool handleFarmlandTransaction(FarmlandTransactionMessage message);
        bool handleFarmlandTransactionResult(FarmlandTransactionResultMessage message);
        bool handleFarmingShopRequest(FarmingShopRequestMessage message);
        bool handleFarmingShopResponse(FarmingShopDataMessage message);
        bool handleFarmingShopTransaction(FarmingShopTransactionMessage message);
        bool handleItemConnsumption(ItemConsumptionMessage message);
        bool handleBarnDataResquest(BarnDataRequestMessage message);
        bool handleBarnDataResponse(BarnDataResponseMessage message);
        bool handleBarnTransaction(BarnTransactionMessage message);
        bool handleBarnSpawnAnimal(BarnSpawnAnimalMessage message);
        bool handleBarnTransactionResult(BarnTransactionResultMessage message);
        bool handleEnterMap(EnterMapMessage message);
        bool handlePlayerJoinMap(PlayerJoinMapMessage message);
        bool handleEnterMapResult(EnterMapResultMessage message);
        bool handlePlayerLeaveMap(PlayerLeaveMapMessage message);
        bool handleRequestDummiesInMap(RequestDummiesInMapMessage message);
        bool handleDummyMovement(DummyMovementMessage message);
        bool handleTopListRequest(TopListRequestMessage message);
        bool handleTopListResponse(TopListDataMessage message);
        bool handleCraftingSlotRequest(CraftingSlotRequestMessage message);
        bool handleCraftingSlotResponse(CraftingDataMessage message);
        bool handleFabricatorTransaction(FabricatorTransactionMessage message);
        bool handleFriendListRequest(FriendListRequestMessage message);
        bool handleFriendListResponse(FriendListDataMessage message);
        bool handleFriendInviteListRequest(FriendInviteListRequestMessage message);
        bool handleFriendInviteListResponse(FriendInviteListDataMessage message);
        bool handleFriendInviteTransaction(FriendInviteTransactionMessage message);
        bool handleFriendTransaction(FriendTransactionMessage message);
        bool handleMailBoxRequest(MailBoxRequestMessage message);
        bool handleMailBoxResponse(MailBoxDataMessage message);
        bool handleMailStreamRequest(MailStreamRequestMessage message);
        bool handleMailStreamResponse(MailStreamDataMessage message);
        bool handleMailTransaction(MailTransactionMessage message);
        bool handleBakeryRequest(BakeryRequestMessage message);
        bool handleBakeryResponse(BakeryDataMessage message);
        bool handleBakeryTransaction(BakeryTransactionMessage message);
    }
}
