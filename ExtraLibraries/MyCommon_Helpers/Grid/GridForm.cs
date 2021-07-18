using System.Collections.Generic;

namespace MyCommon.Helpers.Grid
{
    public class GridForm
    {
        public GridForm()
        {
            HiddenInput = new Dictionary<string, string>();
            SubmitColor = EGridColorType.Success;
            SubmitText = "اطلاعات";
        }

        internal bool IsSetForm { get; set; }
        internal string Url { get; set; }
        internal string HaveAccessToArea { get; set; }
        internal string HaveAccessToController { get; set; }
        internal string HaveAccessToAction { get; set; }
        internal Dictionary<string, string> HiddenInput { get; set; }
        internal string GoToPage { get; set; }
        internal bool IsDisabled { get; set; }
        internal bool IsComplex { get; set; }
        internal string Prefix { get; set; }
        internal string SubmitText { get; set; }
        internal EGridColorType SubmitColor { get; set; }

        internal string UpdateTarget { get; set; }
        internal string OnSuccess { get; set; }
        internal string OnFailure { get; set; }
        internal string OnComplete { get; set; }
        internal string OnBegin { get; set; }
        internal string Loading { get; set; }

    }
}