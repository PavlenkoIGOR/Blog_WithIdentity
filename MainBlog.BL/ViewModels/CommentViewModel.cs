using MainBlog.Data.Models;
using MainBlog.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.ViewModels
{
    public class CommentViewModel
    {
        public int CommentVMId { get; set; }
        public string CommentText { get; set; }
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }        
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}

