namespace MyCommon.Helpers.ToolBox
{
    public static class ToolBoxRadioExtension
    {
        public static ToolBoxRadioGroup AddRadio(this ToolBoxRadioGroup data, string text, string value, 
            EToolBoxColorType color = EToolBoxColorType.Default, bool isChecked = false)
        {
            data.RadioGroups.Add(new ToolBoxRadio
            {
                Value = value,
                Text = text,
                Color = color,
                IsChecked = isChecked
            });

            return data;
        }

        public static ToolBoxRadioGroup AddClass(this ToolBoxRadioGroup data, string @class)
        {
            data.CssClass = @class;
            return data;
        }
    }
}