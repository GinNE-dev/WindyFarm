using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Cheese : Food
    {
        public override int Energy => 120;

        public override int Id => ItemId.CHEESE;

        public override string Name => "Cheese";
    }
}
