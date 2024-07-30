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
            Register(new BlackBerry());
            Register(new BlackBerrySeed());
            Register(new BokChoy());
            Register(new BokChoySeed());
            Register(new Carrot());
            Register(new CarrotSeed());
            Register(new Corn());
            Register(new CornnSeed());
            Register(new EggPlant());
            Register(new EggPlantSeed());
            Register(new Grapes());
            Register(new GrapesSeed());
            Register(new Lettuce());
            Register(new LettuceSeed());
            Register(new Pepper());
            Register(new PepperSeed());
            Register(new Pumpkin());
            Register(new PumpkinSeed());
            Register(new Radish());
            Register(new RadishSeed());
            Register(new StrawBerry());
            Register(new StrawBerrySeed());
            Register(new Turnip());
            Register(new TurnipSeed());
            Register(new WaterMelon());
            Register(new WaterMelonSeed());
            Register(new NormalFertilizer());
            Register(new FertilizedChickenEgg());
            Register(new ChickenEgg());
            Register(new AnimalFoodPack());
            Register(new DuckSpawner());
            Register(new CowSpawner());
            Register(new SheepSpawner());
            Register(new PigSpawner());
            Register(new DuckEgg());
            Register(new Milk());
            Register(new Fur());
            Register(new Truffle());
            Register(new Mayonnaise());
            Register(new Cheese());
            Register(new Wool());
            Register(new TruffleOil());
            Register(new Chocolate());
            Register(new CupCake());
            Register(new Cake());
            Register(new IceCream());
            Register(new Donut());
            Register(new Cookie());
            Register(new Brownie());
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
        public readonly static int POTATO_SEED = 2;
        public readonly static int BLACKBERRY_SEED = 3;
        public readonly static int BOKCHOY_SEED = 4;
        public readonly static int CARROT_SEED = 5;
        public readonly static int CORN_SEED = 6;
        public readonly static int EGGPLANT_SEED = 7;
        public readonly static int GRAPES_SEED = 8;
        public readonly static int LETTUCE_SEED = 9;
        public readonly static int PEPPER_SEED = 10;
        public readonly static int PUMPKIN_SEED = 11;
        public readonly static int RADISH_SEED = 12;
        public readonly static int STRAWBERRY_SEED = 13;
        public readonly static int TURNIP_SEED = 14;
        public readonly static int WATERMELON_SEED = 15;

        public readonly static int TOMATO = 101;
        public readonly static int POTATO = 102;
        public readonly static int BLACKBERRY = 103;
        public readonly static int BOKCHOY = 104;
        public readonly static int CARROT = 105;
        public readonly static int CORN = 106;
        public readonly static int EGGPLANT = 107;
        public readonly static int GRAPES = 108;
        public readonly static int LETTUCE = 109;
        public readonly static int PEPPER = 110;
        public readonly static int PUMPKIN = 111;
        public readonly static int RADISH = 112;
        public readonly static int STRAWBERRY = 113;
        public readonly static int TURNIP = 114;
        public readonly static int WATERMELON = 115;

        public readonly static int NORMAL_FERTILIZER = 201;
        public readonly static int ANIMAL_FOOD_PACK = 202;

        public readonly static int FERTILIZED_CHICKEN_EGG = 301;
        public readonly static int DUCK_SPAWNER = 302;
        public readonly static int COW_SPAWNER = 303;
        public readonly static int SHEEP_SPAWNER = 304;
        public readonly static int PIG_SPAWNER = 305;

        public readonly static int CHICKEN_EGG = 401;
        public readonly static int DUCK_EGG = 402;
        public readonly static int COW_MILK_JAR = 403;
        public readonly static int FUR = 404;
        public readonly static int TRUFFLE = 405;
        public readonly static int MAYONNAISE = 406;
        public readonly static int CHEESE = 407;
        public readonly static int WOOL = 408;
        public readonly static int TRUFFLE_OIL = 409;
        public readonly static int CHOCOLATE = 410;
        public readonly static int CAKE = 411;
        public readonly static int COOKIE = 412;
        public readonly static int CUP_CAKE = 413;
        public readonly static int DONUT = 414;
        public readonly static int ICE_CREAM = 415;
        public readonly static int BROWNIE = 416;
    }
}
