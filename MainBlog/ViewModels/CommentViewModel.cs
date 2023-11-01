namespace MainBlog.ViewModels
{
    public class CommentViewModel
    {
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string CommentText { get; set; }
    }
}
