using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class PigSpawner : CreatureSpawner
    {
        public override List<int> StageGrowingTimes => new() { 50 };

        public override int FeedTime => 3;

        public override int ProduceTime => 5;

        public override Item HarvestProduct => ItemReplicator.Get(ItemId.TRUFFLE);

        public override int Id => ItemId.PIG_SPAWNER;

        public override string Name => "Pig";

        public override string CreatureName => "Pig";
    }
}
