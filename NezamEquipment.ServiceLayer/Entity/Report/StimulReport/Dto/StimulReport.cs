using System;
using System.Collections.Generic;
using NezamEquipment.ServiceLayer.Base;


namespace NezamEquipment.ServiceLayer.Entity.Report.StimulReport.Dto
{
    public class StimulReportDto : BaseEntityDto
    {
        // public Guid GuidId { get; set; }
       // public long Id { get; set; }
        /// <summary>
        /// اطلاعات کوئری لینک
        /// </summary>
        public string Data { get; set; }

        public Dictionary<string, string> DataDictionary { get; set; }
    }
}
