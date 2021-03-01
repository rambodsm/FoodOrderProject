using FoodOrder.Domain.Common;
using FoodOrder.Domain.Users;
using FoodOrder.Infrastructure.Persistance.Configs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrder.Infrastructure.Persistance
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions options)
           : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var assembly = typeof(IEntity).Assembly;
            builder.RegisterAllEntities<IEntity>(assembly);
            builder.RegisterEntityTypeConfiguration(assembly);
            builder.AddRestrictDeleteBehaviorConvention();
            builder.ApplySoftDeleteQueryFilters();
        }
        #region Save Changes Override      
        public override int SaveChanges()
        {
            beforeSave();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            beforeSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            beforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            beforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void beforeSave()
        {
            this.CleanString();
            //This method will Change behavior 'Remove()' and 'RemoveRange()' From Hard Delete to Soft Delete
            //this.SetAuditableEntityOnBeforeSaveChanges();
        }
        #endregion
    }
}
