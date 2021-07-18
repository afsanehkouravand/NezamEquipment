using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum EquipmentStatus
    {
        /// <summary>
        ///  سالم
        /// </summary>
        [Display(Name = "سالم")]
        Healthy,

        /// <summary>
        ///  خراب
        /// </summary>
        [Display(Name = "خراب")]
        Faulty,
            
            /// <summary>
             ///  نیاز به سرویس
             /// </summary>
        [Display(Name = " نیاز به سرویس")]
        Servise
    }
}