using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using StackExchange.Redis;
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
        private static string _region = "";

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string value = null;
            var db = RedisStore.getInstance().RedisCache(ValuesController._region);
            bool isError = true;

            for (int i = 0; i < 3; i++)
            {
                if (db.KeyExists($"RANK_{id}"))
                {
                    value = db.StringGet($"RANK_{id}");
                    isError = false;
                    break;
                }
                else {
                    Thread.Sleep(500);
                }  
            }

            if (isError) {
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
            string contextId = userDataRegion.region;
            Console.WriteLine("Region: " + contextId);

            var db = RedisStore.getInstance().RedisCacheTable;
            db.StringSet(contextId, getStrigifyUserData(userDataRegion, id));
            ValuesController._region = contextId;

            var pub = db.Multiplexer.GetSubscriber();
            pub.Publish("events", contextId);

            return id;
        }

        private string getStrigifyUserData(UserDataRegion userDataRegion, string id) {
            UserData userData = new UserData();
            userData.id = id;
            userData.region = userDataRegion.region;
            userData.message = userDataRegion.message;

            return JsonConvert.SerializeObject(userData);
        }
    }
}
