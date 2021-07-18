using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezamEquipment.Common.Extension
{
    public static class ToMiladiDateExtension
    {
        public static DateTime ToMiladiDate(this string dateTime, bool withTime = true)
        {
            if (dateTime == null)
                return DateTime.Now;

            var getDate = dateTime.Contains(" ") ? dateTime.Split(Convert.ToChar(" "))[0] : dateTime;
            var splitDate = getDate.Split(Convert.ToChar("/"));
            var dic = new Dictionary<string, int>
            {
                {"Year", int.Parse(splitDate[0])},
                {"Month", int.Parse(splitDate[1])},
                {"Day", int.Parse(splitDate[2])},
                {"Hour", 0},
                {"Minute", 0},
                {"Second", 0}
            };

            if (dateTime.Contains(" "))
            {
                var time = dateTime.Split(Convert.ToChar(" "));
                var splitTime = (!string.IsNullOrWhiteSpace(time[1]))
                    ? time[1].Split(Convert.ToChar(":"))
                    : time[2].Split(Convert.ToChar(":"));
                dic["Hour"] = int.Parse(splitTime[0]);
                dic["Minute"] = int.Parse(splitTime[1]);
                dic["Second"] = int.Parse(splitTime[2]);
            }

            var pc = new System.Globalization.PersianCalendar();
            return pc.ToDateTime(dic["Year"], dic["Month"], dic["Day"], dic["Hour"], dic["Minute"], dic["Second"], 0);
        }
    }
}
