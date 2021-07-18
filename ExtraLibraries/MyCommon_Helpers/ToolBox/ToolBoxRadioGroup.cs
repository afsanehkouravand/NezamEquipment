using System.Collections.Generic;

namespace MyCommon.Helpers.ToolBox
{
    public class ToolBoxRadioGroup
    {
        internal string RadioGroupName { get; set; }
        internal IList<ToolBoxRadio> RadioGroups { get; set; }
        internal string CssClass { get; set; }
    }
}