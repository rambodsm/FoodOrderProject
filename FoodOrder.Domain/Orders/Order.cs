using FoodOrder.Domain.Common;
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

        public Guid? CustomerId { get; set; }

        #region Relations
        public User User { get; set; }
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
        }
    }

    #endregion
}
