using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Cake : Food
    {
        public override int Energy => 120;

        public override int Id => ItemId.CAKE;

        public override string Name => "Cake";
    }
}
