using NezamEquipment.ServiceLayer.Entity.Segment.Dto;
using NezamEquipment.Web.Areas.Admin.Base;


namespace NezamEquipment.Web.Areas.Admin.Features.Segment
{
    public class AdminSegmentAddViewModel :AdminBaseViewModel
    {
        public AdminSegmentAddViewModel()
        {
            Segment = new SegmentDto();
        }
        public SegmentDto Segment { get; set; }
        

        public string Error { get; set; }
    }
}
