using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Infrastructure.Configurations.Global
{
    public static class GlobalEntitySettings
    {
        public static void ApplyGlobalQueryFilters(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                var isDeletedProp = clrType.GetProperty("IsDeleted");

                if (isDeletedProp != null && isDeletedProp.PropertyType == typeof(bool))
                {
                    var parameter = Expression.Parameter(clrType, "e");
                    var property = Expression.Property(parameter, isDeletedProp);
                    var compare = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(compare, parameter);
                    builder.Entity(clrType).HasQueryFilter(lambda);

                }
            }
        }
    }
}
