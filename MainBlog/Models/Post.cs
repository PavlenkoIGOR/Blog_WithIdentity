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

        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public User User { get; set; } = null!;


        [ForeignKey("Comment")]
        public string CommentId { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public List<Teg> Tegs { get; set; } //EF - сама создаст пром.таблицу TegsPosts
        public Post()
        {
            Tegs = new List<Teg>();
            Comments = new List<Comment>();
            User = new User();
        }
    }
}

