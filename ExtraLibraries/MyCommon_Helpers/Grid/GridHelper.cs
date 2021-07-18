using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Grid
{
    public static class GridHelper
    {
        public static GridExtension Grid<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, TModel model, Expression<Func<TModel, TProperty>> expression, string title = "لیست اطلاعات", EGridColorType color = EGridColorType.Default)
        {
            var mType = model.GetType();

            IEnumerable<object> listOfData = null;

            var dataListOfData = mType.GetProperty(expression.GetPropertyName());
            if (dataListOfData != null)
                listOfData = (IEnumerable<object>)dataListOfData.GetValue(model, null);

            return new GridExtension(htmlHelper: htmlHelper, model: model, listOfData: listOfData, title: title, color: color);
        }
    }
}