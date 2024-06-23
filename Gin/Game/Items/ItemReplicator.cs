using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Items
{
    public class ItemReplicator
    {
        private static readonly Dictionary<int, Item> _items = new();

        public static void Init()
        {
            Register(new VoidItem());
            Register(new TomatoSeed());
            Register(new Tomato());
            Register(new PotatoSeed());
            Register(new Potato());
        }

        public static bool Register(Item item)
        {
            try
            {
                _items.Add(item.Id, item);
            }
            catch (Exception ex)
            {
                GinLogger.Warning(ex.StackTrace is not null ? ex.StackTrace:ex.Message);
            }
            
            return false;
        }

        public static Item Get(int itemId)
        {
            var sample =_items.GetValueOrDefault(itemId, new VoidItem());
            return (Item) sample.Clone();
        }
    }

    public abstract class ItemId
    {
        public readonly static int VOID_ITEM = 0;
        public readonly static int TOMATO_SEED = 1;
        public readonly static int TOMATO = 2;
        public readonly static int POTATO_SEED = 3;
        public readonly static int POTATO = 4;
    }
}
