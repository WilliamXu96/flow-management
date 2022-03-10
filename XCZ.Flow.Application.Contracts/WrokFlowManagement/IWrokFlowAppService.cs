using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using XCZ.Dtos;
using XCZ.Enums;
using XCZ.WrokFlowManagement.Dto;

namespace XCZ.WrokFlowManagement
{
    public interface IWrokFlowAppService : IApplicationService
    {
        Task CreateWorkFlow(string formName, object obj);

        Task DoWorkFlow(Guid entityId, DoWorkFlowInputDto input);

        Task<ListResultDto<FormWorkFlowStatusDto>> GetWorkFlowStatus(List<Guid> ids);
    }
}
