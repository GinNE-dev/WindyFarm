using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Utils
{
    public class CultureCode
    {
        public static readonly string enUS = "en-US";
        public static readonly string Default = enUS;
    }

    public class TextDictionary
    {
        private static ResourceManager? TextResourceManager;
        public static void Setup(string cultureCode)
        {
            CultureInfo newCulture = new(cultureCode);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            string langDir = $"{Assembly.GetExecutingAssembly().GetName().Name}.Resources.lang.{cultureCode}.Resource";
            TextResourceManager = new(langDir, typeof(Program).Assembly);
        }

        public static string Get(string key)
        {
            if (TextResourceManager == null) return "TextDictionary setup is not complete!";
            if(key ==  null) return string.Empty;

            string? value = TextResourceManager.GetString(key);
            return value is null ? string.Empty : value;
        }
    }
}
