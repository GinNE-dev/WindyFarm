using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Account
{
    public class RegisterMessage : Message
    {
        public override MessageTag Tag => MessageTag.RegisterAccount;
        public string Email = string.Empty;
        public string Password = string.Empty;
        public string Confirmation = string.Empty;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleRegister(this);
        }

        protected override void DecodeJson(string json)
        {
            RegisterMessage? message = JsonHelper.ParseObject<RegisterMessage>(json);
            if (message != null)
            {
                Email = message.Email;
                Password = message.Password;
                Confirmation = message.Confirmation;
            }
        }
    }
}
