using System;
using System.Collections.Generic;

namespace XCZ.FlowManagement.Dto
{
    public class FlowLinkDto
    {
        public Guid Id { get; set; }

        public string Label { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Remark { get; set; }

        public List<LineFormDto> FormField { get; set; }
    }
}
