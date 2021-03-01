using FoodOrder.Domain.Common;
using FoodOrder.Domain.Menus;
using FoodOrder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FoodOrder.Domain.Restuarants
{
    public class Restuarant : BaseEntity<Guid>
    {
        public Restuarant()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ImgName { get; set; }
        public string Telephone { get; set; }
        public int Rate { get; set; }
        public int AverageDeliverTime { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        #region Relations
        public List<Menu> Menus { get; set; }
        public List<Coupon> Coupons { get; set; }
        public List<OpeningHours> OpeningHours { get; set; }
        public List<Comment> Comments { get; set; }
        public List<UserRestuarantFavorite> UserRestuarantFavorites { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class RestuarantConfiguration : IEntityTypeConfiguration<Restuarant>
    {
        public void Configure(EntityTypeBuilder<Restuarant> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(500).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500).IsRequired();
            builder.Property(p => p.Telephone).HasMaxLength(15).IsRequired();
            builder.Property(p => p.ImgName).HasMaxLength(500).IsRequired();
        }
    }

    #endregion
}
