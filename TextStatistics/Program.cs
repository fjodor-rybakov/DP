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
        public int countRejectRequests;
    }
    
    public class ProcessingAccepted
    {
        public string contextId;
        public bool status;
    }
    
    class Program
    {
        private const string EVENTS = "events";
        private const string TEXT_RANK_CALCULATED_EVENT = "TextRankCalculated";
        private const string PROCESSING_ACCEPTED_EVENT = "ProcessingAccepted";
        private static double allTextStatistics;
        private static int textNum;
        private static int highRankPart;
        private static double avgRank;
        private static int countRejectRequests;
        
        static void Main()
        {
            RedisStore instance = RedisStore.getInstance();
            IDatabase db = instance.RedisCacheTable;
            var sub = db.Multiplexer.GetSubscriber();

            sub.Subscribe(EVENTS, (channel, message) =>
            {
                string mes = message;
                string eventName = mes.Split("=>")[0];
                if (eventName == TEXT_RANK_CALCULATED_EVENT)
                {
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
                }

                if (eventName == PROCESSING_ACCEPTED_EVENT)
                {
                    string mesVal = mes.Split("=>")[1];
                    ProcessingAccepted processingAccepted = JsonConvert.DeserializeObject<ProcessingAccepted>(mesVal);
                    if (!processingAccepted.status)
                    {
                        countRejectRequests++;
                    }
                }

                Console.WriteLine("textNum: " + textNum + ", highRankPart: " + highRankPart + 
                                  ", avgRank: " + avgRank + ", countRejectRequests" + countRejectRequests);

                db.StringSet("text_statistic", GetStringifyTextStatistic(textNum, highRankPart, avgRank, countRejectRequests));
            });
            
            Console.WriteLine("Observable subscribe text statistics is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static string GetStringifyTextStatistic(int textNumValue, int highRankPartValue, double avgRankValue, int countRejectRequestsValue)
        {
            TextStatistic textStatistic = new TextStatistic
            {
                textNum = textNumValue,
                highRankPart = highRankPartValue,
                avgRank = avgRankValue,
                countRejectRequests = countRejectRequestsValue
            };
            
            return JsonConvert.SerializeObject(textStatistic);
        }
    }
}
