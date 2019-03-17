using StackExchange.Redis;
using System;
using System.Configuration;

namespace Redis
{
    public static class RedisStore
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnectionRU;
        private static readonly Lazy<ConnectionMultiplexer> LazyConnectionEU;
        private static readonly Lazy<ConnectionMultiplexer> LazyConnectionUSA;
        public static string Region { get; set; }

        static RedisStore()
        {
            var configurationOptionsRU = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:6379" }
            };

            LazyConnectionRU = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsRU));

            var configurationOptionsEU = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8001" }
            };

            LazyConnectionEU = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsEU));

            var configurationOptionsUSA = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8002" }
            };

            LazyConnectionUSA = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsUSA));
        }

        public static IDatabase RedisCache() {
            Console.WriteLine("Region: " + RedisStore.Region);
            switch(RedisStore.Region)
            {
                case "rus": 
                    return ConnectionRU.GetDatabase();
                case "eu": 
                    return ConnectionEU.GetDatabase();
                case "usa": 
                    return ConnectionUSA.GetDatabase();
                default: 
                    return ConnectionRU.GetDatabase();
            }
        }

        private static ConnectionMultiplexer ConnectionRU => LazyConnectionRU.Value;

        private static IDatabase RedisCacheRU => ConnectionRU.GetDatabase();

        private static ConnectionMultiplexer ConnectionEU => LazyConnectionEU.Value;

        private static IDatabase RedisCacheEU => ConnectionEU.GetDatabase();

        private static ConnectionMultiplexer ConnectionUSA => LazyConnectionUSA.Value;

        private static IDatabase RedisCacheUSA => ConnectionUSA.GetDatabase();
    }
}