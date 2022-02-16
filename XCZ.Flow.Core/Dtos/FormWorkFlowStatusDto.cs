using System;
using XCZ.Enums;

namespace XCZ.Dtos
{
    public class FormWorkFlowStatusDto
    {
        public Guid EntityId { get; set; }

        public WorkFlowStatus Status { get; set; }
    }
}
