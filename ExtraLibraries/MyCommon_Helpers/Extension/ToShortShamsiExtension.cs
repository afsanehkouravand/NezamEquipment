using System;
using System.Globalization;

namespace MyCommon.Helpers.Extension
{
    internal static class ToShortShamsiExtension
    {
        internal static string ToShortShamsi(this DateTime dateTime)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)}";
        }

    }
}
