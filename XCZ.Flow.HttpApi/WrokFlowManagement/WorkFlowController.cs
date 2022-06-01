using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using XCZ.Dtos;
using XCZ.WorkFlowManagement.Dto;

namespace XCZ.WorkFlowManagement
{
    [RemoteService]
    [Area("business")]
    [Route("api/business/workflow")]
    public class WorkFlowController : AbpController
    {
        private readonly IWorkFlowAppService _workFlowAppService;

        public WorkFlowController(IWorkFlowAppService workFlowAppService)
        {
            _workFlowAppService = workFlowAppService;
        }

        [HttpPost]
        [Route("status")]
        public Task<ListResultDto<FormWorkFlowStatusDto>> GetWorkFlowStatus(List<Guid> ids)
        {
            return _workFlowAppService.GetWorkFlowStatus(ids);
        }

        [HttpPost]
        [Route("node-status")]
        public Task<ListResultDto<FormWorkFlowNodeStatusDto>> GetWorkFlowNodeStatus(List<Guid> ids)
        {
            return _workFlowAppService.GetWorkFlowNodeStatus(ids);
        }

        [HttpPut]
        [Route("do/{entityId}")]
        public Task DoWorkFlow(Guid entityId, DoWorkFlowInputDto input)
        {
            return _workFlowAppService.DoWorkFlow(entityId, input);
        }
    }
}
