using System;
using XCZ.Enums;

namespace XCZ.Dtos
{
    public class FormWorkFlowNodeStatusDto
    {
        public Guid EntityId { get; set; }

        public WorkFlowStatus Status { get; set; }

        public string NodeId { get; set; }

        public string NodeName { get; set; }

        //审核人、审核日期
    }
}
