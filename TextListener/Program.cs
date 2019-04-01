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
                Console.WriteLine("TextCreated: " + (string)message);
            });
            Console.WriteLine("Observable subscribe text listener is ready. For exit press Enter.");
            Console.ReadLine();
        }
    }
}
