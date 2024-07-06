using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Truffle : Food
    {
        public override int Energy => 70;

        public override int Id => ItemId.TRUFFLE;

        public override string Name => "Truffle";
    }
}
