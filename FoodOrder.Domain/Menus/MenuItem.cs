using FoodOrder.Domain.Common;
using FoodOrder.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrder.Domain.Menus
{
    public class MenuItem:BaseEntity<Guid>
    {
        public MenuItem()
        {
            Id = Guid.NewGuid();
        }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImgName { get; set; }

        public Guid? MenuGroupId { get; set; }

        #region Relations
        public MenuGroup MenuGroup { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500).IsRequired();
            builder.Property(p => p.ImgName).HasMaxLength(500).IsRequired();
            builder.Property(p=>p.Price).HasColumnType("decimal(18,4)");

            builder.HasOne(p => p.MenuGroup).WithMany(p => p.MenuItems).HasForeignKey(p => p.MenuGroupId);
        }
    }

    #endregion
}
