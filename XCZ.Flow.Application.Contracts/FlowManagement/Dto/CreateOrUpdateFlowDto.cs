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

        public List<CreateOrUpdateFlowLinkDto> LinkList { get; set; }
    }

    public class CreateOrUpdateFlowLinkDto
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
        public string SourceId { get; set; }

        /// <summary>
        /// 连线终点
        /// </summary>
        public string TargetId { get; set; }

        public string Remark { get; set; }

        public List<CreateOrUpdateLinkFormDto> TempFieldForm { get; set; }
    }

    public class CreateOrUpdateLinkFormDto
    {
        public string LinkId { get; set; }

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

        public string NodeName { get; set; }

        public string Type { get; set; }

        public int Height { get; set; }

        public int X { get; set; }

        public int Width { get; set; }

        public int Y { get; set; }

        public string Remark { get; set; }
    }
}
