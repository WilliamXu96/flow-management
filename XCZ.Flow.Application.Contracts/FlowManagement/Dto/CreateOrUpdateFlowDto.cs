using System;
using System.Collections.Generic;

namespace XCZ.FlowManagement.Dto
{
    public class CreateOrUpdateFlowDto
    {
        public Guid FormId { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public string UseDate { get; set; }

        public int Level { get; set; }

        public string Remark { get; set; }

        public List<CreateOrUpdateFlowNodeDto> NodeList { get; set; }

        public List<CreateOrUpdateFlowLineDto> LineList { get; set; }
    }

    public class CreateOrUpdateFlowLineDto
    {
        public string Label { get; set; }

        public string Type { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Remark { get; set; }

        public List<CreateOrUpdateLineFormDto> FormField { get; set; }
    }

    public class CreateOrUpdateLineFormDto
    {
        public Guid FieldId { get; set; }

        public string Condition { get; set; }

        public string Content { get; set; }

        public string Remark { get; set; }
    }

    public class CreateOrUpdateFlowNodeDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Left { get; set; }

        public string Top { get; set; }

        public string Ico { get; set; }

        public string State { get; set; }

        public string Executor { get; set; }

        public List<string> Users { get; set; }

        public List<string> Roles { get; set; }

        public string Remark { get; set; }
    }
}
