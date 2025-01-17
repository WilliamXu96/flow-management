﻿using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace XCZ.FlowManagement
{
    public class FlowLine : AggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; }

        public Guid BaseFlowId { get; set; }

        public string Label { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Remark { get; set; }

        public bool IsDeleted { get; set; }

        public FlowLine(Guid id) : base(id)
        {

        }
    }
}
