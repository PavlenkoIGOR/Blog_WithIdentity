using MainBlog.Models;

namespace MainBlog.ViewModels
{
    public class DiscussionPostViewModel
    {
        //оригинал
        //public int Id { get; set; }
        //public int PostId { get; set; }
        //public int CommentId { get; set; }
        //public string AuthorOfPost { get; set; }
        //public string Title { get; set; }
        //public string Text { get; set; }
        //public DateTime PublicationTime { get; set; }
        //public string AuthrorOfComment { get; set; }
        //public string CommentText { get; set; }
        //public List<Comment> UsersComments = new();
        //public List<Teg> PostsTegs = new();
        public PostViewModel PostVM { get; set; } = new();
        public CommentViewModel CommentVM { get; set; } = new();
    }
}

/*
namespace MainBlog.ViewModels
{
    public class DiscussionPostViewModel
    {
        public PostViewModel PostVM { get; set; } = new();
        public CommentViewModel CommentVM { get; set; } = new();
    }
}
 */