using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
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
            try
            {
                var form = await _formRepository.FirstOrDefaultAsync(_ => _.FormName == formName);
                var flow = await _baseflowRepository.FirstOrDefaultAsync(_ => _.FormId == form.Id);
                var snode = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == flow.Id && _.Type == "start");
                var nline = await _linkRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == flow.Id && _.From == snode.NodeId);
                //TODO：连线条件判断

                var exe = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == flow.Id && _.NodeId == nline.To);
                var wf = new FormWorkFlow(GuidGenerator.Create()) { FormId = form.Id, BaseFlowId = flow.Id, EntityId = entityId, Status = WorkFlowStatus.Create, NodeId = exe.NodeId };
                return wf;
            }
            catch
            {
                throw new BusinessException("新增失败：工作流启动异常!");
            }
        }

        public async Task<FormWorkFlow> CreateAsync(string formName, object obj)
        {
            try
            {
                var id1 = await (await _formRepository.GetQueryableAsync()).Where(_ => _.FormName == formName).Select(s => s.Id).FirstOrDefaultAsync();
                var id2 = await (await _baseflowRepository.GetQueryableAsync()).Where(_ => _.FormId == id1).Select(s => s.Id).FirstOrDefaultAsync();

                var snode = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == id2 && _.Type == "start");
                var nline = await _linkRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == id2 && _.From == snode.NodeId);
                //TODO：连线条件判断

                var exe = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == id2 && _.NodeId == nline.To);
                var wf = new FormWorkFlow(GuidGenerator.Create()) { FormId = id1, BaseFlowId = id2, EntityId = Guid.Parse(obj.GetType().GetProperty("id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(obj).ToString()), Status = WorkFlowStatus.Create, NodeId = exe.NodeId };
                return wf;
            }
            catch (Exception ex)
            {
                throw new BusinessException("新增失败：工作流启动异常!");
            }
        }

        public async Task<FormWorkFlow> DoWorkFlowAsync(Guid entityId, string user, string[] roles)
        {
            var wf = await _formWorkFlowRepository.GetAsync(_ => _.EntityId == entityId);
            if (wf.Status == WorkFlowStatus.Checked) throw new BusinessException("审核失败：流程已完成");
            var ns = await (await _nodeRepository.GetQueryableAsync()).Where(_ => _.BaseFlowId == wf.BaseFlowId).ToListAsync();
            var ls = await (await _linkRepository.GetQueryableAsync()).Where(_ => _.BaseFlowId == wf.BaseFlowId).ToListAsync();
            var exe = ns.FirstOrDefault(_ => _.NodeId == wf.NodeId);
            if (exe == null) throw new BusinessException("节点错误");
            if (exe.Executor == "users" && !exe.Users.IsNullOrWhiteSpace())
                if (!exe.Users.Split(',').ToList().Any(u => u == user)) throw new BusinessException("审核失败：没有执行权限");
            if (exe.Executor == "roles" && !exe.Roles.IsNullOrWhiteSpace())
                if (!exe.Roles.Split(',').ToList().Any(i => roles.Contains(i))) throw new BusinessException("审核失败：没有执行权限");
            var nl = ls.FirstOrDefault(_ => _.From == exe.NodeId);
            if (nl == null) throw new BusinessException("节点错误");
            var nn = ns.FirstOrDefault(_ => _.NodeId == nl.To);
            if (nn == null) throw new BusinessException("节点错误");

            //TODO：连线条件判断

            if (nn.Type == "end") wf.Status = WorkFlowStatus.Checked;
            wf.NodeId = nn.NodeId;
            return wf;
        }
    }
}
