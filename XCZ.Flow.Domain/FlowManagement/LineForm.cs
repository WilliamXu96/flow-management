﻿using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace XCZ.FlowManagement
{
    public class LineForm : AggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; }

        public Guid BaseFlowId { get; set; }

        public Guid FlowLineId { get; set; }

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

        public bool IsDeleted { get; set; }

        public LineForm(Guid id) : base(id)
        {

        }
    }
}
