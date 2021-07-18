using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NezamEquipment.ServiceLayer.OtherServices.UtilityService
{
    public class UtilityService : IUtilityService
    {

        public string Sha1Hash(string inputString)
        {
            var sha1 = SHA1.Create();

            var inputBytes = Encoding.UTF8.GetBytes(inputString);

            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            var hashedString = sb.ToString();

            return hashedString;
        }
        public bool IsValidNationalCode(String nationalCode)
        {
            //در صورتی که کد ملی وارد شده تهی باشد

            if (String.IsNullOrEmpty(nationalCode))
                return false;


            //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
            if (nationalCode.Length != 10)
                return false;

            //در صورتی که کد ملی ده رقم عددی نباشد
            var regex = new System.Text.RegularExpressions.Regex(@"\d{10}");
            if (!regex.IsMatch(nationalCode))
                return false;

            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            //var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            //if (allDigitEqual.Contains(nationalCode)) return false;


            //عملیات شرح داده شده در بالا
            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
            var c = b % 11;

            return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
        }
        public bool CheckMobile(string number)
        {
            if (number.Trim().Length == 11)
            {
                if (number.Substring(0, 2) == "09")
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public string GetIpAddress()
        {
            var context = HttpContext.Current;
            var sIpAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(sIpAddress))
                return context.Request.ServerVariables["REMOTE_ADDR"];

            var ipArray = sIpAddress.Split(',');
            return ipArray[0];
        }
        public bool IsNumber(string data)
        {
            int number;
            return int.TryParse(data, out number);
        }

        public bool CheckDateFormat(string data)
        {
            return (new Regex(@"\d{4}/\d{2}/\d{2}")).IsMatch(data);
        }

        public bool CheckTimeFormat(string data)
        {
            return (new Regex(@"\d{2} : \d{2}")).IsMatch(data);
        }


        public DateTime GetMiladiDate(string dateTime = null, bool withTime = true)
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

        public string GetShamsiDate(DateTime? dateTime = null, bool withTime = true)
        {
            var pc = new System.Globalization.PersianCalendar();
            var dt = dateTime ?? DateTime.Now;
            var date = string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt),
                pc.GetDayOfMonth(dt));

            return withTime
                ? string.Format("{0} {1}:{2}:{3}", date, pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt))
                : date;
        }


        public string ToLatinDigits(string number)
        {
            var numbers = new char[][]
            {
                //"0123456789".ToCharArray(), "۰۱۲۳۴۵۶۷۸۹".ToCharArray()
                "۰۱۲۳۴۵۶۷۸۹".ToCharArray(), "0123456789".ToCharArray()
            };
            for (var x = 0; x <= 9; x++)
            {
                number = number.Replace(numbers[0][x], numbers[1][x]);
            }
            return number;
        }

        public string ToPersianDigits(string number)
        {
            var numbers = new char[][]
            {
                "0123456789".ToCharArray(), "۰۱۲۳۴۵۶۷۸۹".ToCharArray()
                //"۰۱۲۳۴۵۶۷۸۹".ToCharArray(), "0123456789".ToCharArray()
            };
            for (var x = 0; x <= 9; x++)
            {
                number = number.Replace(numbers[0][x], numbers[1][x]);
            }
            return number;
        }

        public DateTime RoundUp(DateTime dt)
        {
            var d = TimeSpan.FromMinutes(10);
            return new DateTime(((dt.Ticks + 1) / d.Ticks) * d.Ticks);
        }

        public Image ResizeImage(byte[] img, int width)
        {
            byte[] resizedImage;
            using (var ms = new MemoryStream(img))
            {
                using (var orginalImage = Image.FromStream(ms))
                {
                    var orginalImageFormat = orginalImage.RawFormat;
                    var orginalImageWidth = orginalImage.Width;
                    var orginalImageHeight = orginalImage.Height;
                    var resizedImageWidth = width; // Type here the width you want
                    var resizedImageHeight = Convert.ToInt32(resizedImageWidth * orginalImageHeight / orginalImageWidth);
                    using (var bitmapResized = new Bitmap(orginalImage, resizedImageWidth, resizedImageHeight))
                    {
                        using (var streamResized = new MemoryStream())
                        {
                            bitmapResized.Save(streamResized, orginalImageFormat);
                            resizedImage = streamResized.ToArray();
                        }
                    }
                }
            }

            using (var msResult = new MemoryStream(resizedImage))
            {
                using (var imgFromStream = Image.FromStream(msResult))
                {
                    return imgFromStream;
                }
            }
        }

    }
}
