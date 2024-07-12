using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BarnDataResponseMessage : Message
    {
        public override MessageTag Tag => MessageTag.BarnDataResponse;
        public List<int> SpawnerIdList { get; set; } = new();
        public List<int> HealthList { get; set; } = new();
        public List<int> FeedTimers { get; set; } = new();
        public List<int> HarvestTimers { get; set; } = new();
        public List<int> GrowTimers { get; set; } = new();  

        public override bool Execute(IMessageHandler handler) => handler.handleBarnDataResponse(this);

        protected override void DecodeJson(string json)
        {
            BarnDataResponseMessage? m = JsonHelper.ParseObject<BarnDataResponseMessage>(json);
            if (m is not null)
            {
                SpawnerIdList = m.SpawnerIdList;
                HealthList = m.HealthList;
                FeedTimers = m.FeedTimers;
                HarvestTimers = m.HarvestTimers;
                GrowTimers = m.GrowTimers;
            }
        }
    }
}
