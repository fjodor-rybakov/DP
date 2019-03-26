using System;
using Redis;
using StackExchange.Redis;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace VowelConsCounter
{
    public class UserData {
        public string id;
        public string message;
        public string region;
    }

    public class UserDataCounter : UserData {
        public int countGlasn;
        public int countSoglasn;
    }

    class Program
    {
        const string COUNTER_HINTS_CHANNEL = "counter_hints";
        const string COUNTER_QUEUE_NAME = "counter_queue";
        const string RATE_HINTS_CHANNEL = "rate_hints";
        const string RATE_QUEUE_NAME = "rate_queue";
        static void Main(string[] args)
        {
            IDatabase db = RedisStore.getInstance().RedisCacheTable;
            var sub = db.Multiplexer.GetSubscriber();

            sub.Subscribe(COUNTER_HINTS_CHANNEL, delegate
            {
                // process all messages in queue
                string msg = db.ListRightPop(COUNTER_QUEUE_NAME);
                while (msg != null)
                {
                    UserData userData = JsonConvert.DeserializeObject<UserData>(msg);
                    Console.WriteLine("Region: " + userData.region + " message: " + msg);

                    int countGlasn = Regex.Matches(userData.message, @"[aiueoy]", RegexOptions.IgnoreCase).Count;
                    int countSoglasn = Regex.Matches(userData.message, @"[bcdfghjklmnpqrstvwxz]", RegexOptions.IgnoreCase).Count;
                    string message = getStrigifyUserDataCounter(userData, countGlasn, countSoglasn);
                    Console.WriteLine("Queue value: " + message);

                    SendMessage(message, db);

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

        private static string getStrigifyUserDataCounter(UserData userData, int countGlasn, int countSoglasn) {
            UserDataCounter userDataCounter = new UserDataCounter();
            userDataCounter.id = userData.id;
            userDataCounter.region = userData.region;
            userDataCounter.message = userData.message;
            userDataCounter.countGlasn = countGlasn;
            userDataCounter.countSoglasn = countSoglasn;

            return JsonConvert.SerializeObject(userDataCounter);
        } 
    }
}
