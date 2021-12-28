using System.Collections.Generic;
using System.Linq;
using System.Net;
using Api.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestSharp;
using WebApi.Application;
using WebApi.Application.Services;
using WebApi.Common.Exceptions;
using WebApi.Common.Interfaces;
using WebApi.Common.Models;
using WebApi.Infrastructure;
using WebApi.Infrastructure.Persistence;

namespace Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IList<SecurityKey> SecurityCerts { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SecurityCerts = GetSecurityKeys(Configuration["Jwt:Issuer"]);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddApplicationServices();
            services.AddPersistenceServices(Configuration);
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Note Rest API",
                    Description = "A simple service for creating and downloading notes"
                });
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Jwt:Issuer"];
                    options.Audience = Configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = false;
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = false,
                        IssuerSigningKeys = SecurityCerts
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UsePersistenceConfiguration();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseConfigureExeptions();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static IList<SecurityKey> GetSecurityKeys(string issuer)
        {
            var client = new RestClient($"{issuer}/protocol/openid-connect/certs");
            var request = new RestRequest(Method.GET);
            var restResponse =  client.Execute<KeycloakCertsModel>(request);
            if (restResponse.StatusCode != HttpStatusCode.OK)
                throw new SecurityKeysException("I can't dawnload the certificate list");

            var result = restResponse
                .Data
                .Keys
                .Cast<SecurityKey>()
                .ToList();
            return result;
        }
    }
}