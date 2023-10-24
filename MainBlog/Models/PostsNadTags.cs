using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class PostsNadTags
    {
        [ForeignKey("Id")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Id")]
        public int TegId { get; set; }
        public Teg Teg { get; set; }
        public PostsNadTags() 
        {
            Post = new Post();
            Teg = new Teg();
        }
    }
}
