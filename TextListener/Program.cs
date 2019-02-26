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
            sub.Subscribe("events", (channel, message) => {
                Console.WriteLine("TextCreated: " + (string)message);
            });
            Console.ReadLine();
        }
    }
}
