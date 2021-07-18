
using System;

using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.ServiceLayer.Entity.Applicant.NezamEmploye;
using NezamEquipment.ServiceLayer.Entity.Report.StimulReport;
using NezamEquipment.ServiceLayer.Entity.Transfer.Transfer;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.Web.Areas.Report.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;
namespace NezamEquipment.Web.Areas.Report.Features.TransfrPermitReport
{
    public partial class TransfrPermitReportController : ReportBaseController
    {
        private readonly ITransferIssuingPermitService _issuingPermitService;
      
        private readonly ITransferService _transferService;
        private readonly INezamEmployeService _NezamEmploye;
        private readonly IUserManager _userManager;
        private readonly IStimulReportService _stimulReportService;

        public TransfrPermitReportController(IUserManager userManager,
            ITransferIssuingPermitService issuingPermitService, ITransferService transferService
            , IStimulReportService stimulReportService, INezamEmployeService NezamEmploye)
        {
            _userManager = userManager;
            _issuingPermitService = issuingPermitService;
            _transferService = transferService;
            _stimulReportService = stimulReportService;
            _NezamEmploye = NezamEmploye;


        }

        #region TransfrPermitReport

        public virtual ActionResult TransfrPermit(string id)
        {

            var viewModel = new TransfrPermitViewModel { EnablePrint = true };
            viewModel.Id = id;
            return View(viewModel);
        }

        #endregion

        #region Report

        public virtual async Task<ActionResult> Report()
         {
            var rpt = new StiReport();
            rpt.Load("~/Templates/StimulReport/TransferPermit.mrt".MapPath());

            var id = StimulReportData.FirstOrDefault(x => x.Key == "id").Value;

           
             var transfer = await _transferService.GetAsync(id: Guid.Parse(id));
            
             var applicant = await _NezamEmploye.GetAsync(id: transfer.NezamEmployeId);
            var transferPermit = await _issuingPermitService.GetAsync(transferId: Guid.Parse(id));
             if (Request.Url != null)
             {
                 //string url = Url.Action(MVC.Report.TransfrPermitReport.TransfrPermit(id), Request.Url.Scheme);
          
                 string qrCode = "شماره مجوز:" + transferPermit.LicenseNumber +"\n بهره بردار:"+ transfer.Fullname ; 
                 if (transfer == null)
                     return HttpNotFound();
                 string timestring = transferPermit.IsDueDate.ToShortShamsi(false);
                 
                 string timestringEndDate = transferPermit.IsDueDate.AddYears(1).ToShortShamsi(false);
                 //transfer.CreatedOnStr = transfer.CreatedOn.ToShortShamsi(true);
                 rpt.Dictionary.Variables.Add("IssueDate", timestring);
                 rpt.Dictionary.Variables.Add("EndDate", timestringEndDate);
                 rpt.Dictionary.Variables.Add("LicenseNumber", transferPermit.LicenseNumber);
                 if (!string.IsNullOrWhiteSpace(transferPermit.Description))
                 {
                     rpt.Dictionary.Variables.Add("Description", transferPermit.Description);
                 }
                 else
                 {
                     rpt.Dictionary.Variables.Add("Description","ندارد");
                 }
                 rpt.Dictionary.Variables.Add("Fullname", applicant.Fullname);
                 rpt.Dictionary.Variables.Add("ContractNumber", transfer.ContractNumber);
                 rpt.Dictionary.Variables.Add("Address", transfer.Address);
                 rpt.Dictionary.Variables.Add("FatherName", applicant.FatherName);
                 rpt.Dictionary.Variables.Add("NationalCode", applicant.NationalCode);
                 rpt.Dictionary.Variables.Add("Reqcode", qrCode);
             }
             return StiMvcViewer.GetReportSnapshotResult(HttpContext, rpt);
        }

        #endregion

    }
}