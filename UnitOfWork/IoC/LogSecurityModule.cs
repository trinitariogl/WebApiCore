
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
    using System.Reflection;
    using System.Linq;
    using System.Runtime.CompilerServices;

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

            AddServices(services);
            AddConverters(services);
            AddApplicationService(services);
        }

        //Aplicaría la injección de dependencia a todas las clases con el mismo namespace
        private static void AddApplicationService(IServiceCollection services)
        {
            var applicationServiceTypeType = typeof(UserAccountApplicationService);
            var types = (from t in applicationServiceTypeType.GetTypeInfo().Assembly.GetTypes()
                         where t.Namespace == applicationServiceTypeType.Namespace
                               && t.GetTypeInfo().IsClass
                               && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                         select t).ToArray();

            foreach (var type in types)
            {
                var interfaceQ = type.GetTypeInfo().GetInterfaces().First();
                services.AddScoped(interfaceQ, type);
            }
        }

        //Aplicaría la injección de dependencia a todas las clases con el mismo namespace
        private static System.Type[] AddConverters(IServiceCollection services)
        {
            var convertesType = typeof(UserAccountConverter);
            var types = (from t in convertesType.GetTypeInfo().Assembly.GetTypes()
                         where t.Namespace == convertesType.Namespace
                               && t.GetTypeInfo().IsClass
                               && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                         select t).ToArray();

            foreach (var type in types)
            {
                var interfaceQ = type.GetTypeInfo().GetInterfaces().First();
                services.AddScoped(interfaceQ, type);
            }

            return types;
        }

        //Aplicaría la injección de dependencia a todas las clases con el mismo namespace
        private static void AddServices(IServiceCollection services)
        {
            var dataServiceType = typeof(UserAccountDataService);
            var types = (from t in dataServiceType.GetTypeInfo().Assembly.GetTypes()
                     where t.Namespace == dataServiceType.Namespace
                           && t.GetTypeInfo().IsClass
                           && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                     select t).ToArray();
            foreach (var type in types)
            {
                //En este caso seleccionamos la última interfaz especificada en la clase UserAccountDataService
                var interfaceQ = type.GetTypeInfo().GetInterfaces().Last();
                services.AddScoped(interfaceQ, type);
            }
        }
    }
}
