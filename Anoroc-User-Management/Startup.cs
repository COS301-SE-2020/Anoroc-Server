using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Services;

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
            // Set database engine with connection string
            services.AddSingleton<IDatabaseEngine, SQL_DatabaseService>(sp =>
            {
                return new SQL_DatabaseService(Configuration["SQL_Connection_String"]);
            });


            // Choose cluster service
            if (Configuration["ClusterEngine"] == "MOCK")
            {
                services.AddScoped<IClusterService, Mock_ClusterService>();
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