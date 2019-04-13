using System;
using StackExchange.Redis;
using Newtonsoft.Json;
using Redis;

namespace VowelConsRater
{
    public class UserData {
        public string id;
        public string message;
        public string region;
    }

    public class UserDataRater : UserData {
        public double relation;
    }

    public class UserDataCounter : UserData {
        public int countGlasn;
        public int countSoglasn;
    }

    public class TextRankCalculated
    {
        public string contextId;
        public double rank;
    }
    
    class Program
    {
        private const string RATE_HINTS_CHANNEL = "rate_hints";
        private const string RATE_QUEUE_NAME = "rate_queue";
        private const string EVENTS = "events";
        static void Main()
        {
            RedisStore instance = RedisStore.getInstance();
            IDatabase db = instance.RedisCacheTable;
            var sub = db.Multiplexer.GetSubscriber();

            sub.Subscribe(RATE_HINTS_CHANNEL, delegate
            {
                // process all messages in queue
                string msg = db.ListRightPop(RATE_QUEUE_NAME);
                while (msg != null)
                {
                    Console.WriteLine(msg);
                    UserDataCounter userDataCounter = JsonConvert.DeserializeObject<UserDataCounter>(msg);

                    double relation = userDataCounter.countSoglasn == 0 ? 
                    0 : (double)userDataCounter.countGlasn / (double)userDataCounter.countSoglasn;
                    string message = GetStringifyUserDataRater(userDataCounter, relation);
                    var idDB = (int)db.StringGet($"RANK_{userDataCounter.id}");

                    instance.RedisCache(idDB).StringSet($"RANK_{userDataCounter.id}", message);

                    msg = db.ListRightPop(RATE_QUEUE_NAME);
                    
                    Console.WriteLine(GetStringifyTextStatisticConsRater(userDataCounter.id, relation));

                    sub.Publish(EVENTS, GetStringifyTextStatisticConsRater(userDataCounter.id, relation));
                }
            });
            Console.WriteLine("Observable subscribe vowel cons rater is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static string GetStringifyUserDataRater(UserDataCounter userDataCounter, double relation) {
            UserDataRater userDataRater = new UserDataRater
            {
                id = userDataCounter.id,
                region = userDataCounter.region,
                message = userDataCounter.message,
                relation = relation
            };

            return JsonConvert.SerializeObject(userDataRater);
        }
        
        private static string GetStringifyTextStatisticConsRater(string contextId, double rank) {
            TextRankCalculated textRankCalculated = new TextRankCalculated
            {
                contextId = contextId, 
                rank = rank
            };

            return "TextRankCalculated=>" + JsonConvert.SerializeObject(textRankCalculated);
        }
    }
}
