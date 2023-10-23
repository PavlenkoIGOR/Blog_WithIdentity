using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class Teg
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Id")]
        public int PostId { get; set; }
        public ICollection<Post> Posts { get; set; }

        public Teg() 
        {
            Posts = new List<Post>();
        }
    }
}
