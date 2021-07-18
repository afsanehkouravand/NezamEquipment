using System.ComponentModel.DataAnnotations;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.Segment.Dto
{
 
    public class SegmentDto : BaseEntityDto
    {


        /// <summary>
        /// نوع قطعه
        /// </summary>
        [Display(Name = "نوع قطعه")]
       public string Type { get; set; }


    }
}
