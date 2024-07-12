using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class DuckEgg : Food
    {
        public override int Energy => 25;

        public override int Id => ItemId.DUCK_EGG;

        public override string Name => "Duck Egg";
    }
}
