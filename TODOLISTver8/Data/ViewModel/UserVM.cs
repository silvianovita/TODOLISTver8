using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Data.ViewModel
{
    public class UserVM
    { 
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
