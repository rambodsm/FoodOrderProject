using FoodOrder.Domain.Common;
using FoodOrder.Domain.Restuarants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FoodOrder.Domain.Menus
{
    public class Menu : BaseEntity<Guid>
    {
        public Menu()
        {
            Id = Guid.NewGuid();
        }
        public string FoodTypeName { get; set; }
        public Guid? ResurarantId { get; set; }

        #region Relations
        public Restuarant Restuarant { get; set; }
        public List<MenuGroup> MenuGroups { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.Property(p => p.FoodTypeName).HasMaxLength(200).IsRequired();
            builder.HasOne(p => p.Restuarant).WithMany(p => p.Menus).HasForeignKey(p => p.ResurarantId);

        }
    }

    #endregion
}
