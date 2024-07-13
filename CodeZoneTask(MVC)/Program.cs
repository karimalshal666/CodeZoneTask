using CodeZoneTask_MVC_.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeZoneTask_MVC_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register DbContext with configuration
            builder.Services.AddDbContext<CodeZoneEntities>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                  name: "default",
                    pattern: "{controller=Store}/{action=GetAllStores}/{id?}");

            app.Run();
        }
    }
}