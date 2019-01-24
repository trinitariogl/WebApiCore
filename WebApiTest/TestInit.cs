
namespace WebApiTest
{
    using ApplicationServiceLayer.Services;
    using CrossCutting.Utils.MappingService;
    using CrossCutting.Utils.MappingService.Contracts;
    using CrossCutting.Utils.TransactionService;
    using CrossCutting.Utils.TransactionService.Contracts;
    using DataServiceInterfaces.Services;
    using DataServiceLayer.Context;
    using DataServiceLayer.Services;
    using GenericRepository.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using UnitOfWork.Filters;
    using ApplicationServiceInterfaces.Converters;
    using ApplicationServiceInterfaces.Services;
    using ApplicationServiceLayer.Converters;

    public sealed class TestInit
    {
        private static TestInit ServiceTestInit = null;
        private static readonly object padlock = new object();

        public ServiceCollection Service { get; private set; }

        public TestInit()
        {
            Service = new ServiceCollection();
            IConfigurationRoot Configuration = TestHelper.GetIConfigurationRoot("");

            Filters();
            AutoMapper();
            ScopeContext(Configuration["Data:main"]);
            ScopeServices();
        }

        public static TestInit Instance
        {
            get
            {
                lock (padlock)
                {
                    if (ServiceTestInit == null)
                    {
                        ServiceTestInit = new TestInit();
                    }
                    return ServiceTestInit;
                }
            }
        }

        private void ScopeServices()
        {
            Service.AddScoped<ITransactionHelper, TransactionHelper>();
            Service.AddScoped<IUserAccountDataService, UserAccountDataService>();
            Service.AddScoped<IUserAccountConverter, UserAccountConverter>();
            Service.AddScoped<IUserAccountApplicationService, UserAccountApplicationService>();
        }

        private void ScopeContext(string connectionString)
        {
            Service.AddEntityFrameworkSqlServer();
            Service.AddDbContext<SecurityModelContext>(options => options.UseSqlServer(connectionString));
            Service.AddScoped<IUnitOfWork>(ctx => ctx.GetRequiredService<SecurityModelContext>());
            //Evita tenerque utilizar el ThenInclude ya que devuelve todas las relaciones.
            //Service.AddDbContext<SecurityModelContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
            Service.AddScoped<SecurityModelContext>();
        }

        private void AutoMapper()
        {
            var mapperConfig = AutoMapperConfigurator.Configure();
            var mapper = mapperConfig.CreateMapper();
            Service.AddSingleton(x => mapper);
            Service.AddTransient<IAutoMapper, AutoMapperAdapter>();
        }

        private void Filters()
        {
            Service.AddMvc(
                            config =>
                            {
                                config.Filters.Add<UnitOfWorkFilterAttribute>();
                                config.Filters.Add<ExceptionFilter>();
                            }
                        );
        }
    }
}
