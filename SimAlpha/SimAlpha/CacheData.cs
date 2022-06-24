using System;
using System.Runtime.Caching;

namespace SimAlpha
{
    internal class CacheData
    {
        public static MemoryCache CACHE = new("CacheData");

        internal static void SaveData()
        {
            var cacheItem = new CacheItem(Guid.NewGuid().ToString(), TSData.JSON);
            var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60.0) };
            CACHE.Add(cacheItem, cacheItemPolicy);
        }
    }
}