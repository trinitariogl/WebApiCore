
namespace CrossCutting.Utils.MappingService.Contracts
{
    using AutoMapper;

    public interface IAutoMapperTypeConfigurator
    {
        void Configure(IMapperConfigurationExpression configuration);
    }
}
