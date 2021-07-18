
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
namespace NezamEquipment.Web.Areas.Report.Features.TransfrPermitGroup
{
    public partial class TransfrPermitGroupController : ReportBaseController
    {
        private readonly ITransferIssuingPermitService _issuingPermitService;
      
        private readonly ITransferService _transferService;
        private readonly INezamEmployeService _NezamEmploye;
        private readonly IUserManager _userManager;
        private readonly IStimulReportService _stimulReportService;

        public TransfrPermitGroupController(IUserManager userManager,
            ITransferIssuingPermitService issuingPermitService, ITransferService transferService
            , IStimulReportService stimulReportService, INezamEmployeService NezamEmploye)
        {
            _userManager = userManager;
            _issuingPermitService = issuingPermitService;
            _transferService = transferService;
            _stimulReportService = stimulReportService;
            _NezamEmploye = NezamEmploye;


        }

        #region TransfrPermitGroup

        public virtual ActionResult TransfrPermitGroup(string id)
        {

            var viewModel = new TransfrPermitGroupViewModel { EnablePrint = true };
          
            return View(viewModel);
        }

        #endregion

        #region Report

        public virtual async Task<ActionResult> Report()
         {
            var rpt = new StiReport();
            rpt.Load("~/Templates/StimulReport/TransferPermitGroup.mrt".MapPath());

            var dateStart = StimulReportData.FirstOrDefault(x => x.Key == "id" ).Value;
            var dateEnd= StimulReportData.FirstOrDefault(x => x.Key == "value").Value;
            
            var transferPermitList = await _issuingPermitService.GetAllPermitReportAsync(dateFrom: Convert.ToDateTime(dateStart), dateTo: Convert.ToDateTime(dateEnd), isLast: true);

            if (Request.Url != null)
             {

                var list = new List<object>();
                 foreach (var item in transferPermitList)
                 {
                     item.IsDueDateStr = item.IsDueDate.ToShortShamsi(false);
                     item.QrCode = "شماره مجوز:" + item.LicenseNumber + "\n بهره بردار:" + item.FullName;
                    list.Add(new
                     {
                         item.LicenseNumber,
                         item.ContractNumber,
                         item.IsDueDateStr,
                         item.Description,
                         item.FullName ,
                         item.NationalCode,
                         item.FatherName ,
                         item.Address,
                         item.QrCode,
                     });
                 }
                 rpt.RegData("tb1", list);

                //string qrCode = "شماره مجوز:" + transferPermit.LicenseNumber +"\n بهره بردار:"+ transfer.Fullname ; 
                // if (transfer == null)
                //     return HttpNotFound();
                // string timestring = transferPermit.IsDueDate.ToShortShamsi(false);
                
                // rpt.Dictionary.Variables.Add("IssueDate", timestring);
                // rpt.Dictionary.Variables.Add("LicenseNumber", transferPermit.LicenseNumber);
                // if (!string.IsNullOrWhiteSpace(transferPermit.Description))
                // {
                //     rpt.Dictionary.Variables.Add("Description", transferPermit.Description);
                // }
                // else
                // {
                //     rpt.Dictionary.Variables.Add("Description","ندارد");
                // }
                // rpt.Dictionary.Variables.Add("Fullname", applicant.Fullname);
                // rpt.Dictionary.Variables.Add("ContractNumber", transfer.ContractNumber);
                // rpt.Dictionary.Variables.Add("Address", applicant.Address);
                // rpt.Dictionary.Variables.Add("FatherName", applicant.FatherName);
                // rpt.Dictionary.Variables.Add("NationalCode", applicant.NationalCode);
                // rpt.Dictionary.Variables.Add("Reqcode", qrCode);
             }
             return StiMvcViewer.GetReportSnapshotResult(HttpContext, rpt);
        }

        #endregion

    }
}