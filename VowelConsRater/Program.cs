using System;
using StackExchange.Redis;
using Redis;

namespace VowelConsRater
{
    class Program
    {
        const string RATE_HINTS_CHANNEL = "rate_hints";
        const string RATE_QUEUE_NAME = "rate_queue";
        static void Main(string[] args)
        {
            IDatabase db = RedisStore.RedisCacheRU;
            var sub = db.Multiplexer.GetSubscriber();
            sub.Subscribe(RATE_HINTS_CHANNEL, delegate
            {
                // process all messages in queue
                string msg = db.ListRightPop(RATE_QUEUE_NAME);
                while (msg != null)
                {
                    Console.WriteLine(msg);
                    string id = msg.Split(':')[0];
                    double countGlasn = Convert.ToDouble(msg.Split(':')[1]);
                    double countSoglasn = Convert.ToDouble(msg.Split(':')[2]);
                    double relation = countSoglasn == 0 ? 0 : countGlasn / countSoglasn;

                    db.StringSet($"RANK_{id}", relation);

                    msg = db.ListRightPop(RATE_QUEUE_NAME);
                }
            });
            Console.WriteLine("Obsevable subscribe vowel cons rater is ready. For exit press Enter.");
            Console.ReadLine();
        }
    }
}
