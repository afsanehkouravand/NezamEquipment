namespace MyCommon.Helpers.Grid
{
    public static class GridFormExtension
    {
        public static GridForm SetIsDisabled(this GridForm data, bool disabled)
        {
            data.IsDisabled = disabled;
            return data;
        }
        public static GridForm SetGoToPage(this GridForm data, string goToPage)
        {
            data.GoToPage = goToPage;
            return data;
        }
        public static GridForm SetHiddenInput(this GridForm data, string key, string value)
        {
            data.HiddenInput.Add(key, value);
            return data;
        }
        public static GridForm SetIsComplex(this GridForm data, string prefix)
        {
            data.IsComplex = true;
            data.Prefix = prefix;
            return data;
        }
        public static GridForm SetSubmitColor(this GridForm data, EGridColorType color)
        {
            data.SubmitColor = color;
            return data;
        }
        public static GridForm SetSubmitText(this GridForm data, string text)
        {
            data.SubmitText = text;
            return data;
        }
        public static GridForm SetAjaxData(this GridForm data, string ajaxBegin = "OnBegin", 
            string ajaxComplete = "OnComplete", string ajaxFailure = "OnFailure",
            string ajaxSuccess = "OnSuccess", string ajaxLoading = "loading",
            string updateTarget = null)
        {
            data.UpdateTarget = updateTarget;
            data.OnBegin = ajaxBegin;
            data.OnComplete = ajaxComplete;
            data.OnFailure = ajaxFailure;
            data.OnSuccess = ajaxSuccess;
            data.Loading = ajaxLoading;

            return data;
        }

    }
}