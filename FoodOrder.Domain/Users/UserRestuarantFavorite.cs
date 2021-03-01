using FoodOrder.Domain.Common;
using FoodOrder.Domain.Restuarants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FoodOrder.Domain.Users
{
    public class UserRestuarantFavorite : BaseEntity<Guid>
    {
        public UserRestuarantFavorite()
        {
            Id = Guid.NewGuid();
        }
        public Guid? UserId { get; set; }
        public Guid? RestuarantId { get; set; }

        #region Relations
        public User User { get; set; }
        public Restuarant Restuarant { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class MenuItemConfiguration : IEntityTypeConfiguration<UserRestuarantFavorite>
    {
        public void Configure(EntityTypeBuilder<UserRestuarantFavorite> builder)
        {
            builder.HasOne(p => p.User).WithMany(p => p.UserRestuarantFavorites).HasForeignKey(p => p.UserId);
            builder.HasOne(p => p.Restuarant).WithMany(p => p.UserRestuarantFavorites).HasForeignKey(p => p.RestuarantId);
        }
    }

    #endregion
}
