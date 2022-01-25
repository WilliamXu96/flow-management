using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using XCZ.FlowManagement.Dto;

namespace XCZ.FlowManagement
{
    public class FlowAppService : ApplicationService, IFlowAppService
    {
        private readonly IRepository<BaseFlow, Guid> _baseRep;
        private readonly IRepository<FlowNode, Guid> _nodeRep;
        private readonly IRepository<FlowLine, Guid> _linkRep;
        private readonly IRepository<LineForm, Guid> _linkFormRep;

        public FlowAppService(
            IRepository<BaseFlow, Guid> baseRep,
            IRepository<FlowNode, Guid> nodeRep,
            IRepository<FlowLine, Guid> linkRep,
            IRepository<LineForm, Guid> linkFormRep)
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
                //FlowId = input.FlowId,
                FormId = input.FormId,
                Title = input.Title,
                Code = input.Code,
                UseDate = input.UseDate,
                Level = input.Level,
                Remark = input.Remark
            });

            await InsertNodes(baseFlowId, input.NodeList);
            await InsertLinks(baseFlowId, input.LineList);

            return ObjectMapper.Map<BaseFlow, FlowDto>(baseFlow);
        }

        public async Task Delete(List<Guid> ids)
        {
            foreach (var id in ids)
            {
                await _baseRep.DeleteAsync(_ => _.Id == id);
                await _nodeRep.DeleteAsync(_ => _.BaseFlowId == id);
                await _linkRep.DeleteAsync(_ => _.BaseFlowId == id);
                await _linkFormRep.DeleteAsync(_ => _.BaseFlowId == id);
            }
        }

        public async Task<FlowDto> Get(Guid id)
        {
            var baseFlow = await _baseRep.GetAsync(id);
            var flowNodes = await (await _nodeRep.GetQueryableAsync()).Where(_ => _.BaseFlowId == id).ToListAsync();
            var flowLinks = await (await _linkRep.GetQueryableAsync()).Where(_ => _.BaseFlowId == id).ToListAsync();
            var linkForms = await (await _linkFormRep.GetQueryableAsync()).Where(_ => _.BaseFlowId == id).ToListAsync();

            var dto = ObjectMapper.Map<BaseFlow, FlowDto>(baseFlow);
            dto.NodeList = ObjectMapper.Map<List<FlowNode>, List<FlowNodeDto>>(flowNodes);
            dto.LineList = ObjectMapper.Map<List<FlowLine>, List<FlowLinkDto>>(flowLinks);
            foreach (var link in dto.LineList)
            {
                link.FormField = ObjectMapper.Map<List<LineForm>, List<LineFormDto>>(linkForms.Where(_ => _.FlowLinkId == link.Id).ToList());
            }

            return dto;
        }

        public async Task<PagedResultDto<FlowDto>> GetAll(GetFlowInputDto input)
        {
            var query = (await _baseRep.GetQueryableAsync()).WhereIf(!input.Filter.IsNullOrWhiteSpace(), _ => _.Code.Contains(input.Filter) || _.Title.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting ?? "CreationTime desc")
                                   .Skip(input.SkipCount)
                                   .Take(input.MaxResultCount)
                                   .ToListAsync();

            var dto = ObjectMapper.Map<List<BaseFlow>, List<FlowDto>>(items);
            return new PagedResultDto<FlowDto>(totalCount, dto);
        }

        public async Task<FlowDto> Update(Guid id, CreateOrUpdateFlowDto input)
        {
            var baseFlow = await _baseRep.GetAsync(id);
            //baseFlow.FormId = input.FormId;
            //baseFlow.Title = input.Title;
            //baseFlow.UseDate = input.UseDate;
            //baseFlow.Level = input.Level;
            //baseFlow.Remark = input.Remark;
            await _nodeRep.DeleteAsync(_ => _.BaseFlowId == id);
            await _linkRep.DeleteAsync(_ => _.BaseFlowId == id);
            await _linkFormRep.DeleteAsync(_ => _.BaseFlowId == id);
            await InsertNodes(id, input.NodeList);
            await InsertLinks(id, input.LineList);
            return ObjectMapper.Map<BaseFlow, FlowDto>(baseFlow);
        }

        private async Task InsertNodes(Guid baseFlowId, List<CreateOrUpdateFlowNodeDto> nodes)
        {
            foreach (var node in nodes)
            {
                await _nodeRep.InsertAsync(new FlowNode(GuidGenerator.Create())
                {
                    TenantId = CurrentTenant.Id,
                    BaseFlowId = baseFlowId,
                    NodeId = node.Id,
                    Name = node.Name,
                    Type = node.Type,
                    Left = node.Left,
                    Top = node.Top,
                    Ico = node.Ico,
                    State = node.State,
                    Executor = node.Executor,
                    Users = node.Users,
                    Roles = node.Roles,
                    Remark = node.Remark
                });
            }
        }

        private async Task InsertLinks(Guid baseFlowId, List<CreateOrUpdateFlowLineDto> links)
        {
            foreach (var link in links)
            {
                var flowLinkId = GuidGenerator.Create();
                await _linkRep.InsertAsync(new FlowLine(flowLinkId)
                {
                    TenantId = CurrentTenant.Id,
                    BaseFlowId = baseFlowId,
                    //LineId = link.Id,
                    Label = link.Label,
                    From = link.From,
                    To = link.To,
                    //TargetId = link.To,
                    Remark = link.Remark
                });

                if (link.FormField == null) continue;
                foreach (var form in link.FormField)
                {
                    await _linkFormRep.InsertAsync(new LineForm(GuidGenerator.Create())
                    {
                        TenantId = CurrentTenant.Id,
                        BaseFlowId = baseFlowId,
                        FlowLinkId = flowLinkId,
                        //Pid = link.Id,
                        FieldId = form.FieldId,
                        Condition = form.Condition,
                        Content = form.Content,
                        Remark = form.Remark
                    });
                }
            }
        }
    }
}
