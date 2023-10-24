using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }

        [ForeignKey("Id")]
        public string UserId { get; set; }
        public User User { get; set; }


        [ForeignKey("Id")]
        public string CommentId { get; set; }
        public ICollection<Comment> Comment { get; set; }

        public List<Teg> Tegs { get; set; } //EF - сама создаст пром.таблицу TegsPosts
        public Post()
        {
            Tegs = new List<Teg>();
            Comment = new List<Comment>();
            User = new User();
        }
    }
}

