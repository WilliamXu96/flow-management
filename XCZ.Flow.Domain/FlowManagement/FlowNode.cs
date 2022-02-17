using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace XCZ.FlowManagement
{
    public class FlowNode : AggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; }

        public Guid BaseFlowId { get; set; }

        public string NodeId { get; set; }

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

        public bool IsDeleted { get; set; }

        public FlowNode(Guid id) : base(id)
        {

        }
    }
}
