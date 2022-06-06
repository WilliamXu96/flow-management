using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using XCZ.Enums;
using XCZ.FlowManagement;
using XCZ.FormManagement;

namespace XCZ.WorkFlow
{
    public class FormWorkFlowManager : DomainService
    {
        private readonly IRepository<BaseFlow, Guid> _baseflowRepository;
        private readonly IRepository<FlowNode, Guid> _nodeRepository;
        private readonly IRepository<FlowLine, Guid> _lineRepository;
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
            _lineRepository = lineRep;
            _lineFormRepository = lineFormRep;
            _formRepository = formRepository;
            _formWorkFlowRepository = formWorkFlowRepository;
        }

        public async Task<FormWorkFlow> CreateAsync(string formName, object obj)
        {
            //TODO：性能优化
            var id1 = await (await _formRepository.GetQueryableAsync()).Where(_ => _.FormName == formName).Select(s => s.Id).FirstOrDefaultAsync();
            var id2 = await (await _baseflowRepository.GetQueryableAsync()).Where(_ => _.FormId == id1).Select(s => s.Id).FirstOrDefaultAsync();
            var sn = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == id2 && _.Type == "start");
            if (sn == null) throw new BusinessException("新增失败：找不到流程信息！");
            var nls = await _lineRepository.GetListAsync(_ => _.BaseFlowId == id2 && _.From == sn.NodeId);
            var lfs = await _lineFormRepository.GetListAsync(_ => _.BaseFlowId == id2 && nls.Select(s => s.Id).Contains(_.FlowLineId));
            string nid = null;
            try
            {
                var lin = nls.First();
                var f = lfs.FirstOrDefault(_ => _.FlowLineId == lin.Id);
                if (nls.Count == 1 && f == null) nid = lin.To;
                if (f != null && f.FieldType == "int")
                {
                    if (f.Condition == ">")
                    {
                        var com = GreaterCompare(Convert.ToInt32(obj.GetType().GetProperty(f.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(obj).ToString()), lfs.Where(_ => _.FieldName == f.FieldName && _.FieldType == "int" && _.Condition == f.Condition).OrderBy(o => o.IntContent).Select(s => s.IntContent).ToList());
                        nid = nls.FirstOrDefault(l => l.Id == lfs.FirstOrDefault(f => f.IntContent == com)?.FlowLineId)?.To;
                    }
                    if (f.Condition == "<")
                    {
                        var com = LessCompare(Convert.ToInt32(obj.GetType().GetProperty(f.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(obj).ToString()), lfs.Where(_ => _.FieldName == f.FieldName && _.FieldType == "int" && _.Condition == f.Condition).OrderByDescending(o => o.IntContent).Select(s => s.IntContent).ToList());
                        nid = nls.FirstOrDefault(l => l.Id == lfs.FirstOrDefault(f => f.IntContent == com)?.FlowLineId)?.To;
                    }
                    if (f.Condition == "=")
                    {
                        foreach (var ff in lfs.Where(_ => _.FieldName == f.FieldName && _.FieldType == "int" && _.Condition == f.Condition).ToList())
                        {
                            if (Convert.ToInt32(obj.GetType().GetProperty(f.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(obj).ToString()) == ff.IntContent) nid = nls.FirstOrDefault(l => l.Id == ff.FlowLineId)?.To;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("新增失败：工作流启动异常！");
            }
            if (nid.IsNullOrEmpty()) throw new BusinessException("新增失败：找不到符合条件的流程节点！");
            var exe = await _nodeRepository.FirstOrDefaultAsync(_ => _.BaseFlowId == id2 && _.NodeId == nid);
            var wf = new FormWorkFlow(GuidGenerator.Create()) { FormId = id1, BaseFlowId = id2, EntityId = Guid.Parse(obj.GetType().GetProperty("id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(obj).ToString()), Status = WorkFlowStatus.Create, NodeId = exe.NodeId };
            return wf;
        }

        public async Task CheckFormStatusAsync(Guid entityId)
        {
            var wf = await _formWorkFlowRepository.FirstOrDefaultAsync(_ => _.EntityId == entityId);
            if (wf == null) return;
            if(wf.Status==WorkFlowStatus.Create) throw new BusinessException("修改失败：流程审核中！");
            if (wf.Status == WorkFlowStatus.Checked) throw new BusinessException("修改失败：流程已审核！");
        }

        public async Task<FormWorkFlow> DoWorkFlowAsync(Guid entityId, string jsonStr, string user, string[] roles)
        {
            var wf = await _formWorkFlowRepository.GetAsync(_ => _.EntityId == entityId);
            if (wf.Status == WorkFlowStatus.Checked) throw new BusinessException("审核失败：流程已完成！");
            var ns = await _nodeRepository.GetListAsync(_ => _.BaseFlowId == wf.BaseFlowId);
            if (!ns.Any()) throw new BusinessException("新增失败：找不到流程信息！");
            var exe = ns.FirstOrDefault(_ => _.NodeId == wf.NodeId);
            if (exe == null) throw new BusinessException("节点错误");
            if (exe.Executor == "users" && !exe.Users.IsNullOrWhiteSpace())
                if (!exe.Users.Split(',').ToList().Any(u => u == user)) throw new BusinessException("审核失败：没有执行权限！");
            if (exe.Executor == "roles" && !exe.Roles.IsNullOrWhiteSpace())
                if (!exe.Roles.Split(',').ToList().Any(i => roles.Contains(i))) throw new BusinessException("审核失败：没有执行权限！");
            var nls = await _lineRepository.GetListAsync(_ => _.BaseFlowId == wf.BaseFlowId && _.From == exe.NodeId);
            var lfs = await _lineFormRepository.GetListAsync(_ => _.BaseFlowId == wf.BaseFlowId && nls.Select(s => s.Id).Contains(_.FlowLineId));
            string nid = null;
            try
            {
                var lin = nls.First();
                var f = lfs.FirstOrDefault(_ => _.FlowLineId == lin.Id);
                if (nls.Count == 1 && f == null)
                {
                    nid = lin.To;
                }
                if (f != null && f.FieldType == "int")
                {
                    if (f.Condition == ">")
                    {
                        var com = GreaterCompare(GetPropertyIntValue(f.FieldName, jsonStr), lfs.Where(_ => _.FieldName == f.FieldName && _.FieldType == "int" && _.Condition == f.Condition).OrderBy(o => o.IntContent).Select(s => s.IntContent).ToList());
                        nid = nls.FirstOrDefault(l => l.Id == lfs.FirstOrDefault(f => f.IntContent == com)?.FlowLineId)?.To;
                    }
                    if (f.Condition == "<")
                    {
                        var com = LessCompare(GetPropertyIntValue(f.FieldName, jsonStr), lfs.Where(_ => _.FieldName == f.FieldName && _.FieldType == "int" && _.Condition == f.Condition).OrderByDescending(o => o.IntContent).Select(s => s.IntContent).ToList());
                        nid = nls.FirstOrDefault(l => l.Id == lfs.FirstOrDefault(f => f.IntContent == com)?.FlowLineId)?.To;
                    }
                    if (f.Condition == "=")
                    {
                        foreach (var ff in lfs.Where(_ => _.FieldName == f.FieldName && _.FieldType == "int" && _.Condition == f.Condition).ToList())
                        {
                            if (GetPropertyIntValue(f.FieldName, jsonStr) == ff.IntContent) nid = nls.FirstOrDefault(l => l.Id == ff.FlowLineId)?.To;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("执行失败：工作流异常！");
            }
            if (nid.IsNullOrEmpty()) throw new BusinessException("执行失败：找不到符合条件的流程节点！");
            var nn = ns.FirstOrDefault(_ => _.NodeId == nid);
            if (nn == null) throw new BusinessException("执行失败：找不到下游流程节点！");
            if (nn.Type == "end") wf.Status = WorkFlowStatus.Checked;
            wf.NodeId = nid;
            return wf;
        }

        private int GreaterCompare(int val, List<int> values)
        {
            var com = -987654321;
            foreach (var v in values) { if (val > v) com = v; }
            return com;
        }

        private int LessCompare(int val, List<int> values)
        {
            var com = -987654321;
            foreach (var v in values) { if (val < v) com = v; }
            return com;
        }

        private int GetPropertyIntValue(string name, string jsonStr)
        {
            var jd = JsonDocument.Parse(jsonStr).RootElement;
            foreach (var item in jd.EnumerateObject())
            {
                if (item.Name.ToLower() == name.ToLower())
                    return item.Value.GetInt32();
            }
            throw new Exception();
        }
    }
}
