using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Model
{
    public class User : BaseClass
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] Password { get; set; }

        public byte[] PasswordKey { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string gender { get; set; }

        public string role { get; set; }
    

    }

}