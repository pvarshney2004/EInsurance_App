using EInsurance_App.Data;
using EInsurance_App.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

namespace EInsurance_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSession();

            QuestPDF.Settings.License = LicenseType.Community;

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            // admin auto seeding 
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    var adminEmail = "admin@gmail.com";

                    if (!context.Admins.Any(a => a.Email == adminEmail))
                    {
                        var admin = new Admin
                        {
                            Username = "admin",
                            Email = adminEmail,
                            Password = "Admin@123",
                            FullName = "System Administrator"
                        };

                        context.Admins.Add(admin);
                        context.SaveChanges();

                        Console.WriteLine("✅ Default Admin Created");
                    }
                    else
                    {
                        Console.WriteLine("ℹ️ Admin already exists");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error while seeding admin: " + ex.Message);
                }
            }

            app.Run();
        }
    }
}
