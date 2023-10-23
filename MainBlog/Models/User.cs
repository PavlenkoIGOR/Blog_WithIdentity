using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class User : IdentityUser
    {
        public int Age { get; set; }
        public DateTime RegistrationDate { get; set; }

        [ForeignKey("Id")]
        public int PostId { get; set; }
        public ICollection<Post> Posts { get; set; }
        public User() 
        {
            Posts = new List<Post>();
        }
    }
}
