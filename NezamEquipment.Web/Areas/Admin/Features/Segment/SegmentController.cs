using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

using NezamEquipment.Common.Extension;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.ServiceLayer.Entity.Segment;
using NezamEquipment.ServiceLayer.Entity.Segment.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;

namespace NezamEquipment.Web.Areas.Admin.Features.Segment
{
    [AuthorizeControllerName("قطعات", "SegmentTransfer")]
    public partial class SegmentController : AdminBaseController
    {
        private readonly ISegmentService _segmentService;
      


        public SegmentController(ISegmentService segmentService)
        {

          
            _segmentService = segmentService;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index([Bind(Prefix = "S")]AdminSegmentIndexSearchViewModel s,
            int page = 1, int pageSize = 20, bool? doSearch = null,int ? year = null)
        {
            s = s ?? new AdminSegmentIndexSearchViewModel();

            var viewModel = new AdminSegmentIndexViewModel()
            {
                S = s,
                PageSize = pageSize,
                PageNumber = (page <= 0) ? 1 : page,
                DoSearch = doSearch ?? false,
            };

            if (doSearch != null && doSearch.Value)
            {
                var getAllTupleDto = new GetAllTupleDto()
                {
                    Skip = viewModel.PageNumber,
                    Take = viewModel.PageSize,
                    ToSort = new GetAllTupleDto.Sort()
                    {
                        PropertyName = nameof(viewModel.Segment.CreatedOn),
                        Role = GetAllTupleDto.SortRole.Ascending
                    }
                };
                var data = await
                    _segmentService.GetAllAsync(getAllTupleDto: getAllTupleDto
                       );

                foreach (var item in data.List)
                {
                    item.CreatedOnStr = item.CreatedOn.ToShortShamsi(true);
                }
                viewModel.Segments = data.List;
                viewModel.PageTotal = data.Count;
            }

            return View(viewModel);
        }

        #endregion

        #region Add

        public virtual ActionResult Add()
        {
            var viewModel = new AdminSegmentAddViewModel();
          
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Add(
            [Bind(Prefix = "Segment", Exclude = "Id")] SegmentDto segment)
        {
            if (!ModelState.IsValid)
               return AjaxResult(GetMessage.ModelStateIsNotValid);

          
            var result = await _segmentService.AddAsync(segment);
            return AjaxResult(result);
        }

        #endregion
        #region Edit
        public virtual async Task<ActionResult> Edit(Guid id)
        {
            var getTransfer = await _segmentService.GetAsync(id);
            if (getTransfer == null)
                return HttpNotFound();

            return View(new AdminSegmentEditViewModel()
            {
                Segment = getTransfer,

            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Edit([Bind(Prefix = "Segment")] SegmentDto  segment)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);


            var result = await _segmentService.EditAsync(dto: segment);
            return AjaxResult(result);
        }

        #endregion
        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeActionName("حذف اطلاعات پایه ارزش تجاری")]
        public virtual async Task<JsonResult> Delete(Guid id)
        {
            var result = await _segmentService.DeleteAsync(id: id);
            return AjaxResult(result);
        }

        #endregion

        #region DoNotShowSearchBox

        [AuthorizeActionName("عدم نمایش کادر جستجو")]
        public virtual ActionResult DoNotShowSearchBox()
        {
            return RedirectToAction("Index");
        }

        #endregion

    }
}