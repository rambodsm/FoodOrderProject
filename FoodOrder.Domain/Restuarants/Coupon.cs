using FoodOrder.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FoodOrder.Domain.Restuarants
{
    public class Coupon : BaseEntity<Guid>
    {
        public Coupon()
        {
            Id = Guid.NewGuid();
            MaximumUse = 1;
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public int MaximumUse { get; set; }

        public Guid? RestuarantId { get; set; }

        #region Relations
        public Restuarant Restuarant { get; set; }
        #endregion
    }
    #region EntityConfiguration

    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(500).IsRequired();

            builder.HasOne(p => p.Restuarant).WithMany(p => p.Coupons).HasForeignKey(p => p.RestuarantId);
        }
    }

    #endregion
}
