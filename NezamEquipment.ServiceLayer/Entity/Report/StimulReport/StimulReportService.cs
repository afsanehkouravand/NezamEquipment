using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Report.StimulReport.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;

namespace NezamEquipment.ServiceLayer.Entity.Report.StimulReport
{
    public class StimulReportService : BaseSaveDbResult, IStimulReportService
    {
        private readonly IDbSet<DomainClasses.Entity.Report.StimulReport> _stimulReports;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;

        public StimulReportService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stimulReports = _unitOfWork.Set<DomainClasses.Entity.Report.StimulReport>();
        }

        public async Task<DbResult> AddAsync(Dictionary<string, string> data)
        {
            if (data == null || !data.Any())
                return new DbResult(false);

            var stimulReport = new DomainClasses.Entity.Report.StimulReport()
            {
                Data = Json.Encode(data),
            };

            _stimulReports.Add(stimulReport);

            return await SaveDbResult(_unitOfWork, stimulReport);
        }

        public async Task<StimulReportDto> GetAsync(object id = null)
        {
            if (id == null)
                return null;

            var stimulReport = await _stimulReports.AsNoTracking()
                .Where(x => x.Id.ToString() == id.ToString())
                .ProjectTo<StimulReportDto>(_mapper.ConfigurationProvider)
                .Cacheable().FirstOrDefaultAsync();

            if (stimulReport == null)
                return null;

            stimulReport.DataDictionary = Json.Decode<Dictionary<string, string>>(stimulReport.Data);

            return stimulReport;
        }

        public StimulReportDto Get(object id = null)
        {
            if (id == null)
                return null;

            var stimulReport = _stimulReports.AsNoTracking()
                .Where(x => x.Id.ToString() == id.ToString())
                .ProjectTo<StimulReportDto>(_mapper.ConfigurationProvider)
                .Cacheable()
                .FirstOrDefault();

            if (stimulReport == null)
                return null;

            stimulReport.DataDictionary = Json.Decode<Dictionary<string, string>>(stimulReport.Data);

            return stimulReport;
        }

    }
}
