using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Maps
{
    public class InteriorWindMill : PrivateMap
    {
        public override int Id => 2;

        public override string Name => "WindMill interior";

        //public override Vector3 SpawnPoint => new Vector3(0f, 0f, -2f);

        //public override Dictionary<int, Vector3> Portals => new() { { 0, new Vector3(0f, 0f, -2f)} };
        public override Dictionary<Type, Vector3> EnteredSpawnPoints => new()
        {
            { typeof(Farm), new Vector3(0f, 0f, -2f) }
        };
    }
}
