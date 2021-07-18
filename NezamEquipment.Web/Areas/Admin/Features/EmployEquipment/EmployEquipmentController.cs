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

namespace NezamEquipment.Web.Areas.Admin.Features.EmployEquipment
{
    [AuthorizeControllerName("لیست تجهیزات کاربران")]
    public partial class EmployEquipmentController : AdminBaseController
    {
        private readonly IEquipmentService _nezamEquipmentService;
        private readonly IEmployeesService _nezamEmployeService;
        public EmployEquipmentController(IEquipmentService nezamEquipmentService,IEmployeesService nezamEmployeService)
        {
            _nezamEquipmentService = nezamEquipmentService;
            _nezamEmployeService = nezamEmployeService;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index(Guid id)


        {
            var viewModel = new AdminEmployEquipmentIndexViewModel();
            var employe = await _nezamEmployeService.GetAsync(id: id);
            if(employe!=null)
            {
                var result =await _nezamEquipmentService.GetAllAsync(employId: id);
                if(result !=null)
                {
                    viewModel.NezamEquipments =result.List ;
                }
            }
            return PartialView(viewModel);
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

     

     
    }
}