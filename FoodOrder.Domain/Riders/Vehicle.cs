using FoodOrder.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Domain.Riders
{
    public class Vehicle : BaseEntity<Guid>
    {
        public Vehicle()
        {
            Id = Guid.NewGuid();
        }
        public string Tag { get; set; }
        public int YearOfManufacture { get; set; }
        public int MonthOfManufacture { get; set; }

        public Guid? ColorId { get; set; }

        #region Relations
        public Color Color { get; set; }
        public List<Rider> Riders { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.Property(p => p.Tag).HasMaxLength(50).IsRequired();
        }
    }

    #endregion
}
