using System.Web;

namespace NezamEquipment.Common.Extension
{
    public static class MapPathExtension
    {
        /// <summary>
        /// گرفتن آدرس مپ شده
        /// </summary>
        /// <param name="path">آدرس</param>
        /// <returns></returns>
        public static string MapPath(this string path)
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Server.MapPath(path);

            return HttpRuntime.AppDomainAppPath + path.Replace("~", string.Empty).Replace('/', '\\');
        }
    }
}
