using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebAPICore.Model
{
    public class RefreshtokenTable : BaseClass
    {
        [Key]
        public int RefreshTokenId { get; set; } 
        public string UserId { get; set; }
        public string TokenId { get; set; }
        public string RefreshToken { get; set; }

      
    }
}