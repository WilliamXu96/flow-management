using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using XCZ.Enums;
using XCZ.FlowManagement;
using XCZ.FormManagement;

namespace XCZ.WrokFlow
{
    public class FormWorkFlowManager : DomainService
    {
        private readonly IRepository<BaseFlow, Guid> _baseflowRepository;
        private readonly IRepository<FlowNode, Guid> _nodeRepository;
        private readonly IRepository<FlowLine, Guid> _linkRepository;
        private readonly IRepository<LineForm, Guid> _lineFormRepository;
        private readonly IRepository<Form, Guid> _formRepository;
        private readonly IRepository<FormWorkFlow, Guid> _formWorkFlowRepository;

        public FormWorkFlowManager(
            IRepository<BaseFlow, Guid> baseRep,
            IRepository<FlowNode, Guid> nodeRep,
            IRepository<FlowLine, Guid> lineRep,
            IRepository<LineForm, Guid> lineFormRep,
            IRepository<Form, Guid> formRepository,
            IRepository<FormWorkFlow, Guid> formWorkFlowRepository
            )
        {
            _baseflowRepository = baseRep;
            _nodeRepository = nodeRep;
            _linkRepository = lineRep;
            _lineFormRepository = lineFormRep;
            _formRepository = formRepository;
            _formWorkFlowRepository = formWorkFlowRepository;
        }



        public async Task<FormWorkFlow> CreateAsync(string formName, Guid entityId)
        {
            //TODO：逐行验证
            try
            {
                var form = await _formRepository.FirstOrDefaultAsync(_ => _.FormName == formName);
                var flow = await _baseflowRepository.FirstOrDefaultAsync(_ => _.FormId == form.Id);
                var startNode = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == flow.Id && _.Type == "start");
                var nextLine = await _linkRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == flow.Id && _.From == startNode.NodeId);
                //TODO：连线条件判断

                var executeNode = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == flow.Id && _.NodeId == nextLine.To);
                var wf = new FormWorkFlow(GuidGenerator.Create())
                                {
                                    FormId = form.Id,
                                    BaseFlowId = flow.Id,
                                    EntityId = entityId,
                                    Status = WorkFlowStatus.Create,
                                    NodeId = executeNode.Id,
                                };
                return wf;
            }
            catch
            {
                throw new BusinessException("新增失败：工作流启动错误!");
            }
        }

        public async Task<FormWorkFlow> DoWorkFlowAsync(Guid entityId,string user,string[] roles)
        {
            var wf = await _formWorkFlowRepository.GetAsync(_ => _.EntityId == entityId);
            if (wf.Status == WorkFlowStatus.Checked) throw new BusinessException("流程已完成");
            var nodes = await (await _nodeRepository.GetQueryableAsync()).Where(_ => _.BaseFlowId == wf.BaseFlowId).ToListAsync();
            var lines = await (await _linkRepository.GetQueryableAsync()).Where(_ => _.BaseFlowId == wf.BaseFlowId).ToListAsync();
            var executeNode = nodes.FirstOrDefault(_ => _.Id == wf.NodeId);
            if (executeNode == null) throw new BusinessException("节点错误");
            if (executeNode.Executor == "users" && executeNode.Users.IsNullOrWhiteSpace())
            {
                if (executeNode.Users.Split(',').ToList().FirstOrDefault(user) == null)
                {
                    throw new BusinessException("没有执行权限");
                }
            }
            if (executeNode.Executor == "roles" && executeNode.Roles.IsNullOrWhiteSpace())
            {
                if (!executeNode.Roles.Split(',').ToList().Any(i => roles.Contains(i)))
                {
                    throw new BusinessException("没有执行权限");
                }
            }
            var nextLine = lines.FirstOrDefault(_ => _.From == executeNode.NodeId);
            if (nextLine == null) throw new BusinessException("节点错误");
            var nextNode = nodes.FirstOrDefault(_ => _.NodeId == nextLine.To);
            if (nextNode == null) throw new BusinessException("节点错误");

            //TODO：连线条件判断

            if (nextNode.Type == "end") wf.Status = WorkFlowStatus.Checked;
            wf.NodeId = nextNode.Id;
            return wf;
        }
    }
}
