using System;
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
        public LoginResult Result;
        public string ExtraMessage = string.Empty;
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
