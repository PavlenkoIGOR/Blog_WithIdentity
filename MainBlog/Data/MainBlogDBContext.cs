using MainBlog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;


namespace MainBlog.Data
{
    public class MainBlogDBContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Teg> Tegs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public MainBlogDBContext(DbContextOptions<MainBlogDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ////так можно создавать свои таблицы, но что-то не нравится строка где .Map
            //builder.Entity<Post>()
            //    .HasMany(c => c.Tegs)
            //    .WithMany(p => p.Posts)
            //    .Map(m =>
            //    {
            //        // Ссылка на промежуточную таблицу, т.е. создание промежуточной таблицы с нужным именем
            //        m.ToTable("PostsAndTegs");
                    
            //        // Настройка внешних ключей промежуточной таблицы
            //        m.MapLeftKey("PostId");
            //        m.MapRightKey("TegId");
            //    });
            //    //builder.ApplyConfiguration(new FriendConfiguration());
            //    //builder.ApplyConfiguration(new MessageConfuiguration());
        }
    }
}
