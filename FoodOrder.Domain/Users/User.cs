using FoodOrder.Domain.Common;
using FoodOrder.Domain.Orders;
using FoodOrder.Domain.Restuarants;
using FoodOrder.Domain.Riders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FoodOrder.Domain.Users
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public User()
        {
            Id = Guid.NewGuid();
            IsActive = true;
            EmailConfirmed = false;
            
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public Gender Gender { get; set; }

        #region Relation
        public List<Order> Orders { get; set; }
        public List<Comment> Comments { get; set; }
        public List<UserRestuarantFavorite> UserRestuarantFavorites { get; set; }
        public List<Restuarant> Restuarants { get; set; }
        public List<Rider> Riders { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.PhoneNumber).HasMaxLength(11).IsRequired();
            builder.Property(p => p.Gender).IsRequired();
            builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(200).IsRequired();
            builder.Property(p => p.PasswordHash).IsRequired();
            builder.Property(p => p.Lat).HasPrecision(9, 6);
            builder.Property(p => p.Long).HasPrecision(8, 6);
            builder.Property(p => p.ConcurrencyStamp).IsConcurrencyToken();
        }
    }

    #endregion
}
