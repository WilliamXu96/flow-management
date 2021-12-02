using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using XCZ.FlowManagement.Dto;

namespace XCZ.FlowManagement
{
    public class FlowAppService : ApplicationService, IFlowAppService
    {
        private readonly IRepository<BaseFlow, Guid> _baseRep;
        private readonly IRepository<FlowNode, Guid> _nodeRep;
        private readonly IRepository<FlowLink, Guid> _linkRep;
        private readonly IRepository<LinkForm, Guid> _linkFormRep;

        public FlowAppService(
            IRepository<BaseFlow, Guid> baseRep,
            IRepository<FlowNode, Guid> nodeRep,
            IRepository<FlowLink, Guid> linkRep,
            IRepository<LinkForm, Guid> linkFormRep)
        {
            _baseRep = baseRep;
            _nodeRep = nodeRep;
            _linkRep = linkRep;
            _linkFormRep = linkFormRep;
        }

        public async Task<FlowDto> Create(CreateOrUpdateFlowDto input)
        {
            var baseFlowId = GuidGenerator.Create();
            var baseFlow = await _baseRep.InsertAsync(new BaseFlow(baseFlowId)
            {
                TenantId = CurrentTenant.Id,
                FlowId = input.FlowId,
                FormId = input.FormId,
                Title = input.Title,
                Code = input.Code,
                UseDate = input.UseDate,
                Level = input.Level,
                Remark = input.Remark
            });

            foreach (var node in input.NodeList)
            {
                await _nodeRep.InsertAsync(new FlowNode(GuidGenerator.Create())
                {

                })
            }
            
        }

        public Task Delete(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<FlowDto> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<FlowDto>> GetAll(GetFlowInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<FlowDto> Update(Guid id, CreateOrUpdateFlowDto input)
        {
            throw new NotImplementedException();
        }
    }
}
