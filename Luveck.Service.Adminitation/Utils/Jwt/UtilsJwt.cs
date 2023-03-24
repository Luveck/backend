using Luveck.Service.Administration.Utils.Jwt.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Utils.Jwt
{
    [ExcludeFromCodeCoverage]
    public class UtilsJwt : IUtils
    {

        public readonly IConfiguration configuration;
        private readonly IMemoryCache cache;


        public UtilsJwt(IConfiguration configuration, IMemoryCache cache)
        {
            this.configuration = configuration;
            this.cache = cache;
        }


        public void SaveDataInCache(string key, object data, string parameterTime)
        {
            //IConfiguration conf = configuration.GetSection("ParametersTimeHoursCache");
            //string timeCacheFromHoursToken = conf.GetSection(parameterTime).Value;

            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            cache.Set(key, data, cacheEntryOptions);
        }

        public void RemoveDataInCache(string key)
        {
            cache.Remove(key);
        }


        public T GetDataInCache<T>(object key)
        {
            cache.TryGetValue<T>(key, out T result);

            return result;
        }

    }
}
