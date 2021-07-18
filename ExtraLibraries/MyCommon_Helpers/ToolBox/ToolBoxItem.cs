namespace MyCommon.Helpers.ToolBox
{
    public class ToolBoxItem
    {
        internal bool IsDataInfo { get; set; }
        internal string DataInfoText { get; set; }
        internal string DataInfoValue { get; set; }

        internal bool IsButton { get; set; }
        internal ToolBoxButton Button { get; set; }

        internal bool IsRadioGroup { get; set; }
        public ToolBoxRadioGroup RadioGroup { get; set; }
    }
}