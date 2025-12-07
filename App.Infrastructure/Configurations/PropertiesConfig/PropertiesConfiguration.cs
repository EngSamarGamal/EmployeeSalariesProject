using App.Infrastructure.Configurations.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Configurations.PropertiesConfig
{
    //public class PropertiesConfiguration : IEntityTypeConfiguration<Properties>
    //{
    //    public void Configure(EntityTypeBuilder<Properties> builder)
    //    {

    //        builder.Property(p => p.NameEn)
    //            .HasMaxLength(ColumnEntityConfiguration.DefaultNameSize);

    //        builder.Property(p => p.NameAr)
    //            .HasMaxLength(ColumnEntityConfiguration.DefaultNameSize);

    //        builder.Property(p => p.AboutEn)
    //            .HasMaxLength(ColumnEntityConfiguration.DefaultDescriptionSize);

    //        builder.Property(p => p.AboutAr)
    //            .HasMaxLength(ColumnEntityConfiguration.DefaultDescriptionSize);

    //        builder.Property(p => p.CoverPhotoUrl)
    //            .HasMaxLength(ColumnEntityConfiguration.DefaultImageSize);

    //        builder.Property(p => p.UnitPrice)
    //            .HasColumnType($"decimal({FinancialEntityConfiguration.MoneyPrecision},{FinancialEntityConfiguration.MoneyScale})");

    //        builder.Property(p => p.MaintenanceFees)
    //            .HasColumnType($"decimal({FinancialEntityConfiguration.MoneyPrecision},{FinancialEntityConfiguration.MoneyScale})");

    //        builder.Property(p => p.DownPaymentPercent)
    //            .HasColumnType($"decimal({FinancialEntityConfiguration.PercentagePrecision},{FinancialEntityConfiguration.PercentageScale})");

    //        builder.Property(p => p.EstimatedROI)
    //            .HasColumnType($"decimal({FinancialEntityConfiguration.PercentagePrecision},{FinancialEntityConfiguration.PercentageScale})");

    //        builder.Property(p => p.GrowthExitROI)
    //            .HasColumnType($"decimal({FinancialEntityConfiguration.PercentagePrecision},{FinancialEntityConfiguration.PercentageScale})");

    //        builder.Property(p => p.RentalExitROI)
    //            .HasColumnType($"decimal({FinancialEntityConfiguration.PercentagePrecision},{FinancialEntityConfiguration.PercentageScale})");

    //        builder.Property(p => p.ExpectedRentalDate)
    //            .HasColumnType("datetime2");

    //        builder.Property(p => p.ExitDate)
    //            .HasColumnType("datetime2");

    //        builder.Property(p => p.SharePrice)
    //            .HasColumnType($"decimal({FinancialEntityConfiguration.MoneyPrecision},{FinancialEntityConfiguration.MoneyScale})");

    //        builder.Property(p => p.IsDeleted).HasDefaultValue(ColumnEntityConfiguration.DefaultIsDeleted);

    //    }
    //}
}
