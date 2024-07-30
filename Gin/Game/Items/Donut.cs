using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Donut : Food
    {
        public override int Energy => 30;

        public override int Id => ItemId.DONUT;

        public override string Name => "Donut";
    }
}
