using System;
using System.Globalization;

namespace NezamEquipment.Common.Extension
{
    public static class DateTimeWeekExtention
    {

        public static string StartDayOfWeek(this DateTime date)
        {
            var regionInfo = RegionInfo.CurrentRegion.Name;

            var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            var fdow = ci.DateTimeFormat.FirstDayOfWeek;
            var today = date.DayOfWeek;

            DateTime sow = new DateTime();

            if (regionInfo == "US")
            {
                var d = -((today - fdow) + 1);
                if (d == -7)
                {
                    sow = date.Date;
                }
                else
                {
                    sow = date.AddDays(d).Date;
                }
            }
            else if (regionInfo == "IR")
            {
                sow = date.AddDays(-((today - fdow) + 7)).Date;
            }

            return sow.ToShortShamsi(false);
        }

        public static string EndDayOfWeek(this DateTime date)
        {
            var regionInfo = RegionInfo.CurrentRegion.Name;

            var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            var fdow = ci.DateTimeFormat.FirstDayOfWeek;
            var today = date.DayOfWeek;

            DateTime sow = new DateTime();

            if (regionInfo == "US")
            {
                var d = -((today - fdow) + 1);
                if (d == -7)
                {
                    sow = date.Date;
                }
                else
                {
                    sow = date.AddDays(d).Date;
                }
            }
            else if (regionInfo == "IR")
            {
                sow = date.AddDays(-((today - fdow) + 7)).Date;
            }

            return sow.AddDays(+6).ToShortShamsi(false);
        }


        public static int GetWeekNumber(this int day)
        {
            var week = 0;

            if (day <= 7)
            {
                week = 1;
            }
            else if (day > 7 && day <= 14)
            {
                week = 2;
            }
            else if (day > 14 && day <= 21)
            {
                week = 3;
            }
            else if (day > 21 && day <= 28)
            {
                week = 4;
            }
            else if (day > 28)
            {
                week = 5;
            }

            return week;
        }

    }
}
