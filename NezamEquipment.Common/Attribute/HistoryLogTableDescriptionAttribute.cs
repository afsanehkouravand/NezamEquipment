using System;
using System.Web.Mvc;

namespace NezamEquipment.Common.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HistoryLogTableDescriptionAttribute : FilterAttribute
    {
        public string Name;

        public HistoryLogTableDescriptionAttribute(string name)
        {
            Name = name;
        }
    }
}
