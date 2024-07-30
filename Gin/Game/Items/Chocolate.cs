using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Chocolate : Food
    {
        public override int Energy => 40;

        public override int Id => ItemId.CHOCOLATE;

        public override string Name => "Chocolate";
    }
}
