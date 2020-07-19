using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));
            services.AddControllers();

            //-----------------------------------------------------------------------------------
            // Longest -> shortes life term
            // Singleton - Transient - Scoped

            // Set database engine with connection string
            /*services.AddScoped<IDatabaseEngine, SQL_DatabaseService>(sp =>
            {
                return new SQL_DatabaseService(Configuration["SQL_Connection_String"]);
            });*/



            //-----------------------------------------------------------------------------------
            // Set the database Context with regards to Entity Framework SQL Server with connection string
            services.AddDbContext<dbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SQL_Connection_String")));


            // Choose cluster service
            if (Configuration["ClusterEngine"] == "MOCK")
            {
                services.AddScoped<IClusterService, Mock_ClusterService>(sp => {

                    var database = sp.GetService<IDatabaseEngine>();
                    return new Mock_ClusterService(database);
                });
            }
            else if (Configuration["ClusterEngine"] == "MLNet")
            {
                services.AddScoped<IClusterService, MLNetClustering>();
            }
            else if (Configuration["ClusterEngine"] == "CSM")
            {
                services.AddScoped<IClusterService, CSM_ClusterService>(sp =>
                {
                    var database = sp.GetService<IDatabaseEngine>();
                    return new CSM_ClusterService(database);
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}