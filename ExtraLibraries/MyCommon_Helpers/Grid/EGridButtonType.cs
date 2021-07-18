using System.ComponentModel.DataAnnotations;

namespace MyCommon.Helpers.Grid
{
    public enum EGridButtonType
    {
        [Display(Name = "btn btn-default margin-right-10")]
        BtnDefault,
        [Display(Name = "btn btn-success margin-right-10")]
        BtnSuccess,
        [Display(Name = "btn btn-warning margin-right-10")]
        BtnWarning,
        [Display(Name = "btn btn-info margin-right-10")]
        BtnInfo,
        [Display(Name = "btn btn-danger margin-right-10")]
        BtnDanger,
        [Display(Name = "btn btn-primary margin-right-10")]
        BtnPrimary,

        [Display(Name = "label label-default margin-right-10")]
        LabelDefault,
        [Display(Name = "label label-success margin-right-10")]
        LabelSuccess,
        [Display(Name = "label label-warning margin-right-10")]
        LabelWarning,
        [Display(Name = "label label-info margin-right-10")]
        LabelInfo,
        [Display(Name = "label label-danger margin-right-10")]
        LabelDanger,
        [Display(Name = "label label-primary margin-right-10")]
        LabelPrimary,
    }
}