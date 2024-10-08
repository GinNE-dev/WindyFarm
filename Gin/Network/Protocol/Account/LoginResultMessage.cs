﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Account
{
    public class LoginResultMessage : Message
    {
        public override MessageTag Tag => MessageTag.LoginResult;
        public LoginResult Result { get; set; }
        public string ExtraMessage { get; set; } = string.Empty;
        public int MapId { get; set; } = -1;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleLoginResult(this);
        }

        protected override void DecodeJson(string json)
        {
            LoginResultMessage? m = JsonHelper.ParseObject<LoginResultMessage>(json);
            if (m != null)
            {
                Result = m.Result;
                ExtraMessage = m.ExtraMessage;
                MapId = m.MapId;
            }
        }
    }

    public enum LoginResult
    {
        Success,
        IncorrectLoginInfo,
        MissingCharacter,
        LoginOnOtherSession
    }
}
