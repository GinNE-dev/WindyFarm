using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Account
{
    public class RegisterResultMessage : Message
    {
        public override MessageTag Tag => MessageTag.RegisterResult;
        public RegisterResult Result;
        public string ExtraMessage = string.Empty;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleRegisterResult(this);
        }

        protected override void DecodeJson(string json)
        {
            RegisterResultMessage? message = JsonHelper.ParseObject<RegisterResultMessage>(json);
            if (message != null)
            {
                Result = message.Result;
                ExtraMessage = message.ExtraMessage;
            }
        }
    }

    public enum RegisterResult
    {
        Success,
        EmailHasBeenUsed,
        EmailFormatInvalid,
        ConfirmationPasswordNotMatch,
        ViolationOfPasswordPrivacyPolicy,
        InternalError
    }
}
