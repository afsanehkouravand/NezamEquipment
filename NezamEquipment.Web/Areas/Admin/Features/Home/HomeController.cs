using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Employees;
using NezamEquipment.ServiceLayer.Entity.Equipment;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Areas.Admin.Features.Equipment;
using NezamEquipment.Web.Framework.Attribute;
using OfficeOpenXml;
using UnitType = NezamEquipment.DomainClasses.Enum.UnitType;

namespace NezamEquipment.Web.Areas.Admin.Features.Home
{
    [AuthorizeControllerName("داشبورد")]
    public partial class HomeController : AdminBaseController
    {
        private readonly IEmployeesService _nezamEmployeService;
        private readonly IEquipmentService _nezamEquipmentService;
        public HomeController(IEmployeesService nezamEmployeService, IEquipmentService nezamEquipmentService)
        {
            _nezamEmployeService = nezamEmployeService;
            _nezamEquipmentService = nezamEquipmentService;
        }

        #region Index

        [NoneAuthorizeAction]
        public virtual async Task<ActionResult> Index()
        {
            var data = await _nezamEmployeService.GetAllAsync();
            int casetotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.Case);
            int monitortotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.Monitor);
            int printertotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.Printer);
            int scanertotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.Scaner);
            int allinonetotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.AllinOne);
            int tablettotal = await _nezamEquipmentService.GetAllExcelCountAsync(equipmentType: EquipmentType.Tablet);
            int faxtotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.fax);
            int hardtotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.HardExternal);
            int mousetotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.MousedWireLess);
            int keytotal = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.KeyboardWireLess);
            int laptop = await _nezamEquipmentService.GetAllExcelCountAsync( equipmentType: EquipmentType.Laptop);

            var viewModel = new AdminHomeIndexViewModel();
            viewModel.CaseTotal = casetotal;
            viewModel.MonitorTotal = monitortotal;
            viewModel.PrinterTotal = printertotal;
            viewModel.ScanerTotal = scanertotal;
            viewModel.AllinoneTotal = allinonetotal;
            viewModel.LaptopTotal = laptop;
            viewModel.HardTotal = hardtotal;
            viewModel.mouseTotal = mousetotal;
            viewModel.KeyTotal = keytotal;
            viewModel.FaxTotal = faxtotal;
            viewModel.TabletTotal = tablettotal;
            viewModel.EmployeCount = data.Count;
            return View(viewModel);
        }
        #endregion

        #region ListEquipmentUnit

        [AuthorizeActionName("لیست تجهیزات اداری")]
        public virtual async Task <ActionResult> ListEquipmentUnit(UnitType unitType , int page = 1, int pagesize = 20 )
        {
            //s = s ?? new AdminNezamEquipmentIndexSearchViewModel();

            var viewModel = new AdminNezamEquipmentIndexViewModel()
            {
               // S = s,
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
            var data =  _nezamEquipmentService.GetAllAsync(getAllTupleDto: getAllTupleDto,
                unitType: unitType).Result;
            int casetotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType : unitType, equipmentType: EquipmentType.Case);
            int monitortotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.Monitor);
            int printertotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.Printer);
            int scanertotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.Scaner);
            int allinonetotal = await  _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.AllinOne);
            int tablettotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.Tablet);
            int faxtotal = await  _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.fax);
            int hardtotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.HardExternal);
            int mousetotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.MousedWireLess);
            int keytotal = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.KeyboardWireLess);
            int laptop = await _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType, equipmentType: EquipmentType.Laptop);
         
            viewModel.NezamEquipments = data.List;
            viewModel.unitType = unitType;
            viewModel.CaseTotal = casetotal;
            viewModel.MonitorTotal =monitortotal;
            viewModel.PrinterTotal =printertotal;
            viewModel.ScanerTotal = scanertotal;
            viewModel.AllinoneTotal = allinonetotal; 
            viewModel.LaptopTotal = laptop;
            viewModel.HardTotal = hardtotal;
            viewModel.mouseTotal = mousetotal;
            viewModel.KeyTotal = keytotal;
            viewModel.FaxTotal = faxtotal;
            viewModel.TabletTotal = tablettotal;
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

        #region PrivateMetod

        private async Task<AdminNezamEquipmentIndexViewModel> PrivateMetod(UnitType unitType,int? page = null,int? pageSize = null)
        {
            var pageNumber = (page == 0) ? 1 : page;

            var viewModel = new AdminNezamEquipmentIndexViewModel()
            {
                PageNumber = pageNumber ?? 1,

            };

            var list = await
                _nezamEquipmentService.GetAllExcelAsync(skip: pageNumber, take: pageSize, unitType: unitType);

            var listCont = await
                _nezamEquipmentService.GetAllExcelCountAsync(unitType: unitType);

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

            if (true)
                viewModel.PageTotal = listCont;

            return viewModel;
        }

        #endregion

        #region GetExcelFile

        [AuthorizeActionName("خروجی Excel")]
        public virtual async Task<ActionResult> GetExcelFile(UnitType unitType )
        {
            var list = await PrivateMetod( unitType: unitType);

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
                dataSource.Columns.Add("نام پرسنل");
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