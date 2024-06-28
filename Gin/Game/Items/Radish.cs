using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Radish : Food
    {
        public override int Energy => 20;

        public override int Id => ItemId.RADISH;

        public override string Name => "Radish";
    }
}
