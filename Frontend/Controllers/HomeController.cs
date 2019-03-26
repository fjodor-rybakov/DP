using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace Frontend.Controllers
{
    public struct UserData {
        public string message;
        public string region;
    }

    public class HomeController : Controller
    {
        private const string serverAddress = "http://localhost:5000";
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TextDetails(string id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{serverAddress}/api/values/{id}");
            string relation =  await response.Content.ReadAsStringAsync();
            ViewData["relation"] = relation;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string message, string region)
        {
            Console.WriteLine(message);
            Console.WriteLine(region);
            string id = await SendData(message, region);

            return Redirect("TextDetails/" + id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<string> SendData(string message, string region)
        {
            HttpClient client = new HttpClient();
            UserData userData = new UserData();
            userData.message = message;
            userData.region = region;
            string output = JsonConvert.SerializeObject(userData);

            HttpResponseMessage response = await client.PostAsJsonAsync($"{serverAddress}/api/values", output);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
