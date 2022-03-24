using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace XCZ
{
    [DependsOn(
        typeof(FlowApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class FlowHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(FlowHttpApiModule).Assembly);
            });
        }
    }
}
