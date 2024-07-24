using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class TruffleOil : Food
    {
        public override int Energy => 25;

        public override int Id => ItemId.TRUFFLE_OIL;

        public override string Name => "Truffle Oil";
    }
}
