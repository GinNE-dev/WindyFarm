using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.ServerLog;

namespace WindyFarm.Gin.Network.Utils
{
    public class JsonHelper
    {
        public static string ParseString<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T? ParseObject<T>(string data)
        {
            try
            {
                T? result = JsonConvert.DeserializeObject<T>(data);
                return result;
            }
            catch (Exception ex)
            {
                GinLogger.Fatal("JsonHelper:ParseObject", ex);
            }

            return default;
        }
    }
}
