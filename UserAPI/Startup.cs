using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UserAPI.Src.Contexts;
using UserAPI.Src.Repositories;
using UserAPI.Src.Repositories.Implements;

namespace UserAPI
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
            // Database configuration
            services.AddDbContext<UserContext>(opt => opt.UseSqlServer(Configuration["ConnectionStringsDev:DefaultConnection"]));

            // Repositories
            services.AddScoped<IUser, UserRepository>();

            // Controllers
            services.AddCors();
            services.AddControllers();

            // Swagger configuration
            services.AddSwaggerGen(
            s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "UserCRUD", Version = "v1" });

                s.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT authorization header utiliza: Bearer + JWT Token",
                    }
                    );

                s.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                             {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new List<string>()
                        }
                    }
                    );

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            }
            );


        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserContext context)
        {
            // Development Environment
            if (env.IsDevelopment())
            {
                context.Database.EnsureCreated();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserCRUD v1");
                    c.RoutePrefix = string.Empty;
                });
            }

                // Production environment
                // Routes
                context.Database.EnsureCreated();

                app.UseRouting();

                app.UseCors(c => c
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
         }
    }
}
