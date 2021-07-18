using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NezamEquipment.Common.Extension
{
    public static class IsValidDateStringExtension
    {
        public static bool IsValidDateString(this string date)
        {
            //if (!Regex.IsMatch(date, "^d{4}/d{1,2}/d{1,2}$"))
            //{
            //    return false;
            //}

            try
            {
                var parts = date.Split('/');
                var day = int.Parse(parts[2]);
                var month = int.Parse(parts[1]);
                var year = int.Parse(parts[0]);

                if (year < 1000 || year > 3000 || month == 0 || month > 12)
                {
                    return false;
                }

                return day > 0 && day <= 31;
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
            }

            return false;
        }
        public static bool IsValidShamsiDateString(this string date)
        {
            var d = date.ToMiladiDate(false);
            if (!Regex.IsMatch(date, "^d{4}/d{1,2}/d{1,2}$"))
            {
                return false;
            }

            try
            {
                var parts = date.Split('/');
                var day = int.Parse(parts[2]);
                var month = int.Parse(parts[1]);
                var year = int.Parse(parts[0]);

                var pc = new PersianCalendar();
                var currentYear = pc.GetYear(DateTime.Now);

                if (year < 1300 || year > currentYear || month == 0 || month > 12)
                {
                    return false;
                }

                return day > 0 && day <= 31;
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
            }

            return false;
        }
    }
}