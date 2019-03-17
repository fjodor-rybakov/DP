using StackExchange.Redis;
using System;
using System.Configuration;

namespace Redis
{
    public class RedisStore
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnectionRU;
        private static readonly Lazy<ConnectionMultiplexer> LazyConnectionEU;
        private static readonly Lazy<ConnectionMultiplexer> LazyConnectionUSA;

        static RedisStore()
        {
            var configurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:6379" }
            };

            LazyConnectionRU = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));

            configurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8001" }
            };

            LazyConnectionEU = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));

            configurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8002" }
            };

            LazyConnectionUSA = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer ConnectionRU => LazyConnectionRU.Value;

        public static IDatabase RedisCacheRU => ConnectionRU.GetDatabase();

        public static ConnectionMultiplexer ConnectionEU => LazyConnectionEU.Value;

        public static IDatabase RedisCacheEU => ConnectionEU.GetDatabase();

        public static ConnectionMultiplexer ConnectionUSA => LazyConnectionUSA.Value;

        public static IDatabase RedisCacheUSA => ConnectionUSA.GetDatabase();
    }
}