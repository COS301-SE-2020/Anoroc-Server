using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Services;
using FirebaseAdmin;
using Microsoft.EntityFrameworkCore;
using Anoroc_User_Management.Models;
using System;
using System.Diagnostics;

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


            // Add IMobileMessaging Client
            services.AddScoped<IMobileMessagingClient, FirebaseService>();

            // Add ICrossedPathsService
            services.AddScoped<ICrossedPathsService, CrossedPathsService>( sp=>
            {
                // public CrossedPathsService(IClusterService clusterService, IMobileMessagingClient mobileMessagingClient, IDatabaseEngine databaseEngine, double proximityToCarrier)
                var cluster = sp.GetService<IClusterService>();
                var messaging = sp.GetService<IMobileMessagingClient>();
                var database = sp.GetService<IDatabaseEngine>();
                try
                {
                    var stringy = Configuration["ProximityToCarrier"];
                    double proximity = Convert.ToDouble(stringy);
                    return new CrossedPathsService(cluster, messaging, database, proximity);
                }
                catch(Exception e)
                {
                    Debug.WriteLine("Failed to get Proximity to carrier meters, defualting to 10: " + e.Message);
                    return new CrossedPathsService(cluster, messaging, database, 10.0);
                }
            });


//----------------------------------------------------------------------------------------------------------------------------------
            // Set the database Context with regards to Entity Framework SQL Server with connection string

            services.AddDbContext<AnorocDbContext>(options =>
                options.UseSqlServer(Configuration["SQL_Connection_String"]));
//----------------------------------------------------------------------------------------------------------------------------------



            services.AddScoped<IDatabaseEngine, SQL_DatabaseService>(sp=>
            {
                var context = sp.GetService<AnorocDbContext>();
                try
                {
                    int maxdate = Convert.ToInt32(Configuration["DaysAllowedToStore"]);
                    int hours = Convert.ToInt32(Configuration["HoursConsideredOld"]);
                    return new SQL_DatabaseService(context, maxdate,hours);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Failed to get max expiration date, defualting to 8: " + e.Message);
                    return new SQL_DatabaseService(context, 8,4);
                }
            });


//----------------------------------------------------------------------------------------------------------------------------------
            // Choose Cluster service

            if (Configuration["ClusterEngine"] == "MOCK")
            {
                services.AddScoped<IClusterService, Mock_ClusterService>();
            }
            else if (Configuration["ClusterEngine"] == "MLNet")
            {
                services.AddScoped<IClusterService, MLNetClustering>();
            }
            else if (Configuration["ClusterEngine"] == "DBSCAN")
            {
                services.AddScoped<IClusterService, DBScanClusteringService>(sp => {

                    var databaseServce = sp.GetService<IDatabaseEngine>();
                    try
                    {
                        int numberofpoints = Convert.ToInt32(Configuration["NumberOfPointsPerCluster"]);
                        return new DBScanClusteringService(databaseServce, numberofpoints);
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        Debug.WriteLine("Using Defualt value...");
                        return new DBScanClusteringService(databaseServce, 2);
                    }
                });
            }
//----------------------------------------------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------------------------------------------
            // Cluster Management Service Injection

            services.AddScoped<IClusterManagementService, ClusterManagementService>(sp =>
            {
                var clusterService = sp.GetService<IClusterService>();
                var database = sp.GetService<IDatabaseEngine>();
                try
                {
                    int hours = Convert.ToInt32(Configuration["HoursConsideredOld"]);
                    int days = Convert.ToInt32(Configuration["DaysAllowedToStore"]);

                    return new ClusterManagementService(clusterService, database, hours, days);
                }
                catch(Exception e)
                {

                    Debug.WriteLine("Failed to get Ages value from config file: " + e.Message);
                    Debug.WriteLine("Using defualt values...");
                    return new ClusterManagementService(clusterService, database, 4, 8);
                }
            });

//----------------------------------------------------------------------------------------------------------------------------------
            // Iteneray Analytics Service Injection

            services.AddScoped<IItineraryService, ItineraryService>(sp =>
            {
                var clusterService = sp.GetService<IClusterService>();
                var database = sp.GetService<IDatabaseEngine>();
                try
                {
                    var highDensity = Convert.ToInt32(Configuration["HighDenistyValue"]);
                    return new ItineraryService(clusterService, database, highDensity);
                }
                catch(Exception e)
                {
                    Debug.WriteLine("Failed to get High Density value from config file: " + e.Message);
                    Debug.WriteLine("Using defualt value...");
                    return new ItineraryService(clusterService, database, 50);
                }
            });

//----------------------------------------------------------------------------------------------------------------------------------
            // User Managmement service

            services.AddScoped<IUserManagementService, UserManagementService>(sp =>
            {
                var database = sp.GetService<IDatabaseEngine>();
                var webapptoken = Configuration["WebAppToken"];
                try
                {
                    var tokenLength = Convert.ToInt32(Configuration["Token_Length"]);
                    return new UserManagementService(database, tokenLength, webapptoken);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Failed to get Custom Token Length value from config file: " + e.Message);
                    Debug.WriteLine("Using defualt value...");
                    return new UserManagementService(database, 128, webapptoken);
                }
            });
            //----------------------------------------------------------------------------------------------------------------------------------
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}