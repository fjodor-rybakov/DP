using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Redis;

namespace TextProcessingLimiter
{
    public class ProcessingAccepted
    {
        public string contextId;
        public bool status;
    }
    
    class Program
    {
        private const string TEXT_CREATED_EVENT = "TextCreated";
        private const string PROCESSING_ACCEPTED_EVENT = "ProcessingAccepted";
        private const string EVENTS = "events";
        
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Not enough arguments. Must set count limit words");
                return;
            }
            
            var instance = RedisStore.getInstance();
            var db = instance.RedisCacheTable;
            var sub = db.Multiplexer.GetSubscriber();
            var limitCountWords = int.Parse(args[0]);
            var countWords = 0;
            bool status;
            
            sub.Subscribe(EVENTS, (channel, message) =>
            {
                string mes = message;
                string eventName = mes.Split("=>")[0];
                if (eventName != TEXT_CREATED_EVENT) return;
                var contextId = mes.Split("=>")[1];
                countWords++;
                status = countWords <= limitCountWords;
                sub.Publish(EVENTS, PublishMessage(contextId, status));
                if (!status)
                    Task.Run(async () => {
                        await Task.Delay(60000);
                        countWords = 0;
                        Console.WriteLine("Reset!");
                    });
            });
           
            
            Console.WriteLine(countWords);
            Console.WriteLine("Observable subscribe text processing limiter is ready. For exit press Enter.");
            Console.ReadLine();
        }

        private static string PublishMessage(string contextId, bool status)
        {
            ProcessingAccepted processingAccepted = new ProcessingAccepted { contextId = contextId, status = status };
            return PROCESSING_ACCEPTED_EVENT + "=>" + JsonConvert.SerializeObject(processingAccepted);
        }
    }
}
