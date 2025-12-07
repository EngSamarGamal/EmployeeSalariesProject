using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace App.Infrastructure.Configurations.PropertiesConfig
//{
//    public class PropertyDownPaymentsConfiguration : IEntityTypeConfiguration<PropertyDownPayments>
//    {
//        public void Configure(EntityTypeBuilder<PropertyDownPayments> builder)
//        {

//            builder.HasKey(x => x.Id);

//            builder.Property(x => x.Amount)
//                   .HasColumnType("decimal(18,2)")
//                   .IsRequired();

//            builder.Property(x => x.DueDate)
//                   .HasColumnType("datetime2")
//                   .IsRequired();

//            builder.HasOne(x => x.Property)
//                   .WithMany(p => p.DownPayments)
//                   .HasForeignKey(x => x.PropertyId)
//                   .OnDelete(DeleteBehavior.Cascade);
//        }
//    }
//}
