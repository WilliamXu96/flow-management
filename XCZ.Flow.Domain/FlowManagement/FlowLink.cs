using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace XCZ.FlowManagement
{
    /// <summary>
    /// 流程连线
    /// </summary>
    public class FlowLink : AggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; }

        public Guid BaseFlowId { get; set; }

        /// <summary>
        /// 连线Id
        /// </summary>
        public string LinkId { get; set; }

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

        public bool IsDeleted { get; set; }

        public FlowLink(Guid id) : base(id)
        {

        }
    }
}
