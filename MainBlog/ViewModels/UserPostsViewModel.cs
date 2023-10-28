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
        //public List<Teg> Teg { get; set; }
        public string tegs = string.Empty;
        public UserPostsViewModel() 
        {
            //Tegs = new List<Teg>();
        }
        public List<Teg> HasWritingTags()
        {
            //var noPunctuationText = new string(tegs.Where(c => !char.IsPunctuation(c)).ToArray());
            List<Teg> Tegs = new List<Teg>();
            char[] delimiters = new char[] { ' ', '\r', '\n', ',' };
            var words = tegs.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            //string[] tegsArr = tegs.Split(' ', ',');
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
