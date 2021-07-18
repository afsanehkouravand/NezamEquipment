using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NezamEquipment.Common.Interface;
using EFSecondLevelCache;
using Microsoft.AspNet.Identity.EntityFramework;
using NezamEquipment.DataLayer.Migrations;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses;
using NezamEquipment.DomainClasses.Entity.Employees;
using NezamEquipment.DomainClasses.Entity.Other;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog;
using NezamEquipment.DomainClasses.Entity.Report;
using NezamEquipment.DomainClasses.Entity.Setting;
using NezamEquipment.DomainClasses.Identity;
using EntityFramework.DynamicFilters;
using NezamEquipment.DomainClasses.Entity.Equipment;
using NezamEquipment.DomainClasses.Entity.EquipmentFaulty;

namespace NezamEquipment.DataLayer.DbContext
{
   

    public class NezamEquipmentDbContext : IdentityDbContext<User, Role,
       Guid, UserLogin, UserRole, UserClaim>, IUnitOfWork<NezamEquipmentDbContext>
    {

        #region Ctor

        static NezamEquipmentDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NezamEquipmentDbContext, Configuration>());

            //Database.SetInitializer<NezamEquipmentDBContext>(null);
        }

        public NezamEquipmentDbContext()
            : base("Name=NezamEquipmentDBContext")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 120;
        }

        #endregion

        #region Entity

        public DbSet<RoleAccess> RoleAccesses { get; set; }
        public DbSet<Setting> Settings { get; set; }
      

        #region EntityNew

        public DbSet<Employe> Employees { get; set; }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentFaulty> EquipmentFaulties { get; set; }
        public DbSet<StateLog> StateLogs { get; set; }
        public DbSet<Segment> Segments { get; set; }
        #endregion
        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(DomainClassesAssembly).GetTypeInfo().Assembly);
            modelBuilder.Filter("IsDeleted", (ISoftDelete d) => d.IsDeleted, false);
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region IUnitOfWork Members

        public void RejectChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void MarkAsSafeDelete<TEntity>(TEntity entity) where TEntity : class
        {
            var entityType = entity.GetType();
            var propertyInfo = entityType.GetProperty("IsDeleted");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(entity, true, null);
            }
            else
            {
                throw new Exception("Property IsDelete is not define.");
            }

            Entry(entity).State = EntityState.Modified;
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            return SaveAllChanges();
        }
        public override async Task<int> SaveChangesAsync()
        {
            return await SaveAllChangesAsync();
        }

        // >>>>>>>>>> EF Second level caching
        public int SaveAllChanges(bool invalidateCacheDependencies = true)
        {
            var changedEntityNames = GetChangedEntityNames();
            var result = base.SaveChanges();
            if (invalidateCacheDependencies)
            {
                new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            }
            return result;
        }
        public async Task<int> SaveAllChangesAsync(bool invalidateCacheDependencies = true)
        {
            var changedEntityNames = GetChangedEntityNames();
            var result = await base.SaveChangesAsync();
            if (invalidateCacheDependencies)
            {
                new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            }
            return result;
        }
        private string[] GetChangedEntityNames()
        {
            return ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added ||
                            x.State == EntityState.Modified ||
                            x.State == EntityState.Deleted)
                .Select(x => ObjectContext.GetObjectType(x.Entity.GetType()).FullName)
                .Distinct()
                .ToArray();
        }
        // <<<<<<<<<<<

        #endregion

    }
}
