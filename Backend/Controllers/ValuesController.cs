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

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string value = null;
            bool isError = true;
            var instance = RedisStore.getInstance();

            for (int i = 0; i < 3; i++)
            {
                string region = instance.getValue($"RANK_{id}");
                value = RedisStore.SearchValueById($"RANK_{id}", region);
                if (value != null)
                {
                    isError = false;
                    break;
                }
                else {
                    Thread.Sleep(500);
                }  
            }

            Console.WriteLine(value);

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
            Console.WriteLine("Region: " + userDataRegion.region);
            var instance = RedisStore.getInstance();

            var db = instance.RedisCacheTable;
            db.StringSet(contextId, getStrigifyUserData(userDataRegion, id));

            var pub = db.Multiplexer.GetSubscriber();
            pub.Publish("events", contextId);

            instance.addValue($"RANK_{id}", contextId);

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
