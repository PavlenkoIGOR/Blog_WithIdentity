using MainBlog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MainBlog.Data
{
    public class MainBlogDBContext : IdentityDbContext<User>
    {
        public MainBlogDBContext(DbContextOptions<MainBlogDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //    //builder.ApplyConfiguration(new FriendConfiguration());
            //    //builder.ApplyConfiguration(new MessageConfuiguration());
        }
    }
}
