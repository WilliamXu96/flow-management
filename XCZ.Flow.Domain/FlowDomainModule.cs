using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace XCZ
{
    [DependsOn(
        typeof(AbpDddDomainModule)
    )]
    public class FlowDomainModule : AbpModule
    {

    }
}
