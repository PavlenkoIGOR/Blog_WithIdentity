using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentPublicationTime { get; set; }

        [ForeignKey("Id")]
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
/* 
     public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string? CommentText { get; set; }
        public DateTime CommentPublicationTime { get; set; }
        [ForeignKey("Id")]
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string UserId { get; set; }
        public User User { get; set; } = null!;
    }
*/