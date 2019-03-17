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
    public struct UserData {
        public string data;
        public string region;
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        static readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string value = null;
            var db = RedisStore.RedisCacheRU;
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
            UserData userData = JsonConvert.DeserializeObject<UserData>(json);
            _data[id] = userData.data;
            var db = RedisStore.RedisCacheRU;
            db.StringSet(id, userData.data);
            var pub = db.Multiplexer.GetSubscriber();
            pub.Publish("events", id);

            return id;
        }
    }
}
