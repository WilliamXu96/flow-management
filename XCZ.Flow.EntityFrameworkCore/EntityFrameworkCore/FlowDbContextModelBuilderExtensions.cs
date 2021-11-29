using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace XCZ.EntityFrameworkCore
{
    public static class FlowDbContextModelBuilderExtensions
    {
        public static void ConfigureFlow([NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
        }
    }
}
