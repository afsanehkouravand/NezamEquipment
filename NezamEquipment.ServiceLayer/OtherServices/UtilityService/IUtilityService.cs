using System;
using System.Drawing;

namespace NezamEquipment.ServiceLayer.OtherServices.UtilityService
{
    public interface IUtilityService
    {
        string Sha1Hash(string inputString);
        bool IsValidNationalCode(String nationalCode);
        bool CheckMobile(string number);
        bool IsNumber(string data);
        bool CheckDateFormat(string data);
        bool CheckTimeFormat(string data);
        DateTime GetMiladiDate(string dateTime = null, bool withTime = true);
        string GetShamsiDate(DateTime? dateTime = null, bool withTime = true);
        string ToLatinDigits(string number);
        string ToPersianDigits(string number);
        DateTime RoundUp(DateTime dt);
        Image ResizeImage(byte[] img, int width);
        string GetIpAddress();
    }
}