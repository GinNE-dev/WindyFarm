using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FriendListRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.FriendListRequest;

        public override bool Execute(IMessageHandler handler) => handler.handleFriendListRequest(this);

        protected override void DecodeJson(string json) { }
    }
}
