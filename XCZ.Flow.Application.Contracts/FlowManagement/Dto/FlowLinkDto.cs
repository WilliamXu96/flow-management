﻿using System;
using System.Collections.Generic;

namespace XCZ.FlowManagement.Dto
{
    public class FlowLinkDto
    {
        /// <summary>
        /// 连线Id
        /// </summary>
        public string Id { get; set; }

        public Guid FlowLinkId { get; set; }

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

        public List<LinkFormDto> TempFieldForm { get; set; }
    }
}