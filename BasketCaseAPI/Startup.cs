using Core.CrossCuttingConcerns.Caching;
using Core.Caching.Cache;
using Core.Caching.Stack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using BasketCase.Business.Manager;
using BasketCase.Core.AOP.Handler;

namespace BasketCaseAPI
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BasketCase API",
                    Version = "v1"
                });
            });

            services.Configure<Configuration>(Configuration.GetSection("Redis"));
            services.AddOptions();

            ConfigurationExtends(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketCase API V1");
                });
            }

            // Genel hata yakalama ve custom mesaj gönderme özelliği ekledik.
            app.UseMiddleware<ExceptionHandlerExMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigurationExtends(IServiceCollection services)
        {
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IRedisStack, RedisStack>();
            services.AddSingleton<IBasketService, BasketManager>();
        }
    }
}
