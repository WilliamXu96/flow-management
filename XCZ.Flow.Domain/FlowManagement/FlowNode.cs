using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace XCZ.FlowManagement
{
    /// <summary>
    /// 流程节点
    /// </summary>
    public class FlowNode : AggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 节点Id
        /// </summary>
        public string NodeId { get; set; }

        public string NodeName { get; set; }

        public string Type { get; set; }

        public int Height { get; set; }

        public int X { get; set; }

        public int Width { get; set; }

        public int Y { get; set; }

        public string Remark { get; set; }

        public bool IsDeleted { get; set; }
    }
}
