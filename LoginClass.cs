using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyShopping
{
    class LoginClass
    {
        public LoginClass(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
         
        public string UserName { get; set; }
            public string Password { get; set; }
    }
}