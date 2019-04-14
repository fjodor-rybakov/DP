using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using APIError;
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
        private const string Events = "events";
        private readonly ApiError _errors = new ApiError();
        // GET api/values/<id>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            string value = null;
            bool isError = true;
            var instance = RedisStore.getInstance();
            var tableDb = instance.RedisCacheTable;

            for (int i = 0; i < 3; i++)
            {
                int idDb = (int)tableDb.StringGet($"RANK_{id}");
                value = RedisStore.SearchValueById($"RANK_{id}", idDb);
                if (value != null)
                {
                    isError = false;
                    break;
                }

                Thread.Sleep(500);
            }

            Console.WriteLine(value);

            return isError ? _errors.IncorrectUidOrLimit : Ok(value);
        }

        // GET api/values
        [HttpGet]
        public IActionResult GetTextStatistic()
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
            
            return isError ? _errors.StatisticNotFound : Ok(value);
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]string json)
        {
            var id = Guid.NewGuid().ToString();
            UserDataRegion userDataRegion = JsonConvert.DeserializeObject<UserDataRegion>(json);
            var contextId = $"RANK_{id}";
            var instance = RedisStore.getInstance();
            var db = instance.RedisCacheTable;

            var idDb = instance.GetNumDB(userDataRegion.region);
            var regionDb = instance.RedisCache(idDb);
            db.StringSet(contextId, idDb);
            regionDb.StringSet(id, GetStringifyUserData(userDataRegion, id));
          
            var pub = db.Multiplexer.GetSubscriber();
            string message = "TextCreated=>" + id;
            pub.Publish(Events, message);

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
