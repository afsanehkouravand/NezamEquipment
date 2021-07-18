using System;
using System.Threading.Tasks;
using NezamEquipment.Common.Extension;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Base;

namespace NezamEquipment.ServiceLayer.Base
{
    public abstract class BaseSaveDbResult
    {
        /// <summary>
        /// ذخیره اطلاعات به صورت پیش فرض و ثبت خطاهای احتمالی
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="baseEntity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected async Task<DbResult> SaveDbResult(IUnitOfWork<NezamEquipmentDbContext> unitOfWork, 
            BaseEntity baseEntity = null, string message = null)
        {
            try
            {
                if (baseEntity != null && baseEntity.CreatedOn.Year == 0001)
                    baseEntity.CreatedOn = DateTime.Now;
                
                var result = await unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return new DbResult(DbResult.M.CanNotSave);

                if (string.IsNullOrWhiteSpace(message))
                {
                    if (baseEntity != null)
                        message = baseEntity.Id.ToString();
                }

                return new DbResult(true, message);
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
                Console.WriteLine(e);
            }

            return new DbResult(false, "خطا در پردازش اطلاعات");
        }

    }
}
