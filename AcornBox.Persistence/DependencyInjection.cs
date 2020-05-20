using AcornBox.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AcornBoxDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AcornBoxDatabase")));

            services.AddScoped<IAcornBoxDbContext>(provider => provider.GetService<AcornBoxDbContext>());

            return services;
        }
    }
}
