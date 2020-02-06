using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class UserController : Controller
    {
        readonly HttpClient Client = new HttpClient();
        public UserController()
        {
            Client.BaseAddress = new Uri("https://localhost:44319/api/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetString("Id");
            if (id != null)
            {
                return View();
            }
            return RedirectToAction(nameof(Login));

        }
        public ActionResult Login()
        {
            var id = HttpContext.Session.GetString("Id");
            if (id == null)
            {
                return View();
            }
            //return RedirectToAction("Index", "ToDoLists", new { area = "ToDoLists" });
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserVM userVM)
        {
            try
            {
                // TODO: Add insert logic here
                var myContent = JsonConvert.SerializeObject(userVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = Client.PostAsync("Users/Login/", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    var user = result.Content.ReadAsStringAsync().Result.Replace("\"", "").Split("...");
                    HttpContext.Session.SetString("Token", "Bearer " + user[0]);
                    HttpContext.Session.SetString("Id", user[1]);
                    Client.DefaultRequestHeaders.Add("Authorization", user[0]);
                    //var data = result.Content.ReadAsAsync<User>();
                    //data.Wait();
                    //var user = data.Result;
                    //HttpContext.Session.SetString("Id", user.Id.ToString());
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public async Task<IEnumerable<ToDoListVM>> Search(string keyword, int status)
        {
            try
            {
                Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                var response = await Client.GetAsync("ToDoLists/Search/" + HttpContext.Session.GetString("Id") + "/" + keyword + "/" + status);
                if (response.IsSuccessStatusCode)
                {
                    var e = await response.Content.ReadAsAsync<List<ToDoListVM>>();
                    return e;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
        public async Task<IEnumerable<ToDoListVM>> Paging(int pageSize, int pageNumber, int status, string keyword)
        {
            try
            {
                //Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
                var response = await Client.GetAsync("ToDoLists/Paging/" + HttpContext.Session.GetString("Id") + "/" +pageSize+"/"+ pageNumber+"/" + status + "/" + keyword);
                if (response.IsSuccessStatusCode)
                {
                    var e = await response.Content.ReadAsAsync<List<ToDoListVM>>();
                    return e;
                }
            }
            catch (Exception m)
            {

            }
            return null;
        }

        public async Task<IList<ToDoListVM>> List(int status)
        {
            string Id = HttpContext.Session.GetString("Id");
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            IEnumerable<ToDoListVM> users = null;
            var responseTask = await Client.GetAsync("ToDoLists/GetTodoLists/" + Id + '/' + status);
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsAsync<IList<ToDoListVM>>();
                return readTask;      
        //return Ok(new { data = readTask });
            }
            return null;
        }

        [HttpGet("User/PageData/{status}")]
        public IActionResult PageData(int status, IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            var keyword = "val" + request.Search.Value;
            var data = Search(keyword, status).Result;
            var filteredData = data;
            var dataPage = Paging(pageSize, pageNumber, status, keyword).Result;
            var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            return new DataTablesJsonResult(response, true);
            //var data = List(status).Result;

            //var filteredData = String.IsNullOrWhiteSpace(request.Search.Value)
            //    ? data
            //    : data.Where(_item => _item.Name.Contains(request.Search.Value));

            //var dataPage = filteredData.Skip(request.Start).Take(request.Length);

            //var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            //return new DataTablesJsonResult(response, true);
        }

        public JsonResult Insert(ToDoListVM todolistVM)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            todolistVM.userId = HttpContext.Session.GetString("Id");
            var myContent = JsonConvert.SerializeObject(todolistVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PostAsync("todolists", byteContent).Result;
            return Json(result);
        }
        public JsonResult Update(int id, ToDoListVM grade)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var myContent = JsonConvert.SerializeObject(grade);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PutAsync("todolists/" + id, byteContent).Result;
            return Json(result);

        }
        //[HttpGet("{Id}")]
        public async Task<JsonResult> GetbyIDAsync(int id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            HttpResponseMessage response = await Client.GetAsync("todolists");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<ToDoList>>();
                var attitude = data.FirstOrDefault(s => s.Id == id);
                var json = JsonConvert.SerializeObject(attitude, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return Json(json);
            }
            return Json("Internal Server Error");
        }
        public JsonResult UpdateCheckedTodoList(int Id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = Client.DeleteAsync("todolists/updateCheckedTodolist/" + Id).Result;
            return Json(result);
        }
        public JsonResult UpdateUnCheckedTodoList(int Id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var myContent = JsonConvert.SerializeObject(Id);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = Client.PutAsync("toDoLists/updateUncheckedTodolist/" + Id, byteContent).Result;
            return Json(result);
        }
        public JsonResult Delete(int id)
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = Client.DeleteAsync("ToDoLists/" + id).Result;
            return Json(result);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            HttpContext.Session.Remove("Id");
            return RedirectToAction(nameof(Index));
        }
    }
}