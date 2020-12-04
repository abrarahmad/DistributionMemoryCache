using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleDemoDMC.Model;
using SampleDemoDMC.Services;

namespace SampleDemoDMC
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
            services.AddDistributedMemoryCache();
            services.AddScoped<IDopDistributionCache, DopDistributionCache>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddSwaggerGen(action =>
            {
                action.SwaggerDoc("DistributionCacheAPI", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Distribution Cache",
                    Version = "1",
                    Description = "Demo distribution cache"
                });
            });

            services.Configure<CacheConfiguration>(Configuration.GetSection("ExternalServices:CLQ"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {


                action.SwaggerEndpoint("/swagger/DistributionCacheAPI/swagger.json", "Distribution Cache v1");

            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
