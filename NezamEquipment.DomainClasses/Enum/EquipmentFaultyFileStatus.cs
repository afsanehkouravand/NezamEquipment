using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum EquipmentFaultyFileStatus
    {
        /// <summary>
        /// تایید
        /// </summary>
        [Display(Name = "تایید")]
        Confirm,

        /// <summary>
        /// رد
        /// </summary>
        [Display(Name = "رد")]
        Reject,

        /// <summary>
        /// None
        /// </summary>
        [Display(Name = "-")]
        None,

    }
}