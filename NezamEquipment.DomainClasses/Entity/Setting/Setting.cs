using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Base;

namespace NezamEquipment.DomainClasses.Entity.Setting
{
    public class Setting : BaseEntity
    {
        [Required, StringLength(200)]
        public string Key { get; set; }

        [StringLength(5000)]
        public string Value { get; set; }

        [Required, StringLength(100), Index]
        public string Section { get; set; }
    }
}
