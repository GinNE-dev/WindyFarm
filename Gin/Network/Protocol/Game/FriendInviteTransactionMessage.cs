using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FriendInviteTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.FriendInviteTransaction;
        public Guid PlayerId { get; set; }
        public DateTime OccurAt { get; set; }
        public FriendInviteAction Action { get; set; }
        public override bool Execute(IMessageHandler handler) => handler.handleFriendInviteTransaction(this);

        protected override void DecodeJson(string json)
        {
            FriendInviteTransactionMessage? m = JsonHelper.ParseObject<FriendInviteTransactionMessage>(json);
            if (m is not null)
            {
                PlayerId = m.PlayerId;
                OccurAt = m.OccurAt;
                Action = m.Action;
            }
        }
    }

    public enum FriendInviteAction
    {
        NewInvite,
        Accept,
        Reject
    }
}
