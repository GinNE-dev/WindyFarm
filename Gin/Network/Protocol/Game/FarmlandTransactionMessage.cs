using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FarmlandTransactionMessage : Message
    {
        public override MessageTag Tag => MessageTag.FarmlandTransaction;
        public int PlotIndex { get; set; }
        public FarmlandAction Action { get; set; } = FarmlandAction.Till;
        public int Seed {  get; set; }
        public Guid SeedDataId { get; set; } = Guid.Empty;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleFarmlandTransaction(this);
        }

        protected override void DecodeJson(string json)
        {
            FarmlandTransactionMessage? m = JsonHelper.ParseObject<FarmlandTransactionMessage>(json);
            if (m is not null)
            {
                PlotIndex = m.PlotIndex;
                Action = m.Action;
                Seed = m.Seed;
                SeedDataId = m.SeedDataId;
            }
        }
    }

    public enum FarmlandAction
    {
        Buy,
        Till,
        Plant,
        Harvest
    }
}
