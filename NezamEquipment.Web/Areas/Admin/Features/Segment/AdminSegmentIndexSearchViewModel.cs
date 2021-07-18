using System;
using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Areas.Admin.Features.Segment
{
    public class AdminSegmentIndexSearchViewModel
    {
      

        [Display(Name = "نوع قطعه"), Required]
        public string Type { get; set; }

     
    }
}
