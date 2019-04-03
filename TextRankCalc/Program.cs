using System;
using StackExchange.Redis;
using Redis;

namespace TextRankCalc
{
    class Program
    {
        private const string COUNTER_HINTS_CHANNEL = "counter_hints";
        private const string COUNTER_QUEUE_NAME = "counter_queue";
        static void Main()
        {
            IDatabase db = RedisStore.getInstance().RedisCacheTable;
            var sub = db.Multiplexer.GetSubscriber();
            
            sub.Subscribe("events", (channel, message) =>
            {
                string id = (string)message;
                Console.WriteLine("Message: " + id);
                var idDB = (int)db.StringGet($"RANK_{id}");
                var regionDB = RedisStore.getInstance().RedisCache(idDB);
                
                SendMessage(regionDB.StringGet(id), db);
            });
            
            Console.WriteLine("Observable subscribe text rank calc is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static void SendMessage(string message, IDatabase db)
        {
            // put message to queue
            db.ListLeftPush( COUNTER_QUEUE_NAME, message, flags: CommandFlags.FireAndForget );
            // and notify consumers
            db.Multiplexer.GetSubscriber().Publish( COUNTER_HINTS_CHANNEL, "" );
        }
    }
}
