using Microsoft.Extensions.Logging;
using PublicApi.Infrasctructure.Repositories;
using PublicApi.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Infrasctructure
{
    public class CalculatorContext
    {
        private readonly RedisRepository<Calculator> _redis;
        private readonly ILogger<Calculator> _logger;

        public CalculatorContext(RedisRepository<Calculator> redis, ILogger<Calculator> logger)
        {
            _redis = redis;
            _logger = logger;
        }


    }
}
