using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Account
{
    public class CreateCharacterResultMessage : Message
    {
        public override MessageTag Tag => MessageTag.CreateCharacterResult;
        public CreateCharacterResult Result { get; set; }
        public string ExtraMessage = string.Empty;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleCreateCharacterResult(this);
        }

        protected override void DecodeJson(string json)
        {
            CreateCharacterResultMessage? msg = JsonHelper.ParseObject<CreateCharacterResultMessage>(json);
            if (msg != null)
            {
                Result = msg.Result;
                ExtraMessage = msg.ExtraMessage;
            }
        }
    }

    public enum CreateCharacterResult
    {
        Success,
        DisplayNameDuplicated,
        InternalError
    }
}
