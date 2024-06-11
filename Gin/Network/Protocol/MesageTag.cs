namespace WindyFarm.Gin.Network.Protocol
{
    public enum MessageTag
    {
        //B
        Unknown = 0,
        TextMessage,
        /// C
        KeyRequest,
        KeyConfirm,
        Login,
        RegisterAccount,
        CreateCharacter,
        PlayerDataRequest,
        PlayerMovement,
        ///S
        KeySend,
        ConnectionResult,
        LoginResult,
        RegisterResult,
        CreateCharacterResult,
        PlayerDataResponse
    }
}

