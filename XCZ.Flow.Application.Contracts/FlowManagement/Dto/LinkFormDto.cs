using System;

namespace XCZ.FlowManagement.Dto
{
    public class LinkFormDto
    {
        public string Pid { get; set; }

        /// <summary>
        /// 表单
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        public Guid Condition { get; set; }

        public string Content { get; set; }

        public string Remark { get; set; }
    }
}
