using System;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.Normalization;

namespace NezamEquipment.Web.Framework.ModelBinder
{
    public class PersianStringModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                if (valueResult != null && !string.IsNullOrWhiteSpace(valueResult.AttemptedValue))
                {
                    actualValue = valueResult.AttemptedValue.PersianNumberToEnglish().RemoveDiacritics();
                }
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }

    }
}