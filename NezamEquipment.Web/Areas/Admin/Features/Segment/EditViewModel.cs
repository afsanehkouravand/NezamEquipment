using NezamEquipment.ServiceLayer.Entity.Segment.Dto;
using NezamEquipment.Web.Areas.Admin.Base;


namespace NezamEquipment.Web.Areas.Admin.Features.Segment
{
    public class AdminSegmentEditViewModel : AdminBaseViewModel
    {
        public AdminSegmentEditViewModel()
        {
            Segment = new SegmentDto();
        }
        public SegmentDto Segment { get; set; }


    }
}
