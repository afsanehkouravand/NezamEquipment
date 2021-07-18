using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.ServiceLayer.Entity.Report.StimulReport.Dto;


namespace NezamEquipment.ServiceLayer.Entity.Report.StimulReport
{
    public interface IStimulReportService
    {
        Task<DbResult> AddAsync(Dictionary<string, string> data);
        Task<StimulReportDto> GetAsync(object id = null);
        StimulReportDto Get(object id = null);
    }
}