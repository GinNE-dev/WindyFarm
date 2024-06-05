using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Login
{
    public class LoginMessage : Message
    {
        public override MessageTag Tag => MessageTag.Login;
        public string Email = string.Empty;
        public string Password = string.Empty;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleLogin(this);
        }

        protected override void DecodeJson(string json)
        {
            LoginMessage? message = JsonHelper.ParseObject<LoginMessage>(json);
            if (message != null)
            {
                Email = message.Email;
                Password = message.Password;
            }
        }
    }
}
