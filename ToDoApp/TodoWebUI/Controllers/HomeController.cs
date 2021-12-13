using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TodoWebUI.Models;

namespace TodoWebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri baseapiUrl = new Uri("http://localhost:5000");
        HttpClient client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = baseapiUrl;
        }

        public IActionResult Index()
        {
            List<TodoItemView> todos = new List<TodoItemView>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                todos = JsonConvert.DeserializeObject<List<TodoItemView>>(data);
            }

            return View(todos);
        }

        [HttpPost]
        public IActionResult Add(string title)

        {

            TodoItemView item = new TodoItemView();
            item.Title = title;
            string data= JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress , content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index",data);
            }

            return View("Index",item);
        }


        public TodoItemView GetById(string id)

        {
            TodoItemView todoitem = new TodoItemView();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress +id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                todoitem = JsonConvert.DeserializeObject<TodoItemView>(data);
            }

            return todoitem;
        }
        [HttpPatch]
        public IActionResult Edit(string id, string title)

        {
            TodoItemView item = new TodoItemView();
            item= GetById(id);
            if(!string.IsNullOrEmpty(title))
            item.Title = title;
            string data = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PatchAsync(client.BaseAddress +item.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index",true);
            }

            return View("Index", true);

        }
        [HttpPatch]
        public IActionResult EdtComplated(string id, bool isComplated)

        {
            TodoItemView item = new TodoItemView();
            item = GetById(id);
                item.Completed = isComplated;
            string data = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PatchAsync(client.BaseAddress + item.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", true);
            }

            return View("Index", true);

        }
        [HttpDelete]
        public IActionResult Delete(string id)

        {
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress +id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Index");

        }
        public IActionResult Privacy()

        {
          
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
