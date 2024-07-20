using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Maps
{
    public class MapManager
    {
        private static MapManager? _instacnce;
        public static MapManager Instance 
        { get
            {
                _instacnce ??= new MapManager();
                return _instacnce;
            } 
        }

        private ConcurrentDictionary<int, Map> _maps;
        private MapManager() 
        {
            _maps = new();
        }

        public void Init()
        {
            Add(new Farm());
            Add(new Town());
            Add(new InteriorWindMill());
        }

        public Map? Get(int id)
        {
            _maps.TryGetValue(id, out Map? got);
            return got;
        }

        public void Add(Map map)
        {
            if (map is null) return;
            _maps.TryAdd(map.Id, map);
        }
    }
}
