﻿using MainBlog.Data.Models;
using MainBlog.Models;

namespace MainBlog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime PublicationDateOfPost { get; set; }
        public string AuthorOfPost { get; set; } = null!;
        public string UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Comment>? CommentsOfPost { get; set; }

        public List<Teg>? Tegs { get; set; }
    }
}
