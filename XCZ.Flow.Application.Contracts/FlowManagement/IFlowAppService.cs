using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using XCZ.FlowManagement.Dto;

namespace XCZ.FlowManagement
{
    public interface IFlowAppService : IApplicationService
    {
        Task<FlowDto> Create(CreateOrUpdateFlowDto input);

        Task Delete(List<Guid> ids);

        Task<FlowDto> Update(Guid id, CreateOrUpdateFlowDto input);

        Task<PagedResultDto<FlowDto>> GetAll(GetFlowInputDto input);

        Task<FlowDto> Get(Guid id);

        Task<FlowDto> GetByNode(GetFlowByNodeInputDto input);
    }
}
