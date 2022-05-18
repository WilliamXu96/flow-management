using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using XCZ.Localization;

namespace XCZ
{
    [DependsOn(
        typeof(AbpValidationModule),
        typeof(AbpLocalizationModule),
        typeof(AbpPermissionManagementApplicationContractsModule)
    )]
    public class FlowApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<FlowApplicationContractsModule>(("XCZ"));
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<FlowResource>("zh-Hans")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization");
            });
        }
    }
}
