using StackExchange.Redis;
using System;
using System.Configuration;
using System.Collections.Concurrent;

namespace Redis
{
    public class RedisStore
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnectionTable;
        private static Lazy<ConnectionMultiplexer> _lazyConnectionRu;
        private static Lazy<ConnectionMultiplexer> _lazyConnectionEu;
        private static Lazy<ConnectionMultiplexer> _lazyConnectionUsa;
        private static RedisStore _instance; 
        private static readonly ConcurrentDictionary<string, string> Data = new ConcurrentDictionary<string, string>();

        public static RedisStore getInstance()
        {
            if (_instance != null) return _instance;
            _instance = new RedisStore();
            setSettings();

            return _instance;
        }

        private RedisStore() {}

        public void addValue(string key, string value) {
            Data[key] = value;
        }

        public string getValue(string key) {
            return Data[key];
        }

        public static string SearchValueById(string id, string region) {
            var dbrus = ConnectionRU.GetDatabase();
            var dbeu = ConnectionEU.GetDatabase();
            var dbusa = ConnectionUSA.GetDatabase();

            switch (region)
            {
                case "rus" when dbrus.KeyExists(id):
                    return dbrus.StringGet(id);
                case "eu" when dbeu.KeyExists(id):
                    return dbeu.StringGet(id);
                case "usa" when dbusa.KeyExists(id):
                    return dbusa.StringGet(id);
                default:
                    return null;
            }
        }

        private static void setSettings()
        {
            var configurationOptionsTable = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:6379" }
            };

            _lazyConnectionTable = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsTable));

            var configurationOptionsEU = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8001" }
            };

            _lazyConnectionEu = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsEU));

            var configurationOptionsUSA = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8002" }
            };

            _lazyConnectionUsa = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsUSA));

            var configurationOptionsRU = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:8003" }
            };

            _lazyConnectionRu = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptionsRU));
        }

        public IDatabase RedisCache(string region) {
            Console.WriteLine("Region: " + region);
            switch(region)
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

        public ConnectionMultiplexer ConnectionTable => _lazyConnectionTable.Value;
        public IDatabase RedisCacheTable => ConnectionTable.GetDatabase();
        private static ConnectionMultiplexer ConnectionRU => _lazyConnectionRu.Value;
        private static IDatabase RedisCacheRU => ConnectionRU.GetDatabase();
        private static ConnectionMultiplexer ConnectionEU => _lazyConnectionEu.Value;
        private static IDatabase RedisCacheEU => ConnectionEU.GetDatabase();
        private static ConnectionMultiplexer ConnectionUSA => _lazyConnectionUsa.Value;
        private static IDatabase RedisCacheUSA => ConnectionUSA.GetDatabase();
    }
}