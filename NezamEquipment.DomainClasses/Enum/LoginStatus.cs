using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum LoginStatus
    {
        /// <summary>
        ///  ندارد
        /// </summary>
        [Display(Name = "ندارد")]
        No,

        /// <summary>
        ///  دارد
        /// </summary>
        [Display(Name = "دارد")]
        Yes
    }
}