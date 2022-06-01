using Volo.Abp.Application.Services;
using XCZ.WorkFlowManagement;

namespace XCZ
{
    public abstract class BusinessWorkFlowAppService : ApplicationService
    {
        public IWorkFlowAppService WorkFlow { get; set; }
    }
}
