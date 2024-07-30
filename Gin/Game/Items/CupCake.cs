using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class CupCake : Food
    {
        public override int Energy => 80;

        public override int Id => ItemId.CUP_CAKE;

        public override string Name => "Cup Cake";
    }
}
