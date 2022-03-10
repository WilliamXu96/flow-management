﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using XCZ.Dtos;
using XCZ.WrokFlowManagement.Dto;

namespace XCZ.WrokFlowManagement
{
    [RemoteService]
    [Area("business")]
    [Route("api/business/workflow")]
    public class WrokFlowController : AbpController
    {
        private readonly IWrokFlowAppService _wrokFlowAppService;

        public WrokFlowController(IWrokFlowAppService wrokFlowAppService)
        {
            _wrokFlowAppService = wrokFlowAppService;
        }

        [HttpPost]
        [Route("status")]
        public Task<ListResultDto<FormWorkFlowStatusDto>> GetWorkFlowStatus(List<Guid> ids)
        {
            return _wrokFlowAppService.GetWorkFlowStatus(ids);
        }

        [HttpPut]
        [Route("do/{entityId}")]
        public Task DoWorkFlow(Guid entityId, DoWorkFlowInputDto input)
        {
            return _wrokFlowAppService.DoWorkFlow(entityId, input);
        }
    }
}
