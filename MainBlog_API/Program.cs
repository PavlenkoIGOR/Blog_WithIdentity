using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace MainBlog_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            
            // ��� �� ����� �������������, �� � MVC �� ����� ������ AddControllersWithViews()
            builder.Services.AddControllers();

            #region ���������� �������� API
            // ������������ �������������� ��������� ������������ WebApi � �������������� Swagger
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "MainBlog_Api", Version = "v1" }); });
            #endregion

            var app = builder.Build();

           
            // ����������� ����������� ��� ������� ��� ���������� ��������
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MainBlog_Api v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            #region ���������
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            #endregion

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }

    }
}