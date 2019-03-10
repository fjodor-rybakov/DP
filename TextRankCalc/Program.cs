using System;
using System.Text.RegularExpressions;
using Redis;

namespace TextRankCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = RedisStore.RedisCache;
            var sub = db.Multiplexer.GetSubscriber();
            sub.Subscribe("events", (channel, message) =>
            {
                string id = (string)message;
                Console.WriteLine("TextCreated: " + id);
                string str = db.StringGet(id);

                if (Regex.IsMatch(str, "[А-Яа-я]")) 
                    Console.WriteLine("String contain unknown char");
                else
                {
                    int countGlasn = Regex.Matches(str, @"[aiueoy]", RegexOptions.IgnoreCase).Count;
                    int countSoglasn = Regex.Matches(str, @"[bcdfghjklmnpqrstvwxz]", RegexOptions.IgnoreCase).Count;
                    double relation = countSoglasn == 0 ? 0 : (double)countGlasn / (double)countSoglasn;

                    db.StringSet($"RANK_{id}", relation);
                }
                Console.WriteLine("Value: " + str);
            });
            Console.WriteLine("Obsevable subscribe text rank calc is ready. For exit press Enter.");
            Console.ReadLine();
        }
    }
}
