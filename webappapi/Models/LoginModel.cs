// webappapi.Models.LoginModel.cs
using System.ComponentModel.DataAnnotations;

namespace webappapi.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}