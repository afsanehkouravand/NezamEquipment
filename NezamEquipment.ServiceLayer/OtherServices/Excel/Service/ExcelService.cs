using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using OfficeOpenXml;

namespace NezamEquipment.ServiceLayer.OtherServices.Excel.Service
{
    public class ExcelService : IExcelService
    {
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;

        public ExcelService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Dictionary<int, string> Read(string filepath, int skip)
        {
            var dic = new Dictionary<int, string>();

            using (var fsSource = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                var package = new ExcelPackage(fsSource);
                var workSheet = package.Workbook.Worksheets.First();

                for (var j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    var cellValue = workSheet.Cells[skip, j].Value;
                    dic.Add(j, cellValue?.ToString() ?? string.Empty);
                }

            }

            return dic;
        }

        public List<T> Read<T>(string filepath, Dictionary<string, int> colume, int skip) where T : class , new()
        {
            var allExcelData = new List<T>();

            using (var fsSource = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                var package = new ExcelPackage(fsSource);
                var workSheet = package.Workbook.Worksheets.First();

                for (var i = workSheet.Dimension.Start.Row + skip; i <= workSheet.Dimension.End.Row; i++)
                {
                    var data = new T();
                    var dataType = data.GetType();

                    for (var j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                    {
                        var cellValue = workSheet.Cells[i, j].Value;
                        if (cellValue != null)
                        {
                            if (colume.ContainsValue(j))
                            {
                                var dic = colume.FirstOrDefault(c => c.Value == j);
                                var propertyInfo = dataType.GetProperty(dic.Key);
                                if (propertyInfo != null)
                                {
                                    if (propertyInfo.PropertyType == typeof(int))
                                    {
                                        propertyInfo.SetValue(data, int.Parse(cellValue.ToString()), null);
                                    }
                                    else 
                                    {
                                        propertyInfo.SetValue(data, cellValue.ToString(), null);
                                    }
                                }
                            }
                        }
                    }

                    allExcelData.Add(data);
                }

            }

            return allExcelData;
        }

    }
}
