using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitmqApp.MVC.BackgroundServices;
using RabbitmqApp.MVC.Models;
using RabbitmqApp.MVC.Service;
using System.Configuration;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace RabbitmqApp.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            ConfigurationManager _configuration = builder.Configuration;
            IWebHostEnvironment _environment = builder.Environment;
             
            builder.Services.AddHostedService<ImageWatermarkProcessBackgroundService>();
            builder.Services.AddSingleton<RabbitMQClientService>();
            builder.Services.AddSingleton<RabbitMQPublisher>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName:"productDb");
            });
            builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(_configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync=true}); //Ayaða kalktýðýnda bir kere ayaða kalksýn yalnýzca bir nesne örneði gelecek.



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}