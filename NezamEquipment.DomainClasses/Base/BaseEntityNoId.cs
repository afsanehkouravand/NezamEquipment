using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.Common.Interface;

namespace NezamEquipment.DomainClasses.Base
{
    public abstract class BaseEntityNoId : ISoftDelete
    {
        public DateTime CreatedOn { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }
    }
}
