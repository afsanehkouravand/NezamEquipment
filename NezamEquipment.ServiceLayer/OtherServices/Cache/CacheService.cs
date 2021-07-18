using System;
using System.Runtime.Caching;

namespace NezamEquipment.ServiceLayer.OtherServices.Cache
{
    public class CacheService : ICacheService
    {
        /// <summary>
        /// ذخیره اطلاعات در کش
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        /// <param name="savedItem">اطلاعات برای کش</param>
        /// <param name="absoluteExpiration">زمان پایان عمر کش</param>
        public void SaveToCache(string cacheKey, object savedItem, DateTime absoluteExpiration)
        {
            MemoryCache.Default.Add(cacheKey, savedItem, absoluteExpiration);
        }


        /// <summary>
        /// گرفتن کش با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        /// <returns></returns>
        public T GetFromCache<T>(string cacheKey) where T : class
        {
            return MemoryCache.Default[cacheKey] as T;
        }


        /// <summary>
        /// گرفتن کش با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public object GetFromCache(string cacheKey)
        {
            return MemoryCache.Default[cacheKey];
        }


        /// <summary>
        /// حذف کش موجود با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        public void RemoveFromCache(string cacheKey)
        {
            MemoryCache.Default.Remove(cacheKey);
        }


        /// <summary>
        /// بررسی وجود کش با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        /// <returns>آره یا نه</returns>
        public bool IsInCache(string cacheKey)
        {
            return MemoryCache.Default[cacheKey] != null;
        }
    }
}
