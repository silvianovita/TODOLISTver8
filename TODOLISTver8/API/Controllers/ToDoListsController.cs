using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes ="Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        private readonly IToDoListServices _toDoListServices;
        
        public ToDoListsController(IToDoListServices toDoListServices)
        {
            _toDoListServices = toDoListServices;
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoList>> Get()
        {
            return await _toDoListServices.Get();
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ToDoList>> Get(int Id)
        {
            return await _toDoListServices.Get(Id);
        }

        [HttpGet]
        [Route("GetTodoLists/{Id}/{status}")]
        public async Task<IEnumerable<ToDoListVM>> GetTodoLists(string Id, int status)
        {
            return await _toDoListServices.GetTodoLists(Id,status);
        }

        [HttpPost]
        public ActionResult Post(ToDoListVM toDoListVM)
        {
            return Ok(_toDoListServices.Create(toDoListVM));
        }

        [HttpPut("{Id}")]
        public ActionResult Put(int Id, ToDoListVM toDoListVM)
        {
            return Ok(_toDoListServices.Update(Id, toDoListVM));
        }


        [HttpDelete("{Id}")]
        public ActionResult Delete(int Id)
        {
            return Ok(_toDoListServices.Delete(Id));
        }

        [HttpDelete]
        [Route("UpdateCheckedTodoList/{Id}")]
        public ActionResult UpdateCheckedTodoList(int Id)
        {
            return Ok(_toDoListServices.UpdateCheckedTodoList(Id));
        }
        [HttpPut]
        [Route("updateUncheckedTodolist/{Id}")]
        public ActionResult updateUncheckedTodolist(int Id)
        {
            return Ok(_toDoListServices.updateUncheckedTodolist(Id));
        }

        [HttpGet]
        [Route("Search/{userId}/{keyword}/{param1}")]
        public IEnumerable<ToDoListVM> Search(string userId, string keyword, int param1)
        {
            keyword = keyword.Substring(3);
            return _toDoListServices.Search(userId, keyword, param1);
        }
        [HttpGet]
        [Route("Paging/{userId}/{pageSize}/{pageNumber}/{param1}/{keyword}")]
        public IEnumerable<ToDoListVM> Paging(string userId, int pageSize, int pageNumber, int param1, string keyword)
        {
            keyword = keyword.Substring(3);
            return _toDoListServices.Paging(userId, pageSize, pageNumber, param1, keyword);
        }
    }
}