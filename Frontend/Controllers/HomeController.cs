using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net.Http;
using APIError;
using Newtonsoft.Json;

namespace Frontend.Controllers
{
    public class UserData : UserDataRegion {
        public string id;
    }

    public class UserDataRater : UserData {
        public double relation;
    }
    public class UserDataRegion {
        public string message;
        public string region;
    }
    
    public class HomeController : Controller
    {
        private const string serverAddress = "http://localhost:5000";
        private readonly ApiError _errors = new ApiError();
        
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
            string json = await response.Content.ReadAsStringAsync();
            if (response.StatusCode >= HttpStatusCode.BadRequest)
            {
                Error error = JsonConvert.DeserializeObject<Error>(json);
                
                ViewData["error_message"] = error.Message;
            }
            else
            {
                UserDataRater userDataRater = JsonConvert.DeserializeObject<UserDataRater>(json);
            
                ViewData["relation"] = userDataRater.relation;
                ViewData["region"] = userDataRater.region;
            }
            Console.WriteLine(json);

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
            UserDataRegion userData = new UserDataRegion {message = message, region = region};
            string output = JsonConvert.SerializeObject(userData);

            HttpResponseMessage response = await client.PostAsJsonAsync($"{serverAddress}/api/values", output);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
