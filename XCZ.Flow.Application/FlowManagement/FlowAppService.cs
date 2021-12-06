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

            //foreach (var node in input.NodeList)
            //{
            //    await _nodeRep.InsertAsync(new FlowNode(GuidGenerator.Create())
            //    {
            //        TenantId = CurrentTenant.Id,
            //        BaseFlowId = baseFlowId,
            //        NodeId = node.NodeId,
            //        NodeName = node.NodeName,
            //        Type = node.Type,
            //        Height = node.Height,
            //        X = node.X,
            //        Width = node.Width,
            //        Y = node.Y,
            //        Remark = node.Remark
            //    });
            //}

            await InsertNodes(baseFlowId, input.NodeList);
            await InsertLinks(baseFlowId, input.LinkList);

            //foreach (var link in input.LinkList)
            //{
            //    var flowLinkId = GuidGenerator.Create();
            //    await _linkRep.InsertAsync(new FlowLink(flowLinkId)
            //    {
            //        TenantId = CurrentTenant.Id,
            //        BaseFlowId = baseFlowId,
            //        LinkId = link.LinkId,
            //        Label = link.Label,
            //        Type = link.Type,
            //        SourceId = link.SourceId,
            //        TargetId = link.TargetId,
            //        Remark = link.Remark
            //    });

            //    foreach (var form in link.TempFieldForm)
            //    {
            //        await _linkFormRep.InsertAsync(new LinkForm(GuidGenerator.Create())
            //        {
            //            TenantId = CurrentTenant.Id,
            //            BaseFlowId = baseFlowId,
            //            FlowLinkId = flowLinkId,
            //            FieldId = form.FieldId,
            //            Condition = form.Condition,
            //            Content = form.Content,
            //            Remark = form.Remark
            //        });
            //    }
            //}

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
            var flowNodes = await _nodeRep.Where(_ => _.BaseFlowId == id).ToListAsync();
            var flowLinks = await _linkRep.Where(_ => _.BaseFlowId == id).ToListAsync();
            var linkForms = await _linkFormRep.Where(_ => _.BaseFlowId == id).ToListAsync();

            var dto = ObjectMapper.Map<BaseFlow, FlowDto>(baseFlow);
            dto.NodeList = ObjectMapper.Map<List<FlowNode>, List<FlowNodeDto>>(flowNodes);
            dto.LinkList = ObjectMapper.Map<List<FlowLink>, List<FlowLinkDto>>(flowLinks);
            foreach (var link in dto.LinkList)
            {
                var tempFieldForm = linkForms.Where(_ => _.FlowLinkId == link.Id).ToList();
                link.TempFieldForm = ObjectMapper.Map<List<LinkForm>, List<LinkFormDto>>(tempFieldForm);
            }

            return dto;
        }

        public async Task<PagedResultDto<FlowDto>> GetAll(GetFlowInputDto input)
        {
            var query = _baseRep.WhereIf(!input.Filter.IsNullOrWhiteSpace(), _ => _.Code.Contains(input.Filter) || _.Title.Contains(input.Filter));

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
            baseFlow.FormId = input.FormId;
            baseFlow.Title = input.Title;
            baseFlow.UseDate = input.UseDate;
            baseFlow.Level = input.Level;
            baseFlow.Remark = input.Remark;
            await _nodeRep.DeleteAsync(_ => _.BaseFlowId == id);
            await _linkRep.DeleteAsync(_ => _.BaseFlowId == id);
            await _linkFormRep.DeleteAsync(_ => _.BaseFlowId == id);
            await InsertNodes(id, input.NodeList);
            await InsertLinks(id, input.LinkList);
            return ObjectMapper.Map<BaseFlow, FlowDto>(baseFlow);
        }

        private async Task InsertNodes(Guid baseFlowId, List<FlowNodeDto> nodes)
        {
            foreach (var node in nodes)
            {
                await _nodeRep.InsertAsync(new FlowNode(GuidGenerator.Create())
                {
                    TenantId = CurrentTenant.Id,
                    BaseFlowId = baseFlowId,
                    NodeId = node.NodeId,
                    NodeName = node.NodeName,
                    Type = node.Type,
                    Height = node.Height,
                    X = node.X,
                    Width = node.Width,
                    Y = node.Y,
                    Remark = node.Remark
                });
            }
        }

        private async Task InsertLinks(Guid baseFlowId, List<FlowLinkDto> links)
        {
            foreach (var link in links)
            {
                var flowLinkId = GuidGenerator.Create();
                await _linkRep.InsertAsync(new FlowLink(flowLinkId)
                {
                    TenantId = CurrentTenant.Id,
                    BaseFlowId = baseFlowId,
                    LinkId = link.LinkId,
                    Label = link.Label,
                    Type = link.Type,
                    SourceId = link.SourceId,
                    TargetId = link.TargetId,
                    Remark = link.Remark
                });

                foreach (var form in link.TempFieldForm)
                {
                    await _linkFormRep.InsertAsync(new LinkForm(GuidGenerator.Create())
                    {
                        TenantId = CurrentTenant.Id,
                        BaseFlowId = baseFlowId,
                        FlowLinkId = flowLinkId,
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
