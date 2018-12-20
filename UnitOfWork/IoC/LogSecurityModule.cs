
namespace UnitOfWork.IoC
{
    using ApplicationServiceInterfaces.Converters;
    using ApplicationServiceInterfaces.Services;
    using ApplicationServiceLayer.Converters;
    using ApplicationServiceLayer.Services;
    using DataServiceInterfaces.Services;
    using DataServiceLayer.Context;
    using DataServiceLayer.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore.Proxies;
    using GenericRepository.UnitOfWork;

    public static class LogSecurityModule
    {
        public static void Setup(IServiceCollection services, IConfigurationRoot configuration)
        {
            AddServices(services, configuration);
        }

        private static void AddServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration["Data:main"];

            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<SecurityModelContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork>(ctx => ctx.GetRequiredService<SecurityModelContext>());

            //Evita tenerque utilizar el ThenInclude ya que devuelve todas las relaciones.
            //services.AddDbContext<SecurityModelContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
            services.AddScoped<SecurityModelContext>();

            services.AddScoped<IUserAccountDataService, UserAccountDataService>();
            services.AddScoped<IUserAccountConverter, UserAccountConverter>();
            services.AddScoped<IUserAccountApplicationService, UserAccountApplicationService>();
        }
    }
}
