using FoodOrder.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FoodOrder.Domain.Menus
{
    public class MenuGroup : BaseEntity<Guid>
    {
        public MenuGroup()
        {
            Id = Guid.NewGuid();
        }
        public string Name { get; set; }
        public string ImgName { get; set; }

        public Guid? MenuId { get; set; }

        #region Relations
        public Menu Menu { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        #endregion
    }

    #region EntityConfiguration
    public class MenuGroupConfiguration : IEntityTypeConfiguration<MenuGroup>
    {
        public void Configure(EntityTypeBuilder<MenuGroup> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.ImgName).HasMaxLength(500).IsRequired();
            builder.HasOne(p => p.Menu).WithMany(p => p.MenuGroups).HasForeignKey(p => p.MenuId);
        }
    }
    #endregion
}
