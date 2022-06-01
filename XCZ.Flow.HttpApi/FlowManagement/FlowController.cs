using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using XCZ.FlowManagement.Dto;

namespace XCZ.FlowManagement
{
    [RemoteService]
    [Area("business")]
    [Route("api/business/flow")]
    public class FlowController : AbpController, IFlowAppService
    {
        private readonly IFlowAppService _flowAppService;

        public FlowController(IFlowAppService flowAppService)
        {
            _flowAppService = flowAppService;
        }

        [HttpPost]
        public Task<FlowDto> Create(CreateOrUpdateFlowDto input)
        {
            return _flowAppService.Create(input);
        }

        [HttpPost]
        [Route("delete")]
        public Task Delete(List<Guid> ids)
        {
            return _flowAppService.Delete(ids);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<FlowDto> Get(Guid id)
        {
            return _flowAppService.Get(id);
        }

        [HttpGet]
        [Route("by-node")]
        public Task<FlowDto> GetByNode(GetFlowByNodeInputDto input)
        {
            return _flowAppService.GetByNode(input);
        }

        [HttpGet]
        public Task<PagedResultDto<FlowDto>> GetAll(GetFlowInputDto input)
        {
            return _flowAppService.GetAll(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<FlowDto> Update(Guid id, CreateOrUpdateFlowDto input)
        {
            return _flowAppService.Update(id, input);
        }
    }
}
