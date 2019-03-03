using System;
using StackExchange.Redis;

namespace TextListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("events", (channel, message) =>
            {
                Console.WriteLine("TextCreated: " + (string)message);
                string res = redis.StringGet((string)message);
                Console.WriteLine("Value: " + res);
            });
            Console.WriteLine("Obsevable subscribe Text listener is ready. For exit press Enter.");
            Console.ReadLine();
        }
    }
}
