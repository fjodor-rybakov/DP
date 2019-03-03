using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using StackExchange.Redis;
using System.Threading;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        static readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string value = null;
            var db = RedisStore.RedisCache;

            for (int i = 0; i < 3; i++)
            {
                if (db.KeyExists($"RANK_{id}"))
                {
                    value = db.StringGet($"RANK_{id}");
                    break;
                }
                else
                    Thread.Sleep(500);
            }

            return value;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]string value)
        {
            var id = Guid.NewGuid().ToString();
            _data[id] = value;
            var db = RedisStore.RedisCache;
            db.StringSet(id, value);
            var pub = db.Multiplexer.GetSubscriber();
            pub.Publish("events", id);

            return id;
        }
    }
}
