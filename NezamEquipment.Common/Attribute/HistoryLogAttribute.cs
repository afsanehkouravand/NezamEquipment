using System;
using System.Web.Mvc;

namespace NezamEquipment.Common.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HistoryLogAttribute : FilterAttribute
    {
    }
}
