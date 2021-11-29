using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace XCZ.EntityFrameworkCore
{
    [DependsOn(
       typeof(FlowDomainModule),
       typeof(AbpEntityFrameworkCoreModule)
    )]
    public class FlowEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FlowDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
