using Dapper;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        public readonly ConnectionStrings _connectionString;
        public ToDoListRepository(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }
        public int Create(ToDoListVM toDoListVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramName", toDoListVM.Name);
                parameters.Add("@paramUserId", toDoListVM.userId);
                var result = conn.Execute("SP_InsertDataToDoLists", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public int Delete(int Id)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                var result = conn.Execute("SP_DeleteDataToDoLists", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<ToDoList>> Get()
        {
            const string query = "SP_GetIsDelFalseToDoList";
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.QueryAsync<ToDoList>(query);
                return result;
            }
        }

        public async Task<IEnumerable<ToDoList>> Get(int Id)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                var result = await conn.QueryAsync<ToDoList>("SP_GetByIdToDoLists", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<ToDoListVM>> GetTodoLists(string Id,int status)
        {
            var result = (IEnumerable<ToDoListVM>)null;
            try
            {
                using (var conn = new SqlConnection(_connectionString.Value))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@paramUserId", Id);
                    parameters.Add("@paramStatus", status);
                    result = await conn.QueryAsync<ToDoListVM>("SP_GetIsDelUserIdToDoList", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch(Exception m) { }
            return result;
        }

        public IEnumerable<ToDoListVM> Paging(string userId, int pageSize, int pageNumber, int param1, string keyword)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramUserId", userId);
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@pageNumber", pageNumber);
                parameters.Add("@param1", param1); 
                parameters.Add("@paramKeyword", keyword);
                var result = conn.Query<ToDoListVM>("SP_Paging", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public IEnumerable<ToDoListVM> Search(string userId, string keyword, int param1)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramUserId", userId);
                parameters.Add("@paramKeyword", keyword);
                parameters.Add("@param1", param1);
                var result = conn.Query<ToDoListVM>("SP_Search", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public int Update(int Id, ToDoListVM toDoListVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                parameters.Add("@paramName", toDoListVM.Name);
                var result = conn.Execute("SP_UpdateDataToDoLists", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public int UpdateCheckedTodoList(int Id)
        {
            using (var conn = new SqlConnection(_connectionString.Value)) 
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                var result = conn.Execute("SP_UpdateCheckedTodoList", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public int updateUncheckedTodolist(int Id)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                var result = conn.Execute("SP_UpdateUncheckedTodolist", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
