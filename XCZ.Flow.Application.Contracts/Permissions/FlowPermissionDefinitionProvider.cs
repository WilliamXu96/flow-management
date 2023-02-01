using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using XCZ.Localization;

namespace XCZ.Permissions
{
    public class FlowPermissionDefinitionProvider : PermissionDefinitionProvider
    {

        public override void Define(IPermissionDefinitionContext context)
        {
            var flowManagement = context.AddGroup(FlowPermissions.FlowManagement, L("FlowManagement"));

            var flow = flowManagement.AddPermission(FlowPermissions.Flow.Default, L("Flow"));
            flow.AddChild(FlowPermissions.Flow.Update, L("Edit"));
            flow.AddChild(FlowPermissions.Flow.Delete, L("Delete"));
            flow.AddChild(FlowPermissions.Flow.Create, L("Create"));

            var workflow = flowManagement.AddPermission(FlowPermissions.WorkFlow.Default, L("WorkFlow"));
            workflow.AddChild(FlowPermissions.WorkFlow.DoWorkFlow, L("DoWorkFlow"));
            workflow.AddChild(FlowPermissions.WorkFlow.Create, L("Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FlowResource>(name);
        }
    }
}
