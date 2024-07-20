using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Maps
{
    public class Town : PublicMap
    {
        public override int Id => 1;

        public override string Name => "Town";

        //public override Vector3 SpawnPoint => new Vector3(13f, 0.1f, 149f);

        //public override Dictionary<int, Vector3> Portals => new() { {0, new Vector3(13f, 0.1f, 149f)} };
        public override Dictionary<Type, Vector3> EnteredSpawnPoints => new()
        {
            { typeof(Farm), new Vector3(13f, 0.1f, 149f) }
        };
    }
}
