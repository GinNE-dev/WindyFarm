﻿namespace WindyFarm.Gin.Network.Protocol
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
        FarmlandDataResponse
    }
}

