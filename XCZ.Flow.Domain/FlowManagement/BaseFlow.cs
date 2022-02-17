using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace XCZ.FlowManagement
{
    public class BaseFlow : AuditedAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; }

        public Guid FormId { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public string UseDate { get; set; }

        public int Level { get; set; }

        public string Remark { get; set; }

        public int Status { get; set; }

        public bool IsDeleted { get; set; }

        public BaseFlow(Guid id) : base(id)
        {

        }
    }
}
