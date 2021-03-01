using FoodOrder.Domain.Common;
using FoodOrder.Domain.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Domain.Users
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public User()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public Gender Gender { get; set; }

        #region Relation
        public List<Order> Orders { get; set; }
        public List<Comment> Comments { get; set; }
        public List<UserRestuarantFavorite> UserRestuarantFavorites { get; set; }
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
            builder.Property(p => p.PasswordHash).IsRequired();
        }
    }

    #endregion
}
