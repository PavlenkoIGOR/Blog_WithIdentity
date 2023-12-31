﻿using MainBlog.Data.Models;
using MainBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MainBlog.ViewModels
{
    public class UserBlogViewModel
    {
        public string? Id { get; set; }
        public int PostId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле для названия статьи является обязательным для заполнения!")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле для статьи является обязательным для заполнения!")]
        public string Text { get; set; }

        public DateTime PublicationDate { get; set; }

        //[RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Required(ErrorMessage = "Поле tegs является обязательным для заполнения!")]
        [DataType(DataType.Text)]
        public string tegs { get; set; }

        public List<Teg>? tegsList { get; set; }
        public List<Post>? UserPosts { get; set; }
        public UserBlogViewModel() 
        {
        }
        public List<Teg> HasWritingTags()
        {
            List<Teg> Tegs = new List<Teg>();
            if (tegs == null)
            { return null; }
            else
            {
                char[] delimiters = new char[] { '\r', '\n', ',', ' '};

                var words = tegs.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string teg in words)
                {
                    Teg teg1 = new Teg();
                    teg1.TegTitle = teg;
                    Tegs.Add(teg1);
                }
                return Tegs;
            }
        }
    }
}
