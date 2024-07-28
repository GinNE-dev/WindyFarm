using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FriendInviteListDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.FriendInviteListResponse;
        public List<Guid> PlayerIds { get; set; } = new();
        public List<string> PlayerNames { get; set; } = new();
        public List<DateTime> InviteAts { get; set; } = new();
        public override bool Execute(IMessageHandler handler) => handler.handleFriendInviteListResponse(this);

        protected override void DecodeJson(string json)
        {
            FriendInviteListDataMessage? m = JsonHelper.ParseObject<FriendInviteListDataMessage>(json);
            if (m is not null)
            {
                PlayerIds = m.PlayerIds;
                PlayerNames = m.PlayerNames;
                InviteAts = m.InviteAts;
            }
        }
    }
}
