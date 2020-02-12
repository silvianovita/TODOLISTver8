using Dapper;
using Data.Model;
using Data.Repositories.Interface;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        DynamicParameters parameters = new DynamicParameters();
        public readonly ConnectionStrings _connectionString; 
        public UserRepository(ConnectionStrings connectionString, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _connectionString = connectionString;
        }
        public int Create(UserVM userVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramUserName", userVM.UserName);
                parameters.Add("@paramPassword", userVM.Password);
                var result = conn.Execute("SP_InsertDataUsers", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public int Delete(int Id)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                var result = conn.Execute("SP_DeleteDataUsers", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<User>> Get()
        {
            const string query = "SP_GetIsDelUserIdToDoList";
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.QueryAsync<User>(query);
                return result;
            }
        }

        public async Task<IEnumerable<User>> Get(int id)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", id);
                var result = await conn.QueryAsync<User>("SP_GetbyIdUsers", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<User>> Login(UserVM userVM)
        {
            var data = (IEnumerable<User>)null;

            try
            {
                //var result = await _signInManager.PasswordSignInAsync(userVM.Username, userVM.Password, false, false);
                //if (result.Succeeded)
                //{
                parameters.Add("@paramUserName", userVM.UserName);
                parameters.Add("@paramPassword", userVM.Password);
                //parameters.Add("@Token", userVM.Token);
                using (var con = new SqlConnection(_connectionString.Value))
                {
                    data = await con.QueryAsync<User>("SP_Login",
                        parameters, commandType: CommandType.StoredProcedure);
                    await _signInManager.PasswordSignInAsync(userVM.UserName, userVM.Password, false, false);
                    return data;
                }
                //}
            }
            catch (Exception m) { }
            return data;
        }

        public async Task<IdentityResult> Register(UserVM userVM)
        {
            var user = new User(userVM);
            var result = await _userManager.CreateAsync(user, userVM.Password);
            return result;
        }
        //public User Login(UserVM userVM)
        //{
        //    using (var conn = new SqlConnection(_connectionString.Value))
        //    {
        //        DynamicParameters parameters = new DynamicParameters();
        //        parameters.Add("@paramUserName", userVM.UserName);
        //        parameters.Add("@paramPassword", userVM.Password);
        //        var result = conn.Query<User>("SP_Login", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //        return result;
        //    }
        //}

        public int Update(int Id, UserVM userVM)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@paramId", Id);
                parameters.Add("@paramUserName", userVM.UserName);
                parameters.Add("@paramPassword", userVM.Password);
                var result = conn.Execute("SP_UpdateDataUsers", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
