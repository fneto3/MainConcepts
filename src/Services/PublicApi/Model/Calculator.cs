using PublicApi.Model.Interface;
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

        public RedisValue GetKey()
        {
            return $"C{Id}";
        }

        public RedisKey GetHashKey()
        {
            return new RedisKey(Type);
        }

        public enum CalculatorTypes
        {
            Addition = 1,
            Subtraction = 2,
            Division = 3,
            Multiplication = 4
        }
    }
}
