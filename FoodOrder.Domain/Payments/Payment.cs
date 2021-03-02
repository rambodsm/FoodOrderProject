using FoodOrder.Domain.Common;
using FoodOrder.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FoodOrder.Domain.Payments
{
    public class Payment : BaseEntity<Guid>
    {
        public Payment()
        {
            Id = Guid.NewGuid();
        }
        public string PaymentTypeName { get; set; }

        #region Relations
        public List<Order> Orders { get; set; }
        #endregion
    }

    #region EntityConfiguration
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.PaymentTypeName).HasMaxLength(100).IsRequired();
        }
    }
    #endregion
}
