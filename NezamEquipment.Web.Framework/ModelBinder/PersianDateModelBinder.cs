using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.Normalization;

namespace NezamEquipment.Web.Framework.ModelBinder
{
    public class PersianDateModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(valueResult?.AttemptedValue))
                {
                    var value = valueResult.AttemptedValue.PersianNumberToEnglish();
                    if (Regex.IsMatch(value, "\\d{4}/\\d{1,2}/\\d{1,2}"))
                    {
                        var parts = value.Split('/'); //ex. 1391/1/19
                        if (parts.Length != 3) return null;
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        int day = int.Parse(parts[2]);
                        actualValue = new DateTime(year, month, day, new PersianCalendar());
                    }
                    else if ((Regex.IsMatch(value, "\\d{1,2}/\\d{1,2}/\\d{4}")))
                    {
                        actualValue = DateTime.Parse(value.Replace('/', '-'));
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            //throw new Exception("فرمت تاریخ ارسال شده صحیح نمی باشد");
                        }
                    }
                }
            }
            catch (FormatException e)
            {
                e.LogErrorForElmah();
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}