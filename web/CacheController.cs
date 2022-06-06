using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace web
{
    public class CacheController
    {
        private readonly IMemoryCache memoryCache;
        public CacheController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public iplibrary.IpDetails GetCache(string ip)
        {
            iplibrary.IpDetails ipd;
            memoryCache.TryGetValue(ip, out ipd);
            return ipd;
        }
        public string SetCache(iplibrary.IpDetails ipd)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(0.1),
                Size = 1024,
            };
            memoryCache.Set(ipd.Ip, ipd, cacheExpiryOptions);
            return "ok";
        }
        public class CacheRequest
        {
            public string key { get; set; }
            public string value { get; set; }
        }
    }
}
