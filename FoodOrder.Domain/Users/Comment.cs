using FoodOrder.Domain.Common;
using FoodOrder.Domain.Restuarants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrder.Domain.Users
{
    public class Comment : BaseEntity<Guid>
    {
        public Comment()
        {
            Id = Guid.NewGuid();
        }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ParentId { get; set; }

        public Guid? UserId { get; set; }
        public Guid? RestuarantId { get; set; }

        #region Relations
        public User User { get; set; }
        public Restuarant Restuarant { get; set; }
        [ForeignKey(nameof(ParentId))]
        public List<Comment> Comments { get; set; }
        #endregion
    }
    #region EntityConfiguration

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(p => p.Description).HasMaxLength(500).IsRequired();
            builder.HasOne(p => p.User).WithMany(p => p.Comments).HasForeignKey(p => p.UserId);
            builder.HasOne(p => p.Restuarant).WithMany(p => p.Comments).HasForeignKey(p => p.RestuarantId);
        }
    }

    #endregion
}
