using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace Frontend.Controllers
{
    public class TextStatistic
    {
        public int textNum;
        public int highRankPart;
        public double avgRank;
        public int countRejectRequests;
    }
    
    public class StatisticsController : Controller
    {
        private const string serverAddress = "http://localhost:5000";
        
        [HttpGet]
        public async Task<IActionResult> TextStatistic()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{serverAddress}/api/values");
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            TextStatistic textStatistic = JsonConvert.DeserializeObject<TextStatistic>(json);
            
            ViewData["textNum"] = textStatistic.textNum;
            ViewData["highRankPart"] = textStatistic.highRankPart;
            ViewData["avgRank"] = textStatistic.avgRank;
            ViewData["countRejectRequests"] = textStatistic.countRejectRequests;

            Console.WriteLine("textNum: " + ViewData["textNum"] +
                              ", highRankPart: " + ViewData["highRankPart"] +
                              ", avgRank: " + ViewData["avgRank"] + 
                              ", countRejectRequests: " + ViewData["countRejectRequests"]);

            
            return View();
        }

    }
}