namespace WindyFarm.Gin.Network.Protocol
{
    public enum MessageTag
    {
        //B
        Unknown = 0,
        TextMessage,
        /// C <summary>
        /// C
        /// </summary>
        KeyRequest,
        KeyConfirm,
        Ping,
        Login,
        RegisterAccount,
        CreateCharacter,
        PlayerDataRequest,
        PlayerMovement,
        InventoryDataRequest,
        InventoryTransaction,
        FarmlandDataRequest,
        FarmlandTransaction,
        FarmingShopRequest,
        FarmingShopTransaction,
        ItemConsumption,
        BarnRequest,
        BarnTransaction,
        SpawnBarnAnimal,
        ///S <summary>
        /// S
        KeySend,
        ConnectionResult,
        PingReply,
        LoginResult,
        RegisterResult,
        CreateCharacterResult,
        PlayerDataResponse,
        InventoryDataResponse,
        FarmlandDataResponse,
        FarmlandTransactionResult,
        FarmingShopData,
        BarnDataResponse,
        BarnTransactionResult,
        EnterMap,
        EnterMapResult,
        PlayerJoinMap,
        PlayerLeaveMap,
        RequestDummiesInMap,
        DummyMovement,
        TopListRequest,
        TopListData,
        CraftingRequest,
        CraftingData,
        FabricatorTransaction,
        FriendListRequest,
        FriendListResponse,
        FriendInviteListRequest,
        FriendInviteListResponse,
        FriendInviteTransaction,
        FriendTransaction,
        MailBoxRequest,
        MailBoxResponse,
        MailStreamRequest,
        MailStreamResponse,
        MailTransaction,
        BakeryRequest,
        BakeryResponse,
        BakeryTransaction
    }
}

