namespace MyCommon.Helpers.Ul
{
    public class UlItem
    {
        internal string Id { get; set; }

        internal string Title { get; set; }

        internal bool IsAjax { get; set; }
        internal string AjaxUrl { get; set; }
        internal string HaveAccessToArea { get; set; }
        internal string AjaxHaveAccessToController { get; set; }
        internal string AjaxHaveAccessToAction { get; set; }

        internal bool IsActive { get; set; }
    }
}