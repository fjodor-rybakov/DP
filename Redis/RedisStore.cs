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

        public int GetNumDB(string region) {
            switch (region)
            {
                case "ru": 
                    return 1;
                case "eu": 
                    return 2;
               case "usa": 
                    return 3;
                default:
                    return 1;
            }
        }

        public static string SearchValueById(string id, int idDb) {
            var dbrus = ConnectionRU.GetDatabase();
            var dbeu = ConnectionEU.GetDatabase();
            var dbusa = ConnectionUSA.GetDatabase();

            switch (idDb)
            {
                case 1 when dbrus.KeyExists(id):
                    return dbrus.StringGet(id);
                case 2 when dbeu.KeyExists(id):
                    return dbeu.StringGet(id);
                case 3 when dbusa.KeyExists(id):
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

        public IDatabase RedisCache(int idDb) {
            Console.WriteLine("idDb: " + idDb);
            switch(idDb)
            {
                case 1: 
                    return ConnectionRU.GetDatabase();
                case 2: 
                    return ConnectionEU.GetDatabase();
                case 3: 
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