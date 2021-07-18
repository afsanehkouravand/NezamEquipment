using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using NezamEquipment.DataLayer.DbContext;

namespace NezamEquipment.DataLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<NezamEquipmentDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(NezamEquipmentDbContext context)
        {

            #region Sql

            if (!context.Users.Any(x => x.UserName == "sa"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty).Replace("\\Debug", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\AddAspNetIdentity.sql"));
            }

            #endregion

            #region Default

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            #endregion

        }
    }
}
