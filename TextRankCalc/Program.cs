using System;
using StackExchange.Redis;
using Redis;

namespace TextRankCalc
{
    class Program
    {
        const string COUNTER_HINTS_CHANNEL = "counter_hints";
        const string COUNTER_QUEUE_NAME = "counter_queue";
        static void Main(string[] args)
        {
            IDatabase db = RedisStore.RedisCacheRU;
            var sub = db.Multiplexer.GetSubscriber();
            sub.Subscribe("events", (channel, message) =>
            {
                string id = (string)message;
                Console.WriteLine("TextCreated: " + id);
                string value = db.StringGet(id);
                Console.WriteLine($"{id}:{value}");
                SendMessage($"{id}:{value}", db);
            });
            Console.WriteLine("Obsevable subscribe text rank calc is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static void SendMessage(string message, IDatabase db )
        {
            // put message to queue
            db.ListLeftPush( COUNTER_QUEUE_NAME, message, flags: CommandFlags.FireAndForget );
            // and notify consumers
            db.Multiplexer.GetSubscriber().Publish( COUNTER_HINTS_CHANNEL, "" );
        }
    }
}
