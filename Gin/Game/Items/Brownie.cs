using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Brownie : Food
    {
        public override int Energy => 60;

        public override int Id => ItemId.BROWNIE;

        public override string Name => "Brownie";
    }
}
