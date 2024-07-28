using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FriendTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.FriendTransaction;
        public Guid PlayerId { get; set; }
        public FriendAction Action { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleFriendTransaction(this);

        protected override void DecodeJson(string json)
        {
            FriendTransactionMessage? m = JsonHelper.ParseObject<FriendTransactionMessage>(json);
            if (m is not null)
            {
                PlayerId = m.PlayerId;
                Action = m.Action;
            }
        }
    }

    public enum FriendAction
    {
        Unfriend,
        Mail
    }
}
