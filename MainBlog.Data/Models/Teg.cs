using MainBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Data.Models
{
    public class Teg
    {
        public int Id { get; set; }
        public string TegTitle { get; set; }
        public List<Post> Posts { get; set; } = new();
    }
}


