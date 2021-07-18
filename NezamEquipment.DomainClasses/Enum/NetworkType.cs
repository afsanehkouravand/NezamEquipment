using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum NetworkType
    {
        /// <summary>
        /// Domain
        /// </summary>
        [Display(Name = "Domain")]
        Domain,

        /// <summary>
        /// WorkGroup
        /// </summary>
        [Display(Name = "WorkGroup")]
        WorkGroup,
    }
}