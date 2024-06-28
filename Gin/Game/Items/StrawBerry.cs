using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class StrawBerry : Food
    {
        public override int Energy => 12;

        public override int Id => ItemId.STRAWBERRY;

        public override string Name => "StrawBerry";
    }
}
