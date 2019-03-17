using System;
using Redis;
using StackExchange.Redis;
using System.Text.RegularExpressions;

namespace VowelConsCounter
{
    class Program
    {
        const string COUNTER_HINTS_CHANNEL = "counter_hints";
        const string COUNTER_QUEUE_NAME = "counter_queue";
        const string RATE_HINTS_CHANNEL = "rate_hints";
        const string RATE_QUEUE_NAME = "rate_queue";
        static void Main(string[] args)
        {
            IDatabase db = RedisStore.RedisCacheRU;
            var sub = db.Multiplexer.GetSubscriber();
            sub.Subscribe(COUNTER_HINTS_CHANNEL, delegate
            {
                // process all messages in queue
                string msg = db.ListRightPop(COUNTER_QUEUE_NAME);
                while (msg != null)
                {
                    string id = msg.Split(':')[0];
                    string str = msg.Split(':')[1];
                    Console.WriteLine(msg);
                    int countGlasn = Regex.Matches(str, @"[aiueoy]", RegexOptions.IgnoreCase).Count;
                    int countSoglasn = Regex.Matches(str, @"[bcdfghjklmnpqrstvwxz]", RegexOptions.IgnoreCase).Count;

                    SendMessage($"{id}:{countGlasn}:{countSoglasn}", db);

                    msg = db.ListRightPop(COUNTER_QUEUE_NAME);
                }
            });
            Console.WriteLine("Obsevable subscribe vowel cons counter is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static void SendMessage(string message, IDatabase db)
        {
            // put message to queue
            db.ListLeftPush(RATE_QUEUE_NAME, message, flags: CommandFlags.FireAndForget);
            // and notify consumers
            db.Multiplexer.GetSubscriber().Publish(RATE_HINTS_CHANNEL, "");
        }       
    }
}
