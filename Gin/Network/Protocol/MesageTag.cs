using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

