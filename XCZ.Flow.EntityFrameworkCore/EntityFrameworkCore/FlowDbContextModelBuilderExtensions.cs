using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using XCZ.Enums;
using XCZ.FlowManagement;
using XCZ.WrokFlow;

namespace XCZ.EntityFrameworkCore
{
    public static class FlowDbContextModelBuilderExtensions
    {
        public static void ConfigureFlow([NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<BaseFlow>(b =>
            {
                b.ToTable("base_flow");
                b.ConfigureByConvention();

                //b.Property(x => x.FlowId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Title).IsRequired().HasMaxLength(100);
                b.Property(x => x.Code).IsRequired().HasMaxLength(50);
                b.Property(x => x.UseDate).IsRequired().HasMaxLength(50);
                b.Property(x => x.Remark).HasMaxLength(200);
                b.Property(x => x.Status).HasDefaultValue(0);

                b.HasIndex(x => x.FormId);
            });

            builder.Entity<FlowNode>(b =>
            {
                b.ToTable("base_flow_node");
                b.ConfigureByConvention();

                b.Property(x => x.NodeId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.Type).IsRequired().HasMaxLength(50);
                b.Property(x => x.Left).IsRequired().HasMaxLength(50);
                b.Property(x => x.Top).IsRequired().HasMaxLength(50);
                b.Property(x => x.Ico).HasMaxLength(50);
                b.Property(x => x.State).HasMaxLength(50);
                b.Property(x => x.Executor).HasMaxLength(50);
                b.Property(x => x.Users).HasMaxLength(1000);
                b.Property(x => x.Roles).HasMaxLength(1000);
                b.Property(x => x.Remark).HasMaxLength(200);
            });

            builder.Entity<FlowLine>(b =>
            {
                b.ToTable("base_flow_line");
                b.ConfigureByConvention();

                //b.Property(x => x.LineId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Label).HasMaxLength(100);
                b.Property(x => x.From).IsRequired().HasMaxLength(50);
                b.Property(x => x.To).IsRequired().HasMaxLength(50);
                //b.Property(x => x.TargetId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Remark).HasMaxLength(200);
            });

            builder.Entity<LineForm>(b =>
            {
                b.ToTable("base_flow_line_form");
                b.ConfigureByConvention();

                //b.Property(x => x.Pid).IsRequired().HasMaxLength(100);
                b.Property(x => x.Content).HasMaxLength(1000);
                b.Property(x => x.Remark).HasMaxLength(200);
            });

            builder.Entity<FormWorkFlow>(b =>
            {
                b.ToTable("base_form_workflow");
                b.ConfigureByConvention();

                b.Property(x => x.Status).HasDefaultValue(WorkFlowStatus.Create);
                b.HasIndex(x => x.EntityId);
            });
        }
    }
}
