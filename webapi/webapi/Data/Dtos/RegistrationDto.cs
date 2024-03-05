using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Dtos
{
    public class RegistrationDto
    {
         public string Username { get; set; }      
        public string Password { get; set; }
        public string email { get; set; }          
        public string gender { get; set; }           
        public string role { get; set; }        
         public bool isactive { get; set; } 
    }
}