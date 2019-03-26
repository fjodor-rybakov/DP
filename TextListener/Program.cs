using System;
using Redis;
using StackExchange.Redis;

namespace TextListener
{
    class Program
    {
        static void Main(string[] args)
        {
            IDatabase redis = RedisStore.getInstance().RedisCache("ru");
            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("events", (channel, message) =>
            {
                Console.WriteLine("TextCreated: " + (string)message);
                string res = redis.StringGet((string)message);
                Console.WriteLine("Value: " + res);
            });
            Console.WriteLine("Obsevable subscribe text listener is ready. For exit press Enter.");
            Console.ReadLine();
        }
    }
}
