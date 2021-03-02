using FoodOrder.Domain.Common;
using FoodOrder.Domain.Payments;
using FoodOrder.Domain.Riders;
using FoodOrder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FoodOrder.Domain.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }

        public Guid? PaymentId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? RiderId { get; set; }

        #region Relations
        public User User { get; set; }
        public Payment Payment { get; set; }
        public Rider Rider { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(p => p.Description).HasMaxLength(500).IsRequired();

            builder.HasOne(p => p.User).WithMany(p => p.Orders).HasForeignKey(p => p.CustomerId);
            builder.HasOne(p => p.Payment).WithMany(p => p.Orders).HasForeignKey(p => p.PaymentId);
            builder.HasOne(p => p.Rider).WithMany(p => p.Orders).HasForeignKey(p => p.RiderId);
        }
    }

    #endregion
}
