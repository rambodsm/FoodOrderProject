using FoodOrder.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FoodOrder.Domain.Restuarants
{
    public class OpeningHours:BaseEntity<Guid>
    {
        public OpeningHours()
        {
            Id = Guid.NewGuid();
        }
        public int FromHour { get; set; }
        public int FromMinute { get; set; }
        public int ToHour { get; set; }
        public int ToMinute { get; set; }
        public WeekDay WeekDay { get; set; }

        public Guid? RestuarantId { get; set; }

        #region Relations
        public Restuarant Restuarant { get; set; }
        #endregion
    }

    #region EntityConfiguration

    public class OpeningHoursConfiguration : IEntityTypeConfiguration<OpeningHours>
    {
        public void Configure(EntityTypeBuilder<OpeningHours> builder)
        {
            builder.HasOne(p => p.Restuarant).WithMany(p => p.OpeningHours).HasForeignKey(p => p.RestuarantId);
        }
    }

    #endregion
}
