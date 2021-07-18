using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Base;

namespace NezamEquipment.DomainClasses.Entity.Equipment
{

    /// <summary>
    ///  قطعات
    /// </summary>
    public class Segment: BaseEntity
    {

        #region Properties

        /// <summary>
        ///  نوع قطعه
        /// </summary>
        public string Type { get; set; }



        #endregion



    }
}
