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
    public class Color:BaseEntity<Guid>
    {
        public Color()
        {
            Id = Guid.NewGuid();
        }
        public string Name { get; set; }
        public string Code { get; set; }

        #region Relations
        public List<Vehicle> Vehicles { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.Property(p => p.Code).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            
        }
    }

    #endregion
}
