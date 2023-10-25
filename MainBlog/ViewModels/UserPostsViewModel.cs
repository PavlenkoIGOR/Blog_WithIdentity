using MainBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace MainBlog.ViewModels
{
    public class UserPostsViewModel
    {
        public string Name { get; set; }
        //public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime PublicationDate { get; set; }

        public List<Teg> Tegs { get; set; }

        //public string RoleType { get; set; }
        public UserPostsViewModel() 
        {
            Tegs = new List<Teg>();
        }
    }
}
