using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interface
{
    public interface IToDoListServices
    {
        Task<IEnumerable<ToDoList>> Get();
        Task<IEnumerable<ToDoListVM>> GetTodoLists(string userId,int status);
        Task<IEnumerable<ToDoList>> Get(int Id);

        IEnumerable<ToDoListVM> Search(string userId, string keyword, int param1);
        IEnumerable<ToDoListVM> Paging(string userId, int pageSize, int pageNumber, int param1, string keyword);

        int Create(ToDoListVM toDoListVM);
        int Update(int id, ToDoListVM toDoListVM);
        int Delete(int id);
        int UpdateCheckedTodoList(int Id);
        int updateUncheckedTodolist(int Id);
    }
}
