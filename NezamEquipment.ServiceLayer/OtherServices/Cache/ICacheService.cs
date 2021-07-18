using System;

namespace NezamEquipment.ServiceLayer.OtherServices.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// ذخیره اطلاعات در کش
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        /// <param name="savedItem">اطلاعات برای کش</param>
        /// <param name="absoluteExpiration">زمان پایان عمر کش</param>
        void SaveToCache(string cacheKey, object savedItem, DateTime absoluteExpiration);

        /// <summary>
        /// گرفتن کش با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        /// <returns></returns>
        T GetFromCache<T>(string cacheKey) where T : class;

        /// <summary>
        /// گرفتن کش با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        object GetFromCache(string cacheKey);

        /// <summary>
        /// حذف کش موجود با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        void RemoveFromCache(string cacheKey);

        /// <summary>
        /// بررسی وجود کش با کلید مخصوص
        /// </summary>
        /// <param name="cacheKey">کلید مخصوص</param>
        /// <returns>آره یا نه</returns>
        bool IsInCache(string cacheKey);
    }
}