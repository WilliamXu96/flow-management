using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using XCZ.FlowManagement;

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

                b.Property(x => x.FlowId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Title).IsRequired().HasMaxLength(100);
                b.Property(x => x.Code).IsRequired().HasMaxLength(50);
                b.Property(x => x.Remark).HasMaxLength(200);
                b.Property(x => x.Status).HasDefaultValue(0);

                b.HasIndex(x => x.FormId);
            });

            builder.Entity<FlowNode>(b =>
            {
                b.ToTable("base_flow_node");
                b.ConfigureByConvention();

                b.Property(x => x.NodeId).IsRequired().HasMaxLength(100);
                b.Property(x => x.NodeName).HasMaxLength(100);
                b.Property(x => x.Type).IsRequired().HasMaxLength(50);
                b.Property(x => x.Remark).HasMaxLength(200);
            });

            builder.Entity<FlowLink>(b =>
            {
                b.ToTable("base_flow_link");
                b.ConfigureByConvention();

                b.Property(x => x.LinkId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Label).IsRequired().HasMaxLength(100);
                b.Property(x => x.Type).IsRequired().HasMaxLength(50);
                b.Property(x => x.SourceId).IsRequired().HasMaxLength(100);
                b.Property(x => x.TargetId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Remark).HasMaxLength(200);
            });

            builder.Entity<LinkForm>(b =>
            {
                b.ToTable("base_flow_link_form");
                b.ConfigureByConvention();

                b.Property(x => x.FlowLinkId).IsRequired().HasMaxLength(100);
                b.Property(x => x.Content).HasMaxLength(100);
                b.Property(x => x.Remark).HasMaxLength(200);
            });
        }
    }
}
