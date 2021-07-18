using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Info
{
    public class InfoDataOption
    {
        private readonly InfoOption _option;

        public InfoDataOption(InfoOption options)
        {
            _option = options;
        }

        public void SetClass(string attrClass)
        {
            _option.PanelClass = attrClass;
        }
        public void SetTitle(string title)
        {
            _option.Title = title;
        }
        public void SetColor(EInfoColorType color)
        {
            _option.Color = color;
        }
        public void SetColor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, IDictionary<string, EInfoColorType> colorRoles)
        {
            if (colorRoles == null)
                return;

            var propertyName = expression.GetPropertyName();

            var modelType = model.GetType();

            var propertyInfo = modelType.GetProperty(propertyName);
            if (propertyInfo == null)
                return;

            var value = propertyInfo.GetValue(model, null);
            var valueStr = value?.ToString() ?? string.Empty;

            foreach (var item in colorRoles)
            {
                if (valueStr == item.Key)
                {
                    _option.Color = item.Value;
                    break;
                }
            }
        }
        public void SetColume(EInfoColumeType colume)
        {
            _option.Colume = colume;
        }
        public void SetHaveAccessTo(object haveAccessTo = null, bool enabled = true)
        {
            _option.EnabledHaveAccessTo = enabled;
            _option.HaveAccessTo = haveAccessTo;
            _option.HaveAccessToRoleAccess = new List<RoleAccessDto>();

            if (haveAccessTo == null)
                return;

            var haveAccessToType = haveAccessTo.GetType();

            var propertyInfoIsAdmin = haveAccessToType.GetProperty("IsAdmin");
            if (propertyInfoIsAdmin != null)
            {
                var isAdmin = propertyInfoIsAdmin.GetValue(haveAccessTo, null);
                _option.HaveAccessToIsAdmin = isAdmin != null && bool.Parse(isAdmin.ToString());
            }

            var propertyInfoRoleAccess = haveAccessToType.GetProperty("RoleAccess");
            if (propertyInfoRoleAccess != null)
            {
                var roleAccess = propertyInfoRoleAccess.GetValue(haveAccessTo, null);
                if (roleAccess != null)
                {
                    var roleAccessCast = (IList)roleAccess;
                    foreach (var item in roleAccessCast)
                    {
                        _option.HaveAccessToRoleAccess.Add(item.CustomCastToRoleAccess());
                    }
                }
            }
        }

        public void SetLabelclass(string labelclass)
        {
            _option.Labelclass = labelclass;
        }

    }
}