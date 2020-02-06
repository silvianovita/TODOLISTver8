using Data.Base;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Model
{
    public class User : IdentityUser
    {
        //[Key]
        //public override string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset DeleteDate { get; set; }
        public User() { }
        public User(UserVM userVM)
        {
            this.UserName = userVM.UserName;
            this.Password = userVM.Password;
            this.IsDelete = false;
            this.CreateDate = DateTimeOffset.Now;
        }
        public void Update(UserVM userVM)
        {
            this.UserName = userVM.UserName;
            this.Password = userVM.Password;
            this.UpdateDate = DateTimeOffset.Now;
        }
        public void Delete(UserVM userVM)
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}
