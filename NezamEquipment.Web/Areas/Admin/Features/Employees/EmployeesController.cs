using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.ServiceLayer.Entity.Employees;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.Entity.Employees.Dto;


namespace NezamEquipment.Web.Areas.Admin.Features.Employees
{
    [AuthorizeControllerName("لیست کارمندان")]
    public partial class EmployeesController : AdminBaseController
    {
        private readonly IEmployeesService _nezamEmployeService;
       
        public EmployeesController(IEmployeesService nezamEmployeService)
        {
            _nezamEmployeService = nezamEmployeService;
           
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index(
            [Bind(Prefix = "S")]AdminNezamEmployeIndexSearchViewModel s, int page = 1, int pagesize = 20)
        {
            s = s ?? new AdminNezamEmployeIndexSearchViewModel();

            var viewModel = new AdminNezamEmployeIndexViewModel()
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
                    PropertyName = nameof(viewModel.NezamEmploye.Id),
                    Role = GetAllTupleDto.SortRole.Descending
                }
            };
            var data = await _nezamEmployeService.GetAllAsync(getAllTupleDto: getAllTupleDto,
                unitType: s.UnitType, fullName: s.NezamEmployeFullName,
                nationalCode: s.NezamEmployeNationalCode, codePersonal: s.CodePersonal,ip:s.IP,networkType:s.NetworkType);

            viewModel.NezamEmployes = data.List;
            viewModel.PageTotal = data.Count;

            return View(viewModel);
        }

        #endregion

        #region Details

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Details)]
        public virtual async Task<ActionResult> Details(Guid id)
        {
            var applicant = await _nezamEmployeService.GetAsync(id);
            if (applicant == null)
                return HttpNotFound();

            var viewModel = new AdminNezamEmployeDetailsViewModel()
            {
                NezamEmploye = applicant,
            };

            //viewModel.NezamEmploye.BirthDateStr = viewModel.NezamEmploye.BirthDate != null
            //    ? viewModel.NezamEmploye.BirthDate.Value.ToShortShamsi()
            //    : "-";

            return View(viewModel);
        }

        #endregion

        #region Add

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Add)]
        public virtual ActionResult Add()
        {
            var viewModel = new AdminNezamEmployeAddViewModel()
            {
                PartialForm = new AdminNezamEmployePartialFormViewModel()
            };


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Add(
            [Bind(Prefix = "EmployeesDto", Exclude = "Id")] EmployeDto dto)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            var result = await _nezamEmployeService.AddAsync(dto: dto);
            return AjaxResult(result);
        }

        #endregion

        #region Edit

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Edit)]
        public virtual async Task<ActionResult> Edit(Guid id)
        {
            var applicant = await _nezamEmployeService.GetAsync(id);
           
            if (applicant == null)
                return HttpNotFound();

            var viewModel = new AdminNezamEmployeEditViewModel()
            {
                PartialForm = new AdminNezamEmployePartialFormViewModel()
                {
                    EmployeesDto = applicant

                }
            };
          

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Edit(
            [Bind(Prefix = "EmployeesDto")] EmployeDto employeesDto,
            [Bind(Include = "Name,Base64,CustomData")]IList<UploadByBase64Dto> uploadByBase64,
            string password = null, string confirmPassword = null)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirmPassword))
            {
                if (password != confirmPassword)
                    return AjaxResult("رمز عبور وارد شده صحیح نمی باشد.");
            }

            var result = await _nezamEmployeService.UpdateAsync(dto: employeesDto,
                password: password);
            return AjaxResult(result);
        }

        #endregion

        #region ChangePass

        [AuthorizeActionName("تغییر رمز عبور")]
        public virtual async Task<ActionResult> PasswordCahnge(Guid id)
        {
            var applicant = await _nezamEmployeService.GetAsync(id);
            var viewModel = new AdminNezamEmployeEditViewModel()
            {
                PartialForm = new AdminNezamEmployePartialFormViewModel()
                {
                    EmployeesDto = applicant,
                    PasswordChange = null,

                }
            };
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> PasswordCahnge(Guid id, string passwordChange = null, string confirmPassword = null)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            if (!string.IsNullOrWhiteSpace(passwordChange) && !string.IsNullOrWhiteSpace(confirmPassword))
            {
                if (passwordChange != confirmPassword)
                    return AjaxResult("رمز عبور وارد شده صحیح نمی باشد.");
            }

            var result = await _nezamEmployeService.UpdateAsync(id: id, password: passwordChange);
            return AjaxResult(result);
        }

        #endregion
        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Delete)]
        public virtual async Task<JsonResult> Delete(Guid id)
        {
            var result = await _nezamEmployeService.DeleteAsync(id);
            return AjaxResult(result);
        }

        #endregion
            
    }
}