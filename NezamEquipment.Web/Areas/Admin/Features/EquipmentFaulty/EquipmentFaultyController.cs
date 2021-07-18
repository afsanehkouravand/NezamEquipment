using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.ServiceLayer.Entity.Equipment;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Employees;
using System.Linq;
using OfficeOpenXml;
using System.Data;
using System.IO;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile;
using NezamEquipment.Web.Areas.Admin.Features.Equipment;

namespace NezamEquipment.Web.Areas.Admin.Features.EquipmentFaulty
{
    [AuthorizeControllerName("لیست  قطعات تعمیری")]
    public partial class EquipmentFaultyController : AdminBaseController
    {
        private readonly IEquipmentService _nezamEquipmentService;
        private readonly IEquipmentFaultyService _equipmentFaultyService;
        private readonly IEquipmentFaultyFileService _equipmentFaultyFilesService;
        
       
        public EquipmentFaultyController(IEquipmentService nezamEquipmentService, IEquipmentFaultyService equipmentFaultyService, IEquipmentFaultyFileService equipmentFaultyFilesService)
        {
            _nezamEquipmentService = nezamEquipmentService;
            _equipmentFaultyService = equipmentFaultyService;
            _equipmentFaultyFilesService = equipmentFaultyFilesService;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index(Guid id)
        {
            var viewModel = new AdminEquipmentFaultyIndexViewModel();

           

            // get data from db
           
            var data = await _equipmentFaultyService.GetAllAsync(equipmentId:id);

            viewModel.EquipmentFaultyDtos = data.List;
            
            viewModel.EquipmentFaultyFiles = (await _equipmentFaultyFilesService.GetAllAsync(EquipmentFaultyId: id)).List;
            viewModel.ImagePath = $"~/{ConfigurationManager.AppSettings["FolderPath.FactorDoc"]}";


            return PartialView(viewModel);
        }

        #endregion

      

        #region Details

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Details)]
        public virtual async Task<ActionResult> Details(Guid id)
        {
            var equipmentFaulty = await _equipmentFaultyService.GetAsync(id);
            if (equipmentFaulty == null)
                return HttpNotFound();
          
            var viewModel = new AdminEquipmentFaultyDetailsViewModel()
            {
                EquipmentFaultyDto = equipmentFaulty,
            };
         
            //viewModel.NezamEquipment.BirthDateStr = viewModel.NezamEquipment.BirthDate != null
            //    ? viewModel.NezamEquipment.BirthDate.Value.ToShortShamsi()
            //    : "-";

            return View(viewModel);
        }

        #endregion

        #region Add

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Add)]
       public  virtual async Task<ActionResult> Add(Guid id)
        {
            var equipmentDto = await _nezamEquipmentService.GetAsync(id);
            var viewModel = new AdminEquipmentFaultyAddViewModel()
            {
                PartialForm = new AdminEquipmentFaultyPartialFormViewModel()
                {
                    EquipmentId = equipmentDto.Id,
                },

            };

            return View(viewModel);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Add([Bind(Prefix = "EquipmentFaultyDto", Exclude = "Id")] EquipmentFaultyDto dto,Guid equipmentId, [Bind(Prefix = "UploadByBase64")] UploadByBase64Dto uploadByBase64)
        {
            dto.EquipmentId = equipmentId;
           
            var result = await _equipmentFaultyService.AddAsync(dto: dto, uploadByBase64:uploadByBase64);
            return AjaxResult(result);
        }

        #endregion

        #region Edit

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Edit)]
        public virtual async Task<ActionResult> Edit(Guid id)
        {
            var applicant = await _equipmentFaultyService.GetAsync(id);
           
            if (applicant == null)
                return HttpNotFound();

            var viewModel = new AdminEquipmentFaultyEditViewModel()
            {
                PartialForm = new AdminEquipmentFaultyPartialFormViewModel(),
               
            };
          

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Edit(
            [Bind(Prefix = "EquipmentFaultyDto")] EquipmentFaultyDto applicantDto     
           )
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            var result = await _equipmentFaultyService.UpdateAsync(dto: applicantDto);
            return AjaxResult(result);
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Delete)]
        public virtual async Task<JsonResult> Delete(Guid id)
        {
            var result = await _equipmentFaultyService.DeleteAsync(id);
            return AjaxResult(result);
        }

        #endregion

        #region PrivateMetod

        private async Task<AdminEquipmentFaultyIndexViewModel> PrivateMetod(bool pageTs, int? page = null,
            int? pageSize = null, UnitType? unitType = null, DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null, EquipmentStatus? equipmentStatus = null,
            EquipmentType? equipmentType = null,
            bool? isOk = null)
        {
            var pageNumber = (page == 0) ? 1 : page;

            var viewModel = new AdminEquipmentFaultyIndexViewModel()
            {
                PageNumber = pageNumber ?? 1,

            };

            var list = await
                _equipmentFaultyService.GetAllExcelAsync(skip: pageNumber, take: pageSize, dateBuyFrom: dateBuyFrom,
                    dateBuyTo: dateBuyTo);

            var listCont = await
                _nezamEquipmentService.GetAllExcelCountAsync(dateBuyFrom: dateBuyFrom,
                    dateBuyTo: dateBuyTo);

            var viewModelViewPayMonyFullInfo = list.ToList();

            foreach (var item in viewModelViewPayMonyFullInfo)
            {
                var aplicant = await _equipmentFaultyService.GetAsync(id: item.EquipmentId);
               
                             

            }

           // viewModel.EquipmentFaultyDto = viewModelViewPayMonyFullInfo;

            var payMonyDtos = viewModel.EquipmentFaultyDtos as IList<EquipmentFaultyDto> ??
                              viewModel.EquipmentFaultyDtos.ToList();
           // var sum = payMonyDtos.Sum(x => x.Amount);

            if (pageTs)
                viewModel.PageTotal = listCont;

            return viewModel;
        }

        #endregion

        #region GetExcelFile

        [AuthorizeActionName("خروجی Excel")]
        public virtual async Task<ActionResult> GetExcelFile(DateTime? dateBuyFrom = null,
            DateTime? dateBuyTo = null,EquipmentType? equipmentType = null,
            EquipmentStatus? equipmentStatus = null, UnitType? unitType = null)
        {
                var list = await PrivateMetod(false, dateBuyFrom: dateBuyFrom,
                dateBuyTo: dateBuyTo,unitType: unitType, equipmentStatus: equipmentStatus,
                    equipmentType: equipmentType);

            ExcelPackage excel;
            ExcelWorksheet workSheet;

            using (var dataSource = new DataTable())
            {
               
                dataSource.Columns.Add("نوع تجهیزات");
                dataSource.Columns.Add("برند");
                dataSource.Columns.Add("مدل");
                dataSource.Columns.Add("نام واحد");
                dataSource.Columns.Add("وضعیت");
                dataSource.Columns.Add("کد");
                dataSource.Columns.Add("نام کارمند");
                dataSource.Columns.Add("تاریخ ثبت");


                foreach (var item in list.EquipmentFaultyDtos)
                {


                       dataSource.Rows.Add(
                         //item.EquipmentTypes.GetDisplayName(),
                         item.MoneyRepair,
                         item.RepairDate,
                      
                          item.Description,
                      //   string.IsNullOrEmpty(item.EmployeFullName) ? "-" : item.EmployeFullName,
                        //item.DateBuy.ToShortShamsi(false),
                        item.CreatedOn.ToShortShamsi(false)                  
                        );
                }

                excel = new ExcelPackage();
               
                workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromDataTable(dataSource, true);
            }

            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(4).AutoFit();
            workSheet.Column(5).AutoFit();
            workSheet.Column(6).AutoFit();
            workSheet.Column(7).AutoFit();
            workSheet.Column(8).AutoFit();

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=ReportPayment.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            return Content("Done.");
        }

        #endregion

    }
}