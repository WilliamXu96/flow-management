using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using XCZ.Dtos;
using XCZ.WorkFlowManagement.Dto;

namespace XCZ.WorkFlowManagement
{
    public interface IWorkFlowAppService : IApplicationService
    {
        Task CreateWorkFlow(string formName, object obj);

        Task DoWorkFlow(Guid entityId, DoWorkFlowInputDto input);

        Task<ListResultDto<FormWorkFlowStatusDto>> GetWorkFlowStatus(List<Guid> ids);

        Task<ListResultDto<FormWorkFlowNodeStatusDto>> GetWorkFlowNodeStatus(List<Guid> ids);
    }
}
