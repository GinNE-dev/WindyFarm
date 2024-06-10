using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Account
{
    public class CreateCharacterMessage : Message
    {
        public override MessageTag Tag => MessageTag.CreateCharacter;
        
        public string DisplayName = string.Empty;
        public string Gender = "Male";

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleCreateCharacter(this);
        }

        protected override void DecodeJson(string json)
        {
            CreateCharacterMessage? msg = JsonHelper.ParseObject<CreateCharacterMessage>(json);
            if (msg != null)
            {
                DisplayName = msg.DisplayName;
                Gender = msg.Gender;
            }
        }
    }
}
