using MainBlog.Models;

namespace MainBlog.ViewModels
{
    public class DiscussionPostViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<Comment> UsersComments = new();
        public List<Teg> PostsTegs = new();
    }
}
