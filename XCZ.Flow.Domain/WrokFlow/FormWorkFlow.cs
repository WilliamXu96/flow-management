using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using XCZ.Enums;

namespace XCZ.WrokFlow
{
    public class FormWorkFlow : AuditedAggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public bool IsDeleted { get; set; }
        public Guid FormId { get; set; }

        public Guid BaseFlowId { get; set; }

        public Guid EntityId { get; set; }
        public WorkFlowStatus Status { get; set; }

        public Guid? NodeId { get; set; }

        public FormWorkFlow(Guid id) : base(id)
        {

        }
    }
}
