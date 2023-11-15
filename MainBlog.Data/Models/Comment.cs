using MainBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? CommentText { get; set; }
        public DateTime CommentPublicationTime { get; set; }

        [ForeignKey("Id")]
        public int PostId { get; set; }
        public Post Post { get; } = null!;

        public string UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
