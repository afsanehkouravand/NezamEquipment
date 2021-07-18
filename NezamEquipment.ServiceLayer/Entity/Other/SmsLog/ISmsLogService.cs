using System;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;
using NezamEquipment.ServiceLayer.Entity.Other.SmsLog.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Other.SmsLog
{
    public interface ISmsLogService
    {
        Task<DbResult> SendAsync(string userPhone, string message);
        Task<DbResult> SendRenterReqisterAsync(string userPhone, string name, string code, string trackingcode);
        Task<DbResult> SendApplicantReqisterAsync(string userPhone, string name, string code);
        Task<DbResult> SendDuesTransferAsync(string userPhone, string name, string code);
        Task<bool> SendScheduleAsync();
        Task<SmsLogDto> GetAsync(Guid? id = null, bool? mustSend = null, SmsLogRetvalStatus? smsLogRetvalStatus = null,
            SmsLogStatus? smsLogStatus = null);
        Task<GetAllTupleResult<SmsLogDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string phoneNumber = null, DateTime? fromDate = null, DateTime? toDate = null);

        Task<DbResult> SendForChangePassword(string userPhone, string name, string code);
        Task<DbResult> SendNezamEmployeReqisterAsync(string userPhone, string name, string code);
        Task<DbResult> SendRedirectTransferAsync(string userPhone, string name);
        Task<DbResult> SendConfirmedTransferAsync(string userPhone, string code);
    }
}