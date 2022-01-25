using System;
using System.Collections.Generic;

namespace XCZ.FlowManagement.Dto
{
    public class CreateOrUpdateFlowDto
    {
        /// <summary>
        /// 流程Id
        /// </summary>
        public string FlowId { get; set; }

        public Guid FormId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        public string UseDate { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        public string Remark { get; set; }

        public List<CreateOrUpdateFlowNodeDto> NodeList { get; set; }

        public List<CreateOrUpdateFlowLineDto> LineList { get; set; }
    }

    public class CreateOrUpdateFlowLineDto
    {
        /// <summary>
        /// 连线Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Label { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// 连线起点
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 连线终点
        /// </summary>
        public string To { get; set; }

        public string Remark { get; set; }

        public List<CreateOrUpdateLineFormDto> FormField { get; set; }
    }

    public class CreateOrUpdateLineFormDto
    {
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

    public class CreateOrUpdateFlowNodeDto
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

        public string Users { get; set; }

        public string Roles { get; set; }

        public string Remark { get; set; }
    }
}
