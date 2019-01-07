using Microsoft.Extensions.DependencyInjection;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseConfiguration
    {
        void RegisterToIoc(IServiceCollection services);
    }
}