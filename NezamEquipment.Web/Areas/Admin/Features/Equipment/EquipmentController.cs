using System;
using System.Collections.Generic;
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

namespace NezamEquipment.Web.Areas.Admin.Features.Equipment
{
    [AuthorizeControllerName("لیست تجهیزات کامپیوتری")]
    public partial class EquipmentController : AdminBaseController
    {
        private readonly IEquipmentService _nezamEquipmentService;
        private readonly IEmployeesService _nezamEmployeService;
        public EquipmentController(IEquipmentService nezamEquipmentService,IEmployeesService nezamEmployeService)
        {
            _nezamEquipmentService = nezamEquipmentService;
            _nezamEmployeService = nezamEmployeService;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index(
            [Bind(Prefix = "S")]AdminNezamEquipmentIndexSearchViewModel s, int page = 1, int pagesize = 20)
        {
            s = s ?? new AdminNezamEquipmentIndexSearchViewModel();

            var viewModel = new AdminNezamEquipmentIndexViewModel()
            {
                S = s,
                PageSize = pagesize,
                PageNumber = (page <= 0) ? 1 : page,
            };

            // get data from db
            var getAllTupleDto = new GetAllTupleDto()
            {
                Skip = viewModel.PageNumber,
                Take = viewModel.PageSize,
                ToSort = new GetAllTupleDto.Sort()
                {
                    PropertyName = nameof(viewModel.NezamEquipment.Id),
                    Role = GetAllTupleDto.SortRole.Descending
                }
            };
            var data = await _nezamEquipmentService.GetAllAsync(getAllTupleDto: getAllTupleDto,
                unitType: s.UnitType, code: s.Code,
                equipmentTypes: s.EquipmentType, equipmentStatus: s.EquipmentStatus, fromCreatedOn: s.FromCreatedOn, toCreatedOn: s.ToCreatedOn);

            viewModel.NezamEquipments = data.List;
            viewModel.PageTotal = data.Count;

            return View(viewModel);
        }

        #endregion

      

        #region Details

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Details)]
        public virtual async Task<ActionResult> Details(Guid id)
        {
            var applicant = await _nezamEquipmentService.GetAsync(id);
            if (applicant == null)
                return HttpNotFound();

            var viewModel = new AdminNezamEquipmentDetailsViewModel()
            {
                NezamEquipment = applicant,
            };

            //viewModel.NezamEquipment.BirthDateStr = viewModel.NezamEquipment.BirthDate != null
            //    ? viewModel.NezamEquipment.BirthDate.Value.ToShortShamsi()
            //    : "-";

            return View(viewModel);
        }

        #endregion

        #region Add

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Add)]
       public  virtual async Task<ActionResult> Add()
        {
            var viewModel = new AdminNezamEquipmentAddViewModel()
            {
                PartialForm = new AdminNezamEquipmentPartialFormViewModel()
                {
                    DropDown = (await _nezamEmployeService.GetAllNewAsync()).ToDictionary(x => x.Id.ToString(), x => x.Fullname),
                }                       
          
        };

            return View(viewModel);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Add([Bind(Prefix = "EquipmentDto", Exclude = "Id")] EquipmentDto dto)
        {
            
            var result = await _nezamEquipmentService.AddAsync(dto: dto);
            return AjaxResult(result);
        }

        #endregion

        #region Edit

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Edit)]
        public virtual async Task<ActionResult> Edit(Guid id)
        {
            var applicant = await _nezamEquipmentService.GetAsync(id);
           
            if (applicant == null)
                return HttpNotFound();

            var viewModel = new AdminNezamEquipmentEditViewModel()
            {
                PartialForm = new AdminNezamEquipmentPartialFormViewModel()
                {
                    EquipmentDto = applicant,
                    DropDown = (await _nezamEmployeService.GetAllNewAsync()).ToDictionary(x => x.Id.ToString(), x => x.Fullname),
                }
            };
          

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Edit(
            [Bind(Prefix = "EquipmentDto")] EquipmentDto applicantDto     
           )
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            var result = await _nezamEquipmentService.UpdateAsync(dto: applicantDto);
            return AjaxResult(result);
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Delete)]
        public virtual async Task<JsonResult> Delete(Guid id)
        {
            var result = await _nezamEquipmentService.DeleteAsync(id);
            return AjaxResult(result);
        }

        #endregion

        #region PrivateMetod

        private async Task<AdminNezamEquipmentIndexViewModel> PrivateMetod(bool pageTs, int? page = null,
            int? pageSize = null, UnitType? unitType = null, DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null, EquipmentStatus? equipmentStatus = null,
            EquipmentType? equipmentType = null,
            bool? isOk = null)
        {
            var pageNumber = (page == 0) ? 1 : page;

            var viewModel = new AdminNezamEquipmentIndexViewModel()
            {
                PageNumber = pageNumber ?? 1,

            };

            var list = await
                _nezamEquipmentService.GetAllExcelAsync(skip: pageNumber, take: pageSize, dateBuyFrom: dateBuyFrom,
                    dateBuyTo: dateBuyTo, unitType: unitType,equipmentStatus:equipmentStatus,
                    equipmentType: equipmentType);

            var listCont = await
                _nezamEquipmentService.GetAllExcelCountAsync(dateBuyFrom: dateBuyFrom,
                    dateBuyTo: dateBuyTo, unitType: unitType, equipmentStatus: equipmentStatus,
                    equipmentType: equipmentType);

            var viewModelViewPayMonyFullInfo = list.ToList();

            foreach (var item in viewModelViewPayMonyFullInfo)
            {
                var aplicant = await _nezamEmployeService.GetAsync(id: item.EmployeesId);
                item.EmployeFullName = aplicant.Fullname;
                             

            }

            viewModel.NezamEquipments = viewModelViewPayMonyFullInfo;

            var payMonyDtos = viewModel.NezamEquipments as IList<EquipmentDto> ??
                              viewModel.NezamEquipments.ToList();
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


                foreach (var item in list.NezamEquipments)
                {


                       dataSource.Rows.Add(
                         item.EquipmentTypes.GetDisplayName(),
                         item.Brand,
                         item.Model,
                         item.unitType.GetDisplayName(),
                          item.EquipmentStatus.GetDisplayName(),
                          item.Code,
                         string.IsNullOrEmpty(item.EmployeFullName) ? "-" : item.EmployeFullName,
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