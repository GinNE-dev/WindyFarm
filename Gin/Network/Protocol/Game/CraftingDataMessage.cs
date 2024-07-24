using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class CraftingDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.CraftingData;
        public List<int> MaterialIds { get; set; } = new();
        public List<int> InputQuantities { get; set; } = new();
        public List<int> OutputQuantities { get; set; } = new();
        public List<double> CraftingTimers { get; set; } = new();
        public List<int> Qualities { get; set; } = new();
        public override bool Execute(IMessageHandler handler) => handler.handleCraftingSlotResponse(this);

        protected override void DecodeJson(string json)
        {
            CraftingDataMessage? message = JsonHelper.ParseObject<CraftingDataMessage>(json);
            if(message is not null)
            {
                MaterialIds = message.MaterialIds;
                InputQuantities = message.InputQuantities;
                OutputQuantities = message.OutputQuantities;
                Qualities = message.Qualities;
                CraftingTimers = message.CraftingTimers;
            }
        }
    }
}
