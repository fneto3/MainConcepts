using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Model
{
    public interface IRedisKey
    {
        RedisKey GetKey();
    }
}
