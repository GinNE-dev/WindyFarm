using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Maps
{
    public class Farm : PrivateMap
    {
        public override int Id => 0;

        public override string Name => "Farm";

        public Vector3 SpawnPoint => new Vector3(65f, 0.1f, 22f);

        //public override Dictionary<int, Vector3> Portals => new() { {1, new Vector3(79f, 0.1f, 17f)}, { 2, new Vector3(55f, 0.1f, 41f) } };
        public override Dictionary<Type, Vector3> EnteredSpawnPoints => new() 
        { 
            { typeof(Town), new Vector3(79f, 0.1f, 17f) }, 
            { typeof(InteriorWindMill), new Vector3(55f, 0.1f, 41f) } 
        };
    }
}
