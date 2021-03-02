using FoodOrder.Domain.Common;
using FoodOrder.Domain.Orders;
using FoodOrder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FoodOrder.Domain.Riders
{
    public class Rider : BaseEntity<Guid>
    {
        public Rider()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }
        public string LicenceNumber { get; set; }
        public int Rate { get; set; }
        public DateTime CreateDate { get; set; }

        public Guid? VehicleId { get; set; }
        public Guid? UserId { get; set; }
        #region Relations
        public Vehicle Vehicle { get; set; }
        public User User { get; set; }
        public List<Order> Orders { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class UserConfiguration : IEntityTypeConfiguration<Rider>
    {
        public void Configure(EntityTypeBuilder<Rider> builder)
        {
            builder.Property(p => p.LicenceNumber).HasMaxLength(100).IsRequired();
            builder.HasOne(p => p.Vehicle).WithMany(p => p.Riders).HasForeignKey(p => p.VehicleId);
            builder.HasOne(p => p.User).WithMany(p => p.Riders).HasForeignKey(p => p.UserId);
        }
    }

    #endregion
}
