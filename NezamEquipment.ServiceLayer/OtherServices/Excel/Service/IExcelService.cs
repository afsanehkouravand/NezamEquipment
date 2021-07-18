using System.Collections.Generic;
using System.Web;

namespace NezamEquipment.ServiceLayer.OtherServices.Excel.Service
{
    public interface IExcelService
    {
        Dictionary<int, string> Read(string filepath, int skip);
        List<T> Read<T>(string filepath, Dictionary<string, int> colume, int skip) where T : class, new();
    }
}