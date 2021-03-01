using FoodOrder.Domain.Common;
using FoodOrder.Domain.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FoodOrder.Domain.Orders
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem()
        {
            Id = Guid.NewGuid();
            Quantity = 1;
        }
        public OrderStatus OrderStatus { get; set; }
        public int Quantity { get; set; }

        public Guid? OrderId { get; set; }
        public Guid? MenuItemId { get; set; }

        #region Relations
        public MenuItem MenuItem { get; set; }
        public Order Order { get; set; }
        #endregion
    }
    #region EntityConfiguration

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(p => p.MenuItem).WithMany(p => p.OrderItems).HasForeignKey(p => p.MenuItemId);
            builder.HasOne(p => p.Order).WithMany(p => p.OrderItems).HasForeignKey(p => p.OrderId);
        }
    }

    #endregion
}
