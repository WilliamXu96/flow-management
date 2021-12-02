using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using XCZ.FlowManagement;

namespace XCZ.EntityFrameworkCore
{
    [ConnectionStringName("Business")]
    public class FlowDbContext : AbpDbContext<FlowDbContext>
    {
        public DbSet<BaseFlow> BaseFlows;

        public DbSet<FlowLink> FlowLinks;

        public DbSet<FlowNode> FlowNodes;

        public DbSet<LinkForm> LinkForms;

        public FlowDbContext(DbContextOptions<FlowDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFlow();
        }
    }
}
