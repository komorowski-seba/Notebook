using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Interfaces;
using WebApi.Domain.Common;
using WebApi.Infrastructure.Persistence.Repositories;

namespace WebApi.Infrastructure.Persistence
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<INoteRepository, NoteRepository>();
            return services;
        }

        public static IApplicationBuilder UsePersistenceConfiguration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            if (!dbContext?.Database.GetPendingMigrations().Any() ?? true) 
                return app;

            dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));
            if (dbContext.Database.IsNpgsql())
                dbContext.Database.Migrate();
            return app;
        }
    }
}