using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FarmlandTransactionResultMessage : Message
    {
        public override MessageTag Tag => MessageTag.FarmlandTransactionResult;
        public FarmlandAction Action { get; set; }
        public int PlotIndex;
        public bool Success;
        public int PlantedSeed {  get; set; }

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleFarmlandTransactionResult(this);
        }

        protected override void DecodeJson(string json)
        {
            FarmlandTransactionResultMessage? m = JsonHelper.ParseObject<FarmlandTransactionResultMessage>(json);
            if (m is not null)
            {
                Action = m.Action;
                PlotIndex = m.PlotIndex;
                Success = m.Success;
                PlantedSeed = m.PlantedSeed;
            }
        }
    }
}
