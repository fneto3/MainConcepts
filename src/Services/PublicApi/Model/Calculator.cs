using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Model
{
    public class Calculator : IRedisKey
    {
        public int Id { get; set; }

        public decimal A { get; set; }

        public decimal B { get; set; }

        public decimal Result { get; set; }

        public string Type { get; set; }

        public RedisKey GetKey()
        {
            return new RedisKey($"Calculator:{Type}:{Id}");
        }
    }
}
