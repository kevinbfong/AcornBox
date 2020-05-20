using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AcornBox.Application;
using AcornBox.Persistence;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.SqlServer;

namespace AcornBox.WebUI
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
            services.AddPersistence(Configuration);
            services.AddApplication();

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AcornBoxDbContext dbContext) //todo
        {
            // todo: Hack logic...
            System.Threading.Thread.Sleep(30 * 1000); // Magic number of seconds to wait for sql server to start.
            dbContext.Database.Migrate(); // Create/Update database.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthorization();

            DashboardOptions dashboardOptions = new DashboardOptions
            {
                Authorization = new List<IDashboardAuthorizationFilter>() { new DashboardAnonymousAuthorization() }
            };

            app.UseHangfireDashboard(options: dashboardOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    new { controller = "FileEntries", action = "Index" });
            });      
        }
    }

    // todo: Move.
    /// <summary>
    /// Filter that allows anonymous access to the Hangfire Dashboard.
    /// </summary>
    public class DashboardAnonymousAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context) => true;
    }
}
