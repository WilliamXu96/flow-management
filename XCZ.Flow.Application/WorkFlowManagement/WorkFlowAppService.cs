using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using XCZ.Dtos;
using XCZ.Permissions;
using XCZ.WorkFlow;
using XCZ.WorkFlowManagement.Dto;

namespace XCZ.WorkFlowManagement
{
    [Authorize(FlowPermissions.WorkFlow.Default)]
    public class WorkFlowAppService : ApplicationService, IWorkFlowAppService
    {
        private readonly FormWorkFlowManager _formWorkFlowManager;
        private readonly IRepository<FormWorkFlow, Guid> _formWorkFlowRepository;

        public WorkFlowAppService(
            FormWorkFlowManager formWorkFlowManager,
            IRepository<FormWorkFlow, Guid> formWorkFlowRepository
            )
        {
            _formWorkFlowManager = formWorkFlowManager;
            _formWorkFlowRepository = formWorkFlowRepository;
        }

        [Authorize(FlowPermissions.WorkFlow.Create)]
        public async Task CreateWorkFlow(string formName, object obj)
        {
            var workflow = await _formWorkFlowManager.CreateAsync(formName, obj);
            await _formWorkFlowRepository.InsertAsync(workflow);
        }

        [Authorize(FlowPermissions.WorkFlow.DoWorkFlow)]
        public async Task DoWorkFlow(Guid entityId, DoWorkFlowInputDto input)
        {
            var workflow = await _formWorkFlowManager.DoWorkFlowAsync(entityId,input.Data, CurrentUser.UserName, CurrentUser.Roles);
            await _formWorkFlowRepository.UpdateAsync(workflow);
        }

        public async Task<ListResultDto<FormWorkFlowStatusDto>> GetWorkFlowStatus(List<Guid> ids)
        {
            var result = await (await _formWorkFlowRepository.GetQueryableAsync()).Where(_ => ids.Contains(_.EntityId))
                                                                      .Select(s => new FormWorkFlowStatusDto
                                                                      {
                                                                          EntityId = s.EntityId,
                                                                          Status = s.Status
                                                                      })
                                                                      .ToListAsync();
            return new ListResultDto<FormWorkFlowStatusDto>(result);
        }
    }
}
