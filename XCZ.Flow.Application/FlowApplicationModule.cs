﻿using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace XCZ
{
    [DependsOn(
       typeof(AbpDddApplicationModule),
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
    }
}
