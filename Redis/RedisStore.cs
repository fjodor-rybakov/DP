using StackExchange.Redis;
using System;
using System.Configuration;

namespace Redis
{
    public class RedisStore
    {
        private static Lazy<ConnectionMultiplexer> LazyConnectionRU;
        private static Lazy<ConnectionMultiplexer> LazyConnectionEU;
        private static Lazy<ConnectionMultiplexer> LazyConnectionUSA;
        public static string Region { get; set; }
        private static RedisStore _instance = null; 

        public static RedisStore getInstance()
        {
            if (_instance == null) 
            {
                _instance = new RedisStore();
                setInstance();
            }

            return _instance;
        }

        private RedisStore() {}

        private static void setInstance()
        {
            var configurationOptionsRU = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:6379" }
            };

            RedisStore.LazyConnectionRU = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsRU));

            var configurationOptionsEU = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8001" }
            };

            RedisStore.LazyConnectionEU = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsEU));

            var configurationOptionsUSA = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8002" }
            };

            RedisStore.LazyConnectionUSA = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsUSA));
        }

        public IDatabase RedisCache() {
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