using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MainBlog.Models
{
    public class User : IdentityUser
    {
        public int Age { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
