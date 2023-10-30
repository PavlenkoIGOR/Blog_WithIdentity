using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string? CommentText { get; set; }

        [ForeignKey("Id")]
        public int PostId { get; set; }
        public Post Post { get; } = new();
       
    }
}
