using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.ServiceLayer;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting;
using NezamEquipment.ServiceLayer.OtherServices.UtilityService;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;
using NezamEquipment.ServiceLayer.Entity.Other.SmsLog.Dto;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;

namespace NezamEquipment.ServiceLayer.Entity.Other.SmsLog
{
    public class SmsLogService : BaseSaveDbResult, ISmsLogService
    {
        private const string Username = "ictarvand";
        private const string Password = "Az@13611366269";
        private const string FromNumber = "50001414";

        private readonly IDbSet<DomainClasses.Entity.Other.SmsLog.SmsLog> _smsLogs;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;
        private readonly IUtilityService _utilityService;

        public SmsLogService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            ISettingService settingService,
            IMapper mapper, 
            IUtilityService utilityService)
        {
            _unitOfWork = unitOfWork;
            _smsLogs = _unitOfWork.Set<DomainClasses.Entity.Other.SmsLog.SmsLog>();
            _settingService = settingService;
            _mapper = mapper;
            _utilityService = utilityService;
        }

        public async Task<DbResult> SendAsync(string userPhone, string message)
        {
            var smsLog = SaveButNotSend(userPhone, message);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");

            _smsLogs.Add(smsLog);

            return await SaveDbResult(_unitOfWork, smsLog);
        }

        public async Task<DbResult> UpdateAsync(DomainClasses.Entity.Other.SmsLog.SmsLog smslogDto)
        {
            var smslog = await _smsLogs.FirstOrDefaultAsync(x => x.Id == smslogDto.Id);
            if (smslog == null)
                return new DbResult(DbResult.M.NotFound);

            smslog.SmsLogStatus = smslogDto.SmsLogStatus;
            smslog.RetvalStatus = smslogDto.RetvalStatus;
            smslog.RetvalNumber = smslogDto.RetvalNumber;
            smslog.PhoneNumber = smslogDto.PhoneNumber;
            smslog.Message = smslogDto.Message;
            smslog.DeliveryStatus = smslogDto.DeliveryStatus;
            smslog.DeliveryCheckDate = smslogDto.DeliveryCheckDate;
            smslog.MustSend = smslogDto.MustSend;

            _unitOfWork.MarkAsChanged(smslog);

            return await SaveDbResult(_unitOfWork, smslog);
        }

       

        public async Task<DbResult> SendApplicantReqisterAsync(string userPhone, string name, string code)
        {
            var setting = _settingService.Get<SettingSmsXml>();

            var text = FixMessage(setting.ApplicantRegister, Tuple.Create(nameof(name), name),
                Tuple.Create(nameof(code), code));

            var smsLog = SaveButNotSend(userPhone, text);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");

            _smsLogs.Add(smsLog);

            return await SaveDbResult(_unitOfWork, smsLog);
        }
        public async Task<DbResult> SendRenterReqisterAsync(string userPhone, string name, string code, string trackingcode)
        {
            var setting = _settingService.Get<SettingSmsXml>();

            var text = FixMessage(setting.RenterRegister, Tuple.Create(nameof(name), name),
                Tuple.Create(nameof(code), code), Tuple.Create(nameof(trackingcode), trackingcode));

            var smsLog = SaveButNotSend(userPhone, text);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");

            _smsLogs.Add(smsLog);

            return await SaveDbResult(_unitOfWork, smsLog);
        }

        public async Task<DbResult> SendNezamEmployeReqisterAsync(string userPhone, string name, string code)
        {
            var setting = _settingService.Get<SettingSmsXml>();

            var text = FixMessage(setting.ApplicantRegister, Tuple.Create(nameof(name), name),
                Tuple.Create(nameof(code), code));
           
            var smsLog = SaveButNotSend(userPhone, text);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");

            _smsLogs.Add(smsLog);

            return await SaveDbResult(_unitOfWork, smsLog);
        }
        public async Task<DbResult> SendForChangePassword(string userPhone, string name, string code)
        {
            var setting = _settingService.Get<SettingSmsXml>();
            var text = FixMessage(setting.SmsForChangePassword, Tuple.Create(nameof(name), name),
                Tuple.Create(nameof(code), code));
            var smsLog = SaveButNotSend(userPhone,text);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");
            
                _smsLogs.Add(smsLog);
           

            return await SaveDbResult(_unitOfWork, smsLog);
        }
        public async Task<DbResult> SendDuesTransferAsync(string userPhone, string name, string code)
        {
            var setting = _settingService.Get<SettingSmsXml>();

            var text = FixMessage(setting.SmsDuesTransfer, Tuple.Create(nameof(name), name),
                Tuple.Create(nameof(code), code));

            var smsLog = SaveButNotSend(userPhone, text);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");

            _smsLogs.Add(smsLog);

            return await SaveDbResult(_unitOfWork, smsLog);
        }

        public async Task<DbResult> SendConfirmedTransferAsync(string userPhone, string code)
        {
            var setting = _settingService.Get<SettingSmsXml>();

            var text = FixMessage(setting.SmsConfirmedTransfer, Tuple.Create(nameof(code), code));

            var smsLog = SaveButNotSend(userPhone, text);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");

            _smsLogs.Add(smsLog);

            return await SaveDbResult(_unitOfWork, smsLog);
        }

        public async Task<DbResult> SendRedirectTransferAsync(string userPhone, string name)
        {
            var setting = _settingService.Get<SettingSmsXml>();

            var text = FixMessage(setting.SmsRdirectTransfer, Tuple.Create(nameof(name), name));

            var smsLog = SaveButNotSend(userPhone, text);
            if (smsLog == null)
                return new DbResult("خطا در ارسال پیام");

            _smsLogs.Add(smsLog);

            return await SaveDbResult(_unitOfWork, smsLog);
        }


        private DomainClasses.Entity.Other.SmsLog.SmsLog SaveButNotSend(string userPhone, string message)
        {
            var setting = _settingService.Get<SettingSmsXml>();
            var text = message + "\r\n" + setting.Signature;

            var obj = new DomainClasses.Entity.Other.SmsLog.SmsLog
            {
                CreatedOn = DateTime.Now,
                Message = text,
                PhoneNumber = userPhone,
                RetvalNumber = "0",
                RetvalStatus = SmsLogRetvalStatus.Wait,
                SmsLogStatus = SmsLogStatus.Wait,
                MustSend = true,
            };
            
            if (string.IsNullOrWhiteSpace(userPhone) || _utilityService.CheckMobile(userPhone) == false)
            {
                obj.PhoneNumber = "";
                obj.SmsLogStatus = SmsLogStatus.InvalidNumber;
            }

            return obj;
        }


        public async Task<bool> SendScheduleAsync()
        {
           // if (HttpContext.Current.Request.IsLocal)
             //return false;

            var smsLog =
                await _smsLogs.FirstOrDefaultAsync(x => x.MustSend && x.RetvalStatus == SmsLogRetvalStatus.Wait);
            if (smsLog == null)
                return false;
            
            long[] rec = null;
            byte[] status = null;

            try
            {
                int retval;
                using (var sms = new PkSms.Send())
                {
                    retval = sms.SendSms(username: Username, password: Password,
                        to: new[] { smsLog.PhoneNumber.TrimStart(Convert.ToChar("0")) }, from: FromNumber, text: smsLog.Message,
                        isflash: false, udh: "", recId: ref rec, status: ref status);
                }

                smsLog.RetvalNumber = rec[0].ToString();
                smsLog.RetvalStatus = (SmsLogRetvalStatus) status[0];
                smsLog.SmsLogStatus = (SmsLogStatus) retval;
                smsLog.MustSend = false;

                await UpdateAsync(smsLog);

                return true;
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
                return false;
            }
        }

        private static string FixMessage(string message, params Tuple<string, string>[] data)
        {
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            foreach (var tuple in data)
            {
                if (!string.IsNullOrWhiteSpace(tuple.Item2) 
                    && message.IndexOf($"[{tuple.Item1}]", StringComparison.Ordinal) != -1)
                    message = message.Replace($"[{tuple.Item1}]", tuple.Item2);
            }

            return message;
        }

        public async Task<SmsLogDto> GetAsync(Guid? id = null, bool? mustSend = null, SmsLogRetvalStatus? smsLogRetvalStatus = null,
            SmsLogStatus? smsLogStatus = null)
        {
            var e = _smsLogs.AsNoTracking().AsQueryable();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();

            if (mustSend != null)
                e = e.Where(x => x.MustSend == mustSend).AsQueryable();

            if (smsLogRetvalStatus != null)
                e = e.Where(x => x.RetvalStatus == smsLogRetvalStatus).AsQueryable();

            if (smsLogStatus != null)
                e = e.Where(x => x.SmsLogStatus == smsLogStatus).AsQueryable();

            return await e.ProjectTo<SmsLogDto>(_mapper.ConfigurationProvider).Cacheable().FirstOrDefaultAsync();
        }

        public async Task<GetAllTupleResult<SmsLogDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string phoneNumber = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var e = _smsLogs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                e = e.Where(x => x.PhoneNumber == phoneNumber).AsQueryable();

            if (fromDate != null)
                e = e.Where(x => x.CreatedOn >= fromDate).AsQueryable();

            if (toDate != null)
                e = e.Where(x => x.CreatedOn <= toDate).AsQueryable();

            return await e.ToGetAllTupleResult<DomainClasses.Entity.Other.SmsLog.SmsLog, SmsLogDto>(getAllTupleDto, _mapper);
        }


    }
}
