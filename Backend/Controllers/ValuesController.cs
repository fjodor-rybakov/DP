using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // private ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        static readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string value = null;
            _data.TryGetValue(id, out value);
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
            // string res = db.StringGet(id);
            // Console.WriteLine(res);

            return id;
        }
    }
}
