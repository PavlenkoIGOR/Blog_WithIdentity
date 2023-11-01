using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class User : IdentityUser
    {
        public int Age { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
