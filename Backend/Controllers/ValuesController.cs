using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Newtonsoft.Json;
using Redis;

namespace Backend.Controllers
{
    public class UserDataRegion {
        public string message;
        public string region;
    }

    public class UserData : UserDataRegion {
        public string id;
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private const string EVENTS = "events";
        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string value = null;
            bool isError = true;
            var instance = RedisStore.getInstance();
            var tableDB = instance.RedisCacheTable;

            for (int i = 0; i < 3; i++)
            {
                int idDB = (int)tableDB.StringGet($"RANK_{id}");
                value = RedisStore.SearchValueById($"RANK_{id}", idDB);
                if (value != null)
                {
                    isError = false;
                    break;
                }

                Thread.Sleep(500);
            }

            Console.WriteLine(value);

            if (isError)
            {
                Console.WriteLine("Произошла ошибка");
            }

            return value;
        }

        // GET api/values
        [HttpGet]
        public string GetTextStatistic()
        {
            var instance = RedisStore.getInstance();
            var db = instance.RedisCacheTable;
            bool isError = true;
            string value = null;
            
            for (int i = 0; i < 3; i++)
            {
                value = db.StringGet("text_statistic");
                if (value != null)
                {
                    isError = false;
                    break;
                }

                Thread.Sleep(500);
            }
            
            if (isError)
            {
                Console.WriteLine("Произошла ошибка");
            }

            return value;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]string json)
        {
            var id = Guid.NewGuid().ToString();
            UserDataRegion userDataRegion = JsonConvert.DeserializeObject<UserDataRegion>(json);
            var contextId = $"RANK_{id}";
            var instance = RedisStore.getInstance();

            var idDB = instance.GetNumDB(userDataRegion.region);
            var db = instance.RedisCacheTable;
            var regionDb = instance.RedisCache(idDB);
            db.StringSet(contextId, idDB);
            regionDb.StringSet(id, GetStringifyUserData(userDataRegion, id));
            
            var pub = db.Multiplexer.GetSubscriber();
            string message = "TextCreated=>" + id;
            pub.Publish(EVENTS, message);

            return id;
        }

        private string GetStringifyUserData(UserDataRegion userDataRegion, string id) {
            UserData userData = new UserData
            {
                id = id, region = userDataRegion.region, message = userDataRegion.message
            };

            return JsonConvert.SerializeObject(userData);
        }
    }
}
