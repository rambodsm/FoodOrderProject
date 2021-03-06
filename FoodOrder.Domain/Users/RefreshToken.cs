using FoodOrder.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Domain.Users
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public RefreshToken()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
            IsUsed = false;
            IsRevorked = false;
        }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid? UserId { get; set; }

        #region Relations
        public User User { get; set; }
        #endregion
    }
    #region EntityConfiguration

    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne(p => p.User).WithMany(p => p.RefreshTokens).HasForeignKey(p => p.UserId);
        }
    }

    #endregion
}
