using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using XCZ.FlowManagement.Dto;
using XCZ.FormManagement;

namespace XCZ.FlowManagement
{
    public class FlowAppService : ApplicationService, IFlowAppService
    {
        private readonly IRepository<BaseFlow, Guid> _baseRep;
        private readonly IRepository<FlowNode, Guid> _nodeRep;
        private readonly IRepository<FlowLine, Guid> _linkRep;
        private readonly IRepository<LineForm, Guid> _linkFormRep;
        private readonly IRepository<Form, Guid> _formRep;
        private readonly IRepository<FormField, Guid> _formFieldRep;

        public FlowAppService(
            IRepository<BaseFlow, Guid> baseRep,
            IRepository<FlowNode, Guid> nodeRep,
            IRepository<FlowLine, Guid> linkRep,
            IRepository<LineForm, Guid> linkFormRep,
            IRepository<Form, Guid> formRep,
            IRepository<FormField, Guid> formFieldRep)
        {
            _baseRep = baseRep;
            _nodeRep = nodeRep;
            _linkRep = linkRep;
            _linkFormRep = linkFormRep;
            _formRep = formRep;
            _formFieldRep = formFieldRep;
        }

        public async Task<FlowDto> Create(CreateOrUpdateFlowDto input)
        {
            if (await _baseRep.AnyAsync(_ => _.FormId == input.FormId))
                throw new BusinessException("新增失败：选择表单已使用");
            var baseFlowId = GuidGenerator.Create();
            var baseFlow = await _baseRep.InsertAsync(new BaseFlow(baseFlowId)
            {
                TenantId = CurrentTenant.Id,
                FormId = input.FormId,
                Title = input.Title,
                Code = input.Code,
                UseDate = input.UseDate,
                Level = input.Level,
                Remark = input.Remark
            });

            await InsertNodes(baseFlowId, input.NodeList);
            await InsertLines(baseFlow, input.LineList);

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
                //TODO：delete WF
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
                link.FormField = ObjectMapper.Map<List<LineForm>, List<LineFormDto>>(linkForms.Where(_ => _.FlowLineId == link.Id).ToList());
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

            var forms = await (await _formRep.GetQueryableAsync()).Where(_ => items.Select(i => i.FormId).Contains(_.Id)).ToListAsync();

            var dtos = ObjectMapper.Map<List<BaseFlow>, List<FlowDto>>(items);
            foreach (var dto in dtos)
            {
                dto.FormName = forms.FirstOrDefault(_ => _.Id == dto.FormId)?.FormName;
            }
            return new PagedResultDto<FlowDto>(totalCount, dtos);
        }

        public async Task<FlowDto> Update(Guid id, CreateOrUpdateFlowDto input)
        {
            var baseFlow = await _baseRep.GetAsync(id);
            await _nodeRep.DeleteAsync(_ => _.BaseFlowId == id);
            await _linkRep.DeleteAsync(_ => _.BaseFlowId == id);
            await _linkFormRep.DeleteAsync(_ => _.BaseFlowId == id);
            await InsertNodes(id, input.NodeList);
            await InsertLines(baseFlow, input.LineList);
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
                    Executor = node.Executor.IsNullOrWhiteSpace() ? null : node.Executor,
                    Users = node.Users.IsNullOrEmpty() ? null : string.Join(",", node.Users),
                    Roles = node.Roles.IsNullOrEmpty() ? null : string.Join(",", node.Roles),
                    Remark = node.Remark
                });
            }
        }

        private async Task InsertLines(BaseFlow flow, List<CreateOrUpdateFlowLineDto> links)
        {
            var fields = await _formFieldRep.GetListAsync(_ => _.FormId == flow.FormId);
            foreach (var link in links)
            {
                var flowLineId = GuidGenerator.Create();
                await _linkRep.InsertAsync(new FlowLine(flowLineId)
                {
                    TenantId = CurrentTenant.Id,
                    BaseFlowId = flow.Id,
                    Label = link.Label,
                    From = link.From,
                    To = link.To,
                    Remark = link.Remark
                });

                if (link.FormField == null) continue;

                foreach (var form in link.FormField)
                {
                    var field = fields.FirstOrDefault(_ => _.Id == form.FieldId);
                    if (field == null) continue;
                    await _linkFormRep.InsertAsync(new LineForm(GuidGenerator.Create())
                    {
                        TenantId = CurrentTenant.Id,
                        BaseFlowId = flow.Id,
                        FlowLineId = flowLineId,
                        FieldId = form.FieldId,
                        FieldName = field.FieldName,
                        FieldType = field.DataType,
                        Condition = form.Condition,
                        Content = form.Content,
                        IntContent = field.DataType == "int" ? int.Parse(form.Content) : 0,
                        Remark = form.Remark
                    }); ;
                }
            }
        }
    }
}
