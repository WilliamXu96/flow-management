using System.Collections.Generic;

namespace XCZ.FlowManagement.Dto
{
    public class FlowNodeDto
    {
        /// <summary>
        /// 节点Id
        /// </summary>
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
