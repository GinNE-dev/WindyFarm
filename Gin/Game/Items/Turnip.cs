using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Turnip : Food
    {
        public override int Energy => 11;

        public override int Id => ItemId.TURNIP;

        public override string Name => "Turnip";
    }
}
