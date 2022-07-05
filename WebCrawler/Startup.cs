using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain;
using WebCrawler.Domain.Repositories.EntityFramework;
using WebCrawler.Domain.Repositories.Interfaces;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCrawler.Domain.Repositories.MemoryRepository;
using WebCrawler.Domain.Crawlers;
using WebCrawler.Domain.Parsers;
using WebCrawler.Domain.Entities;
using System.Net.Http;
using ServiceStack;

namespace WebCrawler
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Поддержка контроллеров и представлений
            services.AddControllersWithViews()
                // Совместимость в Asp.Net Core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            // Fill a DI-container for EF realization
            services.AddTransient<IArticlesRepository, EFArticlesRepository>();
            services.AddScoped<IPersonAttributesRepository, EFPersonAttributesRepository>();
            services.AddScoped<IOrganizationAttributesRepository, EFOrganizationAttributesRepository>();
            services.AddScoped<IGeoAttributesRepository, EFGeoAttributesRepository>();

            // Fill a DI-container for inMemory realization
            //services.AddSingleton<IArticlesRepository, MemoryArticlesRepository>();
            //services.AddSingleton<IPersonAttributesRepository, MemoryPersonAttributesRepository>();
            //services.AddSingleton<IOrganizationAttributesRepository, MemoryOrganizationAttributesRepository>();
            //services.AddSingleton<IGeoAttributesRepository, MemoryGeoAttributesRepository>();

            services.AddScoped<Storage>();
            services.AddTransient<MyCrawler>();
            services.AddTransient<MyParser>();
            services.AddTransient<HttpClient>();
            // Connect to DataBase
            services.AddDbContext<AppDbContext>(x => x.UseNpgsql(Configuration["ConnectionString"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Поддержка статических файлов
            app.UseStaticFiles();

            // Регистрация маршрутов
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
