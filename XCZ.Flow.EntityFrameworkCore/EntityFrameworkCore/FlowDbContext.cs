﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using XCZ.FlowManagement;

namespace XCZ.EntityFrameworkCore
{
    [ConnectionStringName("Business")]
    public class FlowDbContext : AbpDbContext<FlowDbContext>
    {
        public DbSet<BaseFlow> BaseFlows { get; set; }

        public DbSet<FlowLine> FlowLines { get; set; }

        public DbSet<FlowNode> FlowNodes { get; set; }

        public DbSet<LineForm> LinkForms { get; set; }

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
