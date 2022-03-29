using System;
using System.Collections.Generic;
using XCZ.FlowManagement;

namespace XCZ.DataSeeder
{
    public class FlowSeeder
    {
        public BaseFlow Flow { get; set; }

        public List<FlowNode> Nodes { get; set; }

        public List<FlowLine> Lines { get; set; }

        public List<LineForm> LineForms { get; set; }

        public FlowSeeder(Guid formId, Guid fieldId)
        {
            NewBaseFlow(formId);
            NewFlowNodes(Flow.Id);
            NewFlowLines(Flow.Id, fieldId);
        }

        private void NewBaseFlow(Guid formId)
        {
            Flow = new BaseFlow(Guid.NewGuid()) { FormId = formId, Title = "书籍审核", Code = "BookCheck", UseDate = DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd"), Level = 5, Remark = "图书上市审核", Status = 0 };
        }

        private void NewFlowNodes(Guid flowId)
        {
            Nodes = new List<FlowNode>
            {
                new FlowNode(Guid.NewGuid()){ BaseFlowId=flowId, NodeId="node-0f1pka7rmp", Name="流程开始",Type="start", Left="75px", Top="35px", Ico="el-icon-time", State="success" },
                new FlowNode(Guid.NewGuid()){ BaseFlowId=flowId, NodeId="node-dsh9dke9th", Name="管理员审核", Type="task", Left="128px", Top="270px",Ico="el-icon-odometer",State="success", Executor="users",Users="admin" },
                new FlowNode(Guid.NewGuid()){ BaseFlowId=flowId, NodeId="node-pnphhz952", Name="老板审批", Type="task", Left="438.333px", Top="234.677px", Ico="el-icon-odometer", State="success", Executor="users", Users="boss" },
                new FlowNode(Guid.NewGuid()){ BaseFlowId=flowId, NodeId="node-g7teux3r0c", Name="流程结束", Type="end", Left="180px", Top="424px", Ico="el-icon-switch-button", State="success"}
            };
        }

        private void NewFlowLines(Guid flowId, Guid fieldId)
        {
            var lines = new List<FlowLine>();
            var lineForms = new List<LineForm>();
            lines.Add(new FlowLine(Guid.NewGuid()) { BaseFlowId = flowId, From = "node-pnphhz952", To = "node-g7teux3r0c" });
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            lines.Add(new FlowLine(id1) { BaseFlowId = flowId, Label = "价格大于500", From = "node-0f1pka7rmp", To = "node-pnphhz952" });
            lines.Add(new FlowLine(id2) { BaseFlowId = flowId, Label = "价格大于100", From = "node-0f1pka7rmp", To = "node-dsh9dke9th" });
            lines.Add(new FlowLine(Guid.NewGuid()) { BaseFlowId = flowId, From = "node-dsh9dke9th", To = "node-g7teux3r0c" });
            lineForms.Add(new LineForm(Guid.NewGuid()) { BaseFlowId = flowId, FlowLineId = id1, FieldId = fieldId, FieldName = "price", FieldType = "int", Condition = ">", IntContent = 500, Content = "500", });
            lineForms.Add(new LineForm(Guid.NewGuid()) { BaseFlowId = flowId, FlowLineId = id2, FieldId = fieldId, FieldName = "price", FieldType = "int", Condition = ">", IntContent = 100, Content = "100", });
            Lines = lines;
            LineForms = lineForms;
        }
    }
}
