using StackExchange.Redis;
using System;
using System.Configuration;

namespace Redis
{
    public class RedisStore
    {
        private static Lazy<ConnectionMultiplexer> LazyConnectionTable;
        private static Lazy<ConnectionMultiplexer> LazyConnectionRU;
        private static Lazy<ConnectionMultiplexer> LazyConnectionEU;
        private static Lazy<ConnectionMultiplexer> LazyConnectionUSA;
        private static RedisStore _instance = null; 

        public static RedisStore getInstance()
        {
            if (_instance == null) 
            {
                _instance = new RedisStore();
                setSettings();
            }

            return _instance;
        }

        private RedisStore() {}

        public static string SeatchValueById(string id) {
            var dbrus = ConnectionRU.GetDatabase();
            var dbeu = ConnectionEU.GetDatabase();
            var dbusa = ConnectionUSA.GetDatabase();

            if (dbrus.KeyExists(id)) {
                return dbrus.StringGet(id);
            }

            if (dbeu.KeyExists(id)) {
                return dbeu.StringGet(id);
            }

            if (dbusa.KeyExists(id)) {
                return dbusa.StringGet(id);
            }

            return null;
        }

        private static void setSettings()
        {
            var configurationOptionsTable = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:6379" }
            };

            RedisStore.LazyConnectionTable = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsTable));

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

            var configurationOptionsRU = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8003" }
            };

            RedisStore.LazyConnectionRU = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsRU));
        }

        public IDatabase RedisCache(string contextId) {
            Console.WriteLine("Region: " + contextId);
            switch(contextId)
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

        public ConnectionMultiplexer ConnectionTable => LazyConnectionTable.Value;
        public IDatabase RedisCacheTable => ConnectionTable.GetDatabase();
        private static ConnectionMultiplexer ConnectionRU => LazyConnectionRU.Value;
        private static IDatabase RedisCacheRU => ConnectionRU.GetDatabase();
        private static ConnectionMultiplexer ConnectionEU => LazyConnectionEU.Value;
        private static IDatabase RedisCacheEU => ConnectionEU.GetDatabase();
        private static ConnectionMultiplexer ConnectionUSA => LazyConnectionUSA.Value;
        private static IDatabase RedisCacheUSA => ConnectionUSA.GetDatabase();
    }
}