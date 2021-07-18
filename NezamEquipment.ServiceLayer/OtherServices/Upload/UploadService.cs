using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.Security;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Enum;

namespace NezamEquipment.ServiceLayer.OtherServices.Upload
{
    public class UploadService : IUploadService
    {
        public string UploadByBase64(UploadByBase64Dto dto)
        {
            var byteArray = GetByteFromBase64String(dto.Base64);
            var outPutPath = string.Empty;

            if (dto.FolderType != UploadFolderType.None)
                outPutPath = GetOutPutPath(dto.FolderType);

            var fileName = Md5HashExtention.Md5Hash() + Path.GetExtension(dto.Name);
            var filePath = outPutPath.TrimEnd(Convert.ToChar("/")) + "/" + fileName;

            File.WriteAllBytes(filePath, byteArray);

            if (File.Exists(filePath))
            {
                return fileName;
            }

            return string.Empty;
        }

        public string UploadImageByBase64(UploadByBase64Dto dto)
        {

            var byteArray = GetByteFromBase64String(dto.Base64);

            Stream stream = new MemoryStream(byteArray);
            try
            {
                using (var bitmap = new Bitmap(stream))
                {
                }
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
                return string.Empty;
            }

            var outPutPath = string.Empty;

            if (dto.FolderType != UploadFolderType.None)
                outPutPath = GetOutPutPath(dto.FolderType);

            var fileName = Md5HashExtention.Md5Hash() + Path.GetExtension(dto.Name);
            var filePath = outPutPath.TrimEnd(Convert.ToChar("/")) + "/" + fileName;

            File.WriteAllBytes(filePath, byteArray);

            if (File.Exists(filePath))
            {
                return fileName;
            }

            return string.Empty;
        }
        public string UploadImageAndPdfByBase64(UploadByBase64Dto dto)
        {

            var fileType = Path.GetExtension(dto.Name);

            if (fileType == null)
                return string.Empty;

            var ext = new List<string>()
            {
                ".jpg",
                ".jpge",
                ".doc",
                ".docx",
                ".png",
                ".pdf",
                ".jpeg"
            };

            if (ext.All(x => x.ToLower() != fileType.ToLower()))
                return string.Empty;

            var byteArray = GetByteFromBase64String(dto.Base64);
            if (fileType == ".jpg" || fileType == ".jpge" || fileType == ".png")
            {

                Stream stream = new MemoryStream(byteArray);
                try
                {
                    using (var bitmap = new Bitmap(stream))
                    {
                    }
                }
                catch (Exception e)
                {
                    e.LogErrorForElmah();
                    return string.Empty;
                }
            }
            var outPutPath = string.Empty;

            if (dto.FolderType != UploadFolderType.None)
                outPutPath = GetOutPutPath(dto.FolderType);

            var fileName = Md5HashExtention.Md5Hash() + Path.GetExtension(dto.Name);
            var filePath = outPutPath.TrimEnd(Convert.ToChar("/")) + "/" + fileName;

            File.WriteAllBytes(filePath, byteArray);

            if (File.Exists(filePath))
            {
                return fileName;
            }

            return string.Empty;
        }

        public bool DeleteUploadFile(string name, UploadFolderType folerType)
        {
            if (folerType == UploadFolderType.None)
                return false;
            var folderpath = "FolderPath." + folerType.ToString();
            var outPutPath = GetOutPutPath(folerType);
            var filePath1 = (outPutPath + name);
            if (File.Exists(filePath1))
            {
                File.Delete(filePath1);
            }

            return !File.Exists(filePath1);
        }
       

        #region Private GetByteFromBase64String

        private static byte[] GetByteFromBase64String(string imageString)
        {
            if (imageString != null)
            {
                string[] arrayBase64 = imageString.Split(',');

                var selectBase64 = arrayBase64.Length >= 2 ? arrayBase64[1] : imageString;

                if (selectBase64 != null)
                {
                    byte[] outBytes = Convert.FromBase64String(selectBase64);
                    return outBytes;
                }
            }
            return null;
        }

        #endregion

        #region Private GetOutPutPath

        private string GetOutPutPath(UploadFolderType folerType)
        {
            var pathFromWebConfig = string.Empty;
            switch (folerType)
            {
                case UploadFolderType.FactorDoc:
                    pathFromWebConfig = ConfigurationManager.AppSettings["FolderPath.FactorDoc"];
                    break;
               
            }

            var folder = $"~/{pathFromWebConfig}/".MapPath();
            Directory.CreateDirectory(folder);

            var permission = new FileIOPermission(FileIOPermissionAccess.Write, folder);
            var permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.AddPermission(permission);
            if (!permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            {
                throw new Exception($"اجازه خواندن/نوشتن روی پوشه {folder} وجود ندارد.");
            }

            return folder;
        }

        #endregion

    }
}
