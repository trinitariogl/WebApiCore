
namespace UnitOfWork
{
    using CrossCutting.Ioc;
    using DataServiceLayer.Context;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using UnitOfWork.Filters;
    using UnitOfWork.IoC;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
               .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            ContainerSetup.Setup(services, Configuration);
            LogSecurityModule.Setup(services, Configuration);
            services.AddScoped<UnitOfWorkFilterAttribute>();

            //var connection = @"Server=(localdb)\mssqllocaldb;Database=LogSecurity;Trusted_Connection=True;ConnectRetryCount=0";
            //services.AddDbContext<SecurityModelContext>(options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
