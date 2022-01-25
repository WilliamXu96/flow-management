using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace XCZ.FlowManagement
{
    /// <summary>
    /// 流程信息
    /// </summary>
    public class BaseFlow : AuditedAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; }

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

        /// <summary>
        /// 状态：0未启用、1启用
        /// </summary>
        public int Status { get; set; }

        public bool IsDeleted { get; set; }

        public BaseFlow(Guid id) : base(id)
        {

        }
    }
}
