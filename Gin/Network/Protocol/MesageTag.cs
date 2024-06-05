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
        CreateCharacter,
        PlayerDataRequest,
        ///S
        KeySend,
        ConnectionResult,
        LoginResult,
        NoCharacter,
        PlayerDataResponse
        
    }
}

