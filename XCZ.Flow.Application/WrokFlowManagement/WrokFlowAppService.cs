using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using XCZ.Dtos;
using XCZ.WrokFlow;

namespace XCZ.WrokFlowManagement
{
    public class WrokFlowAppService : ApplicationService, IWrokFlowAppService
    {
        private readonly FormWorkFlowManager _formWorkFlowManager;
        private readonly IRepository<FormWorkFlow, Guid> _formWorkFlowRepository;

        public WrokFlowAppService(
            FormWorkFlowManager formWorkFlowManager,
            IRepository<FormWorkFlow, Guid> formWorkFlowRepository
            )
        {
            _formWorkFlowManager = formWorkFlowManager;
            _formWorkFlowRepository = formWorkFlowRepository;
        }

        public async Task CreateWorkFlow(string formName, Guid entityId)
        {
            var workflow = await _formWorkFlowManager.CreateAsync(formName, entityId);
            await _formWorkFlowRepository.InsertAsync(workflow);
        }

        public async Task DoWorkFlow(Guid entityId)
        {
            var workflow = await _formWorkFlowManager.DoWorkFlowAsync(entityId, CurrentUser.UserName, CurrentUser.Roles);
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
