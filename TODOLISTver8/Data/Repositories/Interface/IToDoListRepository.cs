using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interface
{
    public interface IToDoListRepository
    {
        Task<IEnumerable<ToDoList>> Get();
        Task<IEnumerable<ToDoListVM>> GetTodoLists(string userId, int status);
        Task<IEnumerable<ToDoList>> Get(int Id);

        IEnumerable<ToDoListVM> Search(string userId, string keyword, int param1);
        //IEnumerable<ToDoListVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize);
        Task<ToDoListVM> Paging(string userId, int param1, string keyword, int pageNumber, int pageSize);

        int Create(ToDoListVM toDoListVM);
        int Update(int Id, ToDoListVM toDoListVM);
        int Delete(int Id);
        int UpdateCheckedTodoList(int Id);
        int updateUncheckedTodolist(int Id);
    }
}
