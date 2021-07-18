using System.Data.Entity.ModelConfiguration;

namespace NezamEquipment.DomainClasses.Entity.Equipment.Mapping
{
    public class EquipmentMap : EntityTypeConfiguration<Equipment>
    {
        public EquipmentMap()
        {

            HasRequired(x => x.User)
                .WithMany(x => x.Equipments)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);


        }
    }
}
