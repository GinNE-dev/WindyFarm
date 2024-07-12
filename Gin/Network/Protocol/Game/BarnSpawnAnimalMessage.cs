using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BarnSpawnAnimalMessage : Message
    {
        public override MessageTag Tag => MessageTag.SpawnBarnAnimal;
        public int SpawnerId { get; set; }
        public Guid SpawnerDataId { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleBarnSpawnAnimal(this);

        protected override void DecodeJson(string json)
        {
            BarnSpawnAnimalMessage? message = JsonHelper.ParseObject<BarnSpawnAnimalMessage>(json);
            if (message != null)
            {
                SpawnerId = message.SpawnerId;
                SpawnerDataId = message.SpawnerDataId;
            }
        }
    }
}
