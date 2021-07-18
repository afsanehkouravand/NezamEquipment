using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.Common.Interface;

namespace NezamEquipment.DomainClasses.Base
{
    public abstract class BaseEntity : ISoftDelete
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }
    }
}
