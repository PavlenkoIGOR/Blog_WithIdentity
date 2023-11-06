using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class Teg
    {
        [Key]
        public int Id { get; set; }
        public string? TegTitle { get; set; }

        public List<Post> Posts { get; set; } = new();

    }
}


