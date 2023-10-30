using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; }        
        public DateTime PublicationDate { get; set; }

        public string UserId { get; set; }
        //[Comment("Vghjdfkjhdfkjhg")] //так можно сделать комментарий для таблицы в БД
        public User User { get; set; } = null!;

        public List<Comment> Comments { get; set; } = new();

        public List<Teg> Tegs { get; set; } = new(); //EF - сама создаст пром.таблицу TegsPosts
    }
}

