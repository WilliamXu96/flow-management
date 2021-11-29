using Volo.Abp.AutoMapper;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace XCZ
{
    [DependsOn(
       typeof(FlowDomainModule),
       typeof(FlowApplicationContractsModule),
       typeof(AbpAutoMapperModule)
    )]
    public class FlowApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<FlowApplicationModule>();
            });
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpJsonOptions>(option =>
            {
                option.UseHybridSerializer = false;
            });
        }
    }
}
