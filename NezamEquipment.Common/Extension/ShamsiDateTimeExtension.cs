using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NezamEquipment.Common.Extension
{
    public static class ShamsiDateTimeExtension
    {
        public static string ShamsiDateTime(this DateTime dateTime, bool? withTime = true)
        {
            var endDate = DateTime.Now;
            var pc = new PersianCalendar();

            var result = new StringBuilder("در ");

            var ts = endDate.Subtract(dateTime);
            var years = (ts.Days / 365);
            var months = 0;

            do
            {
                for (int i = 0; i <= 12; i++)
                {
                    if (endDate.Subtract(dateTime.AddYears(years).AddMonths(i)).Days > 0)
                        months = i;
                    else
                        break;
                }

                if (months > 12) 
                    years = years + 1;

            } while (months > 12);

            var days = endDate.Subtract(dateTime.AddYears(years).AddMonths(months)).Days;


            if (years > 0)
            {
                result.AppendFormat("{0} سال", years);

                if (months > 0)
                    result.AppendFormat(" و {0} ماه", months);

                result.Append(" قبل، ");
            }

            else if (months > 0)
                result.AppendFormat("{0} ماه قبل، ", months);

            else if (days > 1)
                result.AppendFormat("{0} روز قبل، ", days);

            else
            {
                result.Append(dateTime.Date.ToShortDateString() != DateTime.Now.ToShortDateString()
                    ? "دیروز " : "امروز ");

                if (withTime != null && withTime == true)
                {
                    result.AppendFormat("{0}، {1}", GetDayName(pc.GetDayOfWeek(dateTime)), GetTime(dateTime));
                }
                else
                {
                    result.AppendFormat("{0}", GetDayName(pc.GetDayOfWeek(dateTime)));
                }

                return result.ToString();
            }

            if (withTime != null && withTime == true)
            {
                result.AppendFormat("{0} {1} {2} {3}، {4}", GetDayName(pc.GetDayOfWeek(dateTime)),
                    pc.GetDayOfMonth(dateTime), GetMonthName(pc.GetMonth(dateTime)), pc.GetYear(dateTime), GetTime(dateTime));
            }
            else
            {
                result.AppendFormat("{0} {1} {2} {3}", GetDayName(pc.GetDayOfWeek(dateTime)),
                    pc.GetDayOfMonth(dateTime), GetMonthName(pc.GetMonth(dateTime)), pc.GetYear(dateTime));
            }


            return result.ToString();
        }
        public static string GetTime(this DateTime lastdate)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("ساعت {0}:{1}", pc.GetHour(lastdate).ToString("00"),
                pc.GetMinute(lastdate).ToString("00"));
        }
        public static string GetDayName(this DayOfWeek day)
        {
            var dicDayOfWeek = new Dictionary<DayOfWeek, string>
            {
                {DayOfWeek.Friday, "جمعه"},
                {DayOfWeek.Monday, "دوشنبه"},
                {DayOfWeek.Saturday, "شنبه"},
                {DayOfWeek.Sunday, "یکشنبه"},
                {DayOfWeek.Thursday, "پنج شنبه"},
                {DayOfWeek.Tuesday, "سه شنبه"},
                {DayOfWeek.Wednesday, "چهارشنبه"}
            };
            return dicDayOfWeek[day];
        }
        public static string GetMonthName(this int month)
        {
            var divMonthInYear = new Dictionary<int, string>
            {
                {1,"فروردین"},{2,"اردیبهشت"},{3,"خرداد"},
                {4,"تیر"},{5,"مرداد"},{6,"شهریور"},
                {7,"مهر"},{8,"آبان"},{9,"آذر"},
                {10,"دی"},{11,"بهمن"},{12,"اسفند"}
            };
            return divMonthInYear[month];
        }

        public static string ToShortShamsi(this DateTime dateTime, bool? withTime = true)
        {
            if (dateTime.Year == 0001)
            {
                return string.Empty;
            }
            var pc = new PersianCalendar();

            return (withTime != null && withTime == true)
                ? string.Format("{0}/{1}/{2} {3}:{4}", pc.GetYear(dateTime), pc.GetMonth(dateTime),
                    pc.GetDayOfMonth(dateTime), pc.GetHour(dateTime), pc.GetMinute(dateTime))
                : string.Format("{0}/{1}/{2}", pc.GetYear(dateTime), pc.GetMonth(dateTime),
                    pc.GetDayOfMonth(dateTime));
        }

        public static IList<string> ToShortShamsiArray(this DateTime dateTime)
        {
            var pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}/{3}/{4}", pc.GetYear(dateTime), pc.GetMonth(dateTime),
                pc.GetDayOfMonth(dateTime), pc.GetHour(dateTime), pc.GetMinute(dateTime)).Split(Convert.ToChar("/"));
        }

        public static string FixTime(this string data)
        {
            return data.Contains(".") ? data.Split(Convert.ToChar("."))[0] : data;
        }

        public static string ShamsiDefaultDateTime(this DateTime dateTime, bool? withTime = true)
        {
            var endDate = DateTime.Now;
            var pc = new PersianCalendar();

            var result = new StringBuilder("");

            var ts = endDate.Subtract(dateTime);
            var years = (ts.Days / 365);
            var months = 0;

            do
            {
                for (int i = 0; i <= 12; i++)
                {
                    if (endDate.Subtract(dateTime.AddYears(years).AddMonths(i)).Days > 0)
                        months = i;
                    else
                        break;
                }

                if (months > 12)
                    years = years + 1;

            } while (months > 12);

            //var days = endDate.Subtract(dateTime.AddYears(years).AddMonths(months)).Days;

            //if (years > 0)
            //{
            //    result.AppendFormat("{0} سال", years);

            //    if (months > 0)
            //        result.AppendFormat(" و {0} ماه", months);

            //    result.Append(" قبل، ");
            //}

            //else if (months > 0)
            //    result.AppendFormat("{0} ماه قبل، ", months);

            //else if (days > 1)
            //    result.AppendFormat("{0} روز قبل، ", days);

            //else
            //{
            //    result.Append(dateTime.Date.ToShortDateString() != DateTime.Now.ToShortDateString()
            //        ? "دیروز " : "امروز ");

            //    if (withTime != null && withTime == true)
            //    {
            //        result.AppendFormat("{0}، {1}", GetDayName(pc.GetDayOfWeek(dateTime)), GetTime(dateTime));
            //    }
            //    else
            //    {
            //        result.AppendFormat("{0}", GetDayName(pc.GetDayOfWeek(dateTime)));
            //    }

            //    return result.ToString();
            //}

            if (withTime != null && withTime == true)
            {
                result.AppendFormat("{0} {1} {2} {3}، {4}", GetDayName(pc.GetDayOfWeek(dateTime)),
                    pc.GetDayOfMonth(dateTime), GetMonthName(pc.GetMonth(dateTime)), pc.GetYear(dateTime), GetTime(dateTime));
            }
            else
            {
                result.AppendFormat("{0} {1} {2} {3}", GetDayName(pc.GetDayOfWeek(dateTime)),
                    pc.GetDayOfMonth(dateTime), GetMonthName(pc.GetMonth(dateTime)), pc.GetYear(dateTime));
            }


            return result.ToString();
        }

    }
}
