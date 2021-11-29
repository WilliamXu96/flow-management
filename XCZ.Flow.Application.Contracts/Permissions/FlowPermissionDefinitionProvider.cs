using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using XCZ.Localization;

namespace XCZ.Permissions
{
    public class FlowPermissionDefinitionProvider: PermissionDefinitionProvider
    {

        public override void Define(IPermissionDefinitionContext context)
        {
            var business = context.AddGroup(FlowPermissions.Flow, L("Flow"), MultiTenancySides.Tenant);

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FlowResource>(name);
        }
    }
}
