using System.Collections.Generic;
using NezamEquipment.ServiceLayer.Entity.Segment.Dto;
using NezamEquipment.Web.Areas.Admin.Base;


namespace NezamEquipment.Web.Areas.Admin.Features.Segment
{
    public class AdminSegmentIndexViewModel : AdminBaseViewModel
    {
        public AdminSegmentIndexViewModel()
        {
            Segment = new SegmentDto();
            Segments = new List<SegmentDto>();
        }

        public SegmentDto Segment { get; set; }

        public IEnumerable<SegmentDto> Segments { get; set; }
        public int PageTotal { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public AdminSegmentIndexSearchViewModel S { get; set; }

        public bool DoSearch { get; set; }


    }
}
