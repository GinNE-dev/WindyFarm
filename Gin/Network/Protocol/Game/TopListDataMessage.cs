using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class TopListDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.TopListData;
        public TopField TopField { get; set; }
        public List<string> DisplayNames { get; set; } = new();
        public List<Guid> PlayerIdList { get; set; } = new();
        public List<int> Levels { get; set; } = new();
        public List<int> GoldList {  get; set; } = new();

        public override bool Execute(IMessageHandler handler) => handler.handleTopListResponse(this);

        protected override void DecodeJson(string json)
        {
            TopListDataMessage? m = JsonHelper.ParseObject<TopListDataMessage>(json);
            if (m is not null)
            {
                TopField = m.TopField;
                PlayerIdList = m.PlayerIdList;
                DisplayNames = m.DisplayNames;
                Levels = m.Levels;
                GoldList = m.GoldList;
            }
        }
    }
}
