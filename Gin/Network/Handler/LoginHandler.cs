
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Login;

namespace WindyFarm.Gin.Network.Handler
{
    public class LoginHandler : MessageHandler
    {
        private readonly Server _server;
        private readonly Session _session;
        public LoginHandler(Server server, Session session) 
        {
            _server = server;
            _session = session;
        }

        public override bool handleLogin(LoginMessage message)
        {
            _session.Login(message.Email, message.Password);    
            return true;
        }
    }
}
