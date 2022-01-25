using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace XCZ.FlowManagement.Dto
{
    public class FlowDto : EntityDto<Guid>
    {
        /// <summary>
        /// 流程Id
        /// </summary>
        //public string FlowId { get; set; }

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

        public List<FlowNodeDto> NodeList { get; set; }

        public List<FlowLinkDto> LineList { get; set; } 
    }
}
