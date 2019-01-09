using Microsoft.Extensions.DependencyInjection;

namespace Scalesoft.Localization.Core.Database
{
    public interface IDatabaseConfiguration
    {
        void RegisterToIoc(IServiceCollection services);
    }
}