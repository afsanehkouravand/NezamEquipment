using System;
using System.Web.Mvc;

namespace NezamEquipment.Common.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConvertToBase64StringAttribute : FilterAttribute
    {
    }
}
