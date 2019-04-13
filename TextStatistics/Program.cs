using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StackExchange.Redis;
using Redis;

namespace TextStatistics
{
    public class TextStatisticConsRater
    {
        public string contextId;
        public double rank;
    }

    public class TextStatistic
    {
        public int textNum;
        public int highRankPart;
        public double avgRank;
    }
    
    class Program
    {
        private const string EVENTS = "events";
        private static double allTextStatistics;
        private static int textNum;
        private static int highRankPart;
        private static double avgRank;
        
        static void Main()
        {
            RedisStore instance = RedisStore.getInstance();
            IDatabase db = instance.RedisCacheTable;
            var sub = db.Multiplexer.GetSubscriber();

            sub.Subscribe(EVENTS, (channel, message) =>
            {
                string mes = message;
                if (mes.Split("=>")[0] != "TextRankCalculated") return;
                string mesVal = mes.Split("=>")[1];
                TextStatisticConsRater textStatisticConsRater = JsonConvert.DeserializeObject<TextStatisticConsRater>(mesVal);
                Console.WriteLine(message);
                allTextStatistics += textStatisticConsRater.rank;
                textNum++;

                if (textStatisticConsRater.rank > 0.5)
                {
                    highRankPart++;
                }

                avgRank = allTextStatistics / textNum;
                
                Console.WriteLine("textNum: " + textNum + ", highRankPart: " + highRankPart + ", avgRank: " + avgRank);

                db.StringSet("text_statistic", GetStringifyTextStatistic(textNum, highRankPart, avgRank));
            });
            
            Console.WriteLine("Observable subscribe text statistics is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static string GetStringifyTextStatistic(int textNumValue, int highRankPartValue, double avgRankValue)
        {
            TextStatistic textStatistic = new TextStatistic
            {
                textNum = textNumValue,
                highRankPart = highRankPartValue,
                avgRank = avgRankValue
            };
            
            return JsonConvert.SerializeObject(textStatistic);
        }
    }
}
