using MainBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MainBlog.ViewModels
{
    public class UserPostsViewModel
    {
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public string tegs { get; set; }
        public UserPostsViewModel() 
        {
        }
        public List<Teg> HasWritingTags()
        {
            List<Teg> Tegs = new List<Teg>();
            char[] delimiters = new char[] { ' ', '\r', '\n', ',' };
            var words = tegs.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            foreach (string teg in words)
            {
                Teg teg1 = new Teg();
                teg1.Name = teg;
                Tegs.Add(teg1);
            }
            return Tegs;
        }
    }
}
