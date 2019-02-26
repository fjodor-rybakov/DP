using StackExchange.Redis;
using System;
using System.Configuration;

namespace TextListener
{
    public class RedisStore
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisStore()
        {
            var configurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { "localhost:6379" }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();
    }
}