using API.Services.Interface;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ToDoListServices : IToDoListServices
    {
        private readonly IToDoListRepository _toDoListRepository;
        public ToDoListServices(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }
        public int Create(ToDoListVM toDoListVM)
        {
            return _toDoListRepository.Create(toDoListVM);
        }

        public int Delete(int id)
        {
            return _toDoListRepository.Delete(id);
        }

        public Task<IEnumerable<ToDoList>> Get()
        {
            return _toDoListRepository.Get();
        }

        public Task<IEnumerable<ToDoList>> Get(int Id)
        {
            return _toDoListRepository.Get(Id);
        }

        public Task<IEnumerable<ToDoListVM>> GetTodoLists(string userId, int status)
        {
            return _toDoListRepository.GetTodoLists(userId,status);
        }

       // public IEnumerable<ToDoListVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize)
        public async Task<ToDoListVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize)
        {
            return await _toDoListRepository.Paging(userId, param1, keyword, pageNumber, pageSize);
        }

        public IEnumerable<ToDoListVM> Search(string userId, string keyword, int param1)
        {
            return _toDoListRepository.Search(userId, keyword, param1);
        }

        public int Update(int id, ToDoListVM toDoListVM)
        {
            return _toDoListRepository.Update(id, toDoListVM);
        }

        public int UpdateCheckedTodoList(int Id)
        {
            return _toDoListRepository.UpdateCheckedTodoList(Id);
        }

        public int updateUncheckedTodolist(int Id)
        {
            return _toDoListRepository.updateUncheckedTodolist(Id);
        }
    }
}
