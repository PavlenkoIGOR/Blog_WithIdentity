using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        [ForeignKey("Id")]
        public int TegId { get; set; }
        public ICollection<Teg> Tegs { get; set; }
        public Post()
        {
            Tegs = new List<Teg>();
        }
    }
}

