using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace XCZ.EntityFrameworkCore
{
    [ConnectionStringName("Business")]
    public class FlowDbContext : AbpDbContext<FlowDbContext>
    {

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
