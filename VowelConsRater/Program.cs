﻿using System;
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
    class Program
    {
        const string RATE_HINTS_CHANNEL = "rate_hints";
        const string RATE_QUEUE_NAME = "rate_queue";
        static void Main(string[] args)
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
                    string message = getStrigifyUserDataRater(userDataCounter, relation);

                    instance.RedisCache(userDataCounter.region).StringSet($"RANK_{userDataCounter.id}", message);

                    msg = db.ListRightPop(RATE_QUEUE_NAME);
                }
            });
            Console.WriteLine("Obsevable subscribe vowel cons rater is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static string getStrigifyUserDataRater(UserDataCounter userDataCounter, double relation) {
            UserDataRater userDataReter = new UserDataRater();
            userDataReter.id = userDataCounter.id;
            userDataReter.region = userDataCounter.region;
            userDataReter.message = userDataCounter.message;
            userDataReter.relation = relation;

            return JsonConvert.SerializeObject(userDataReter);
        } 
    }
}
