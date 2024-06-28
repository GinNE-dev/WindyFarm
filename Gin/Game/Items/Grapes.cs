using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Grapes : Food
    {
        public override int Energy => 10;

        public override int Id => ItemId.GRAPES;

        public override string Name => "Grapes";
    }
}
