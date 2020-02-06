using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interface
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> Get();
        Task<IEnumerable<User>> Get(int Id);
        Task<IEnumerable<User>> Login(UserVM userVM);
        Task<IdentityResult> Register(UserVM userVM);

        //User Login(UserVM userVM);
        int Create(UserVM userVM);
        int Update(int Id, UserVM userVM);
        int Delete(int Id);
    }
}
