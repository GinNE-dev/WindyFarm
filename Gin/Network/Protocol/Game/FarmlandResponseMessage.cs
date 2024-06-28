using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FarmlandResponseMessage : Message
    {
        public override MessageTag Tag => MessageTag.FarmlandDataResponse;
        public List<int> Seeds { get; set; } = new();
        public List<int> CropQualities { get; set; } = new();
        public List<bool> FertilizeStats { get; set; } = new();
        public List<int> GrownTimes { get; set; } = new();
        public List<string> PlotStates { get; set; } = new();
        public FarmlandResponseMessage() { }
        public override bool Execute(IMessageHandler handler) => handler.handleFarmlandResponse(this);

        protected override void DecodeJson(string json)
        {
            FarmlandResponseMessage? m = JsonHelper.ParseObject<FarmlandResponseMessage>(json);
            if (m is not null)
            {
                Seeds = m.Seeds;
                CropQualities = m.CropQualities;
                FertilizeStats = m.FertilizeStats;
                GrownTimes = m.GrownTimes;
                PlotStates = m.PlotStates;
            }
        }
    }
}
