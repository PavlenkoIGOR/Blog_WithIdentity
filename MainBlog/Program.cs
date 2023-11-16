using MainBlog.Data;
using MainBlog.Data.Data;
using MainBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MainBlog;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        var connectionString = builder.Configuration.GetConnectionString("BlogContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddTransient<IServiceCollection, ServiceCollection>();
        //builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<ILogger, Logger>();

        builder.Services.AddDbContext<MainBlogDBContext>(options => options.UseSqlite(connectionString)).AddDatabaseDeveloperPageExceptionFilter()
            //.AddUnitOfWork()
            .AddIdentity<User, IdentityRole>(opts =>
            {                   
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<MainBlogDBContext>();

        // Connect logger -- ���� ��� ��������������� ������� ������������ ���� � "Appsettings.json"
        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole();



        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddRazorPages();
       

        var app = builder.Build();


        app.Environment.EnvironmentName = "Production"; // ������ ��� ���������
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseMiddleware<MyExceptionMiddleware>();


        app.UseStatusCodePagesWithReExecute("/ErrorPages/MyErrorsAction", "?statusCode={0}");

        app.UseStaticFiles();
        
        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        //app.UseMiddleware<Authensdfgdsfg>();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        using (var scope = app.Services.CreateScope())
        {
            var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //1. scope.ServiceProvider` -��� ������ - ��������� �����, ��������� � ������� ������(`scope`).
            //2. `GetRequiredService()` -��� ����� - ����������, ������� ��������� ������ ���������� ���� `T` �� ���������� �����.
            //3. `RoleManager` -��� �����, ��������������� ASP.NET Core Identity ��� ���������� ������ ������������� � ����������.

            string [] roles = new[] { "Administrator","Moderator", "User"};
            foreach (var role in roles)
            {
                if (!await roleManger.RoleExistsAsync(role))
                {
                    await roleManger.CreateAsync(new IdentityRole(role));
                }
            }
        }

        app.Run(); 
    }
}