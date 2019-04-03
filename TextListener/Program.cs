using System;
using Redis;
using StackExchange.Redis;

namespace TextListener
{
    class Program
    {
        static void Main()
        {
            IDatabase redis = RedisStore.getInstance().RedisCacheTable;
            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("events", (channel, message) =>
            {
                string id = message;
                var contextId = $"RANK_{id}";
                int idDB = (int)redis.StringGet(contextId);
                
                var regionDB = RedisStore.getInstance().RedisCache(idDB);
                Console.WriteLine("TextCreated: " + regionDB.StringGet(id));
            });
            Console.WriteLine("Observable subscribe text listener is ready. For exit press Enter.");
            Console.ReadLine();
        }
    }
}
