using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Wool : Item
    {
        public override int Id => ItemId.WOOL;

        public override string Name => "'Wool";

        public override string Description => "Used to produce fabric";
    }
}
