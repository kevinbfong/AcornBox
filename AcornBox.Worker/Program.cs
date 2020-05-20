
using AcornBox.Common;
using AcornBox.Worker;
using Hangfire;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Worker
{
    public class MyJobActivator : JobActivator
    {
        private readonly IConfiguration _config;

        public MyJobActivator(IConfiguration config)
        {
            _config = config;
        }

        public override object ActivateJob(Type jobType)
        {
            Console.WriteLine($"{DateTime.Now.TimeOfDay} - ActivateJob called, jobType is '{jobType.Name}'.");

            if (jobType == typeof(IGenerateSchema))
            {
                Console.WriteLine($"{DateTime.Now.TimeOfDay} - ActivateJob called, IS handling job.");
                return new GenerateSchema(_config);
            }

            Console.WriteLine($"{DateTime.Now.TimeOfDay} - ActivateJob called, IS NOT handling job!");
            return base.ActivateJob(jobType);
        }
    }


    class Program
    {
        //static void Main(string[] args)
        //{
        //    //GlobalConfiguration.Configuration.UseSqlServerStorage("Server=(localdb)\\mssqllocaldb; Database=AcornBox; Trusted_Connection = True; MultipleActiveResultSets = true");
        //    GlobalConfiguration.Configuration.UseSqlServerStorage("Server=acornbox.db; Database=AcornBox; User=sa; Password=Your_password123");

        //    GlobalConfiguration.Configuration.UseActivator(new MyJobActivator());

        //    using (var server = new BackgroundJobServer())
        //    {
        //        Console.WriteLine("Hangfire Server started. Press any key to exit...");
        //        Console.ReadKey();
        //    }
        //}

        static async Task Main(string[] args)
        {
            Console.WriteLine("AcornBox.Worker - Main - Entered");
            var hostBuilder = new Microsoft.Extensions.Hosting.HostBuilder();


            hostBuilder.ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("appsettings.json", optional: true);
                configHost.AddEnvironmentVariables();
                configHost.AddCommandLine(args);  
            });

            hostBuilder.ConfigureServices(configServices => {
                configServices.AddHostedService<TimedHostedService>();
            
            });

            //hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            //{


            //    serviceCollection.AddHostedService<TimedHostedService>();
            //});

            await hostBuilder.RunConsoleAsync();
            Console.WriteLine("AcornBox.Worker - Main - Leaving");
        }
    }


    public class TimedHostedService : IHostedService, IDisposable
    {
        private BackgroundJobServer _backgroundJobServer;
        private IConfiguration _configuration;

        public TimedHostedService(IConfiguration config)
        {
            _configuration = config;
            var x = config.GetConnectionString("HangfireConnection");
        }

        public void Dispose()
        {
            _backgroundJobServer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            string connStr = _configuration.GetConnectionString("HangfireConnection");


            Console.WriteLine("AcornBox.Worker - StartAsyc - Entered");
            //Server=acornbox.db; Database=AcornBox; User=sa; Password=Your_password123
            GlobalConfiguration.Configuration.UseSqlServerStorage(connStr
                , new Hangfire.SqlServer.SqlServerStorageOptions()
                {

                });

            GlobalConfiguration.Configuration.UseFilter(new AutomaticRetryAttribute() { 
                Attempts = 1,
            });

            GlobalConfiguration.Configuration.UseActivator(new MyJobActivator(_configuration));



            _backgroundJobServer = new BackgroundJobServer(new BackgroundJobServerOptions() { 
                WorkerCount = 1,





            });

            Console.WriteLine("Hangfire Server started.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("AcornBox.Worker - StopAsync - Entered");

            _backgroundJobServer?.SendStop();
            return Task.CompletedTask;
        }
    }
}