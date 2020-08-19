using System;
using System.Linq;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using Anoroc_User_Management.Testing.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Anoroc_User_Management.Testing
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<AnorocDbContext>));
                services.Remove(descriptor);

                descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == 
                    typeof(IDatabaseEngine));
                services.Remove(descriptor);

                services.AddDbContext<AnorocDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopeServices = scope.ServiceProvider;
                    var db = scopeServices.GetRequiredService<AnorocDbContext>();

                    db.Database.EnsureCreated();

                    try
                    {
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                services.AddScoped<IDatabaseEngine, SQL_DatabaseService>(sp =>
                {
                    var context = sp.GetService<AnorocDbContext>();
                    try
                    {
                        int maxdate = 8;
                        return new SQL_DatabaseService(context, maxdate);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to get max expiration date, defualting to 8: " + e.Message);
                        return new SQL_DatabaseService(context, 8);
                    }
                });
            });
        }
    }
}