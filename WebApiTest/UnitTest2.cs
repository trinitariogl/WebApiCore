
namespace WebApiTest
{
    using ApplicationServiceInterfaces.Models;
    using ApplicationServiceInterfaces.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;
    using Xunit;

    public class UnitTest2
    {
        public ServiceCollection service = null;

        public UnitTest2()
        {
            service = TestInit.Instance.Service;
        }

        [Fact]
        public async Task Test2()
        {
            ServiceProvider serviceProvider = service.BuildServiceProvider();
            IUserAccountApplicationService userBusiness = serviceProvider.GetRequiredService<IUserAccountApplicationService>();
            UserAccountDto user = await userBusiness.FindUserById("1ea57e74-6984-4d98-86b3-c516c464b356");
            Assert.NotNull(user.Id);
        }
    }
}
