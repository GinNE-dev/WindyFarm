using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FriendListDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.FriendListResponse;
        public List<Guid> PlayerIds { get; set; } = new();
        public List<string> PlayerNames { get; set; } = new();
        public List<int> LastActiveTimes { get; set; } = new(); //In seconds

        public override bool Execute(IMessageHandler handler) => handler.handleFriendListResponse(this);

        protected override void DecodeJson(string json)
        {
            FriendListDataMessage? m = JsonHelper.ParseObject<FriendListDataMessage>(json);
            if(m is not null) 
            {
                PlayerIds = m.PlayerIds;
                PlayerNames = m.PlayerNames;
                LastActiveTimes = m.LastActiveTimes;
            }
        }
    }
}
