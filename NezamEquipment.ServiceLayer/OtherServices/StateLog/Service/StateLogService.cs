using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NezamEquipment.Common.Extension;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.ServiceLayer;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer.OtherServices.StateLog.Dto;
using NezamEquipment.ServiceLayer.OtherServices.UtilityService;

namespace NezamEquipment.ServiceLayer.OtherServices.StateLog.Service
{
    public class StateLogService : IStateLogService
    {
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IDbSet<DomainClasses.Entity.Other.StateLog> _stateLogs;
        private readonly IDbSet<User> _users;
        private readonly IUtilityService _utilityService;
        private readonly IUserManager _userManager;
        
        public StateLogService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IUtilityService utilityService, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _stateLogs = _unitOfWork.Set<DomainClasses.Entity.Other.StateLog>();
            _users = _unitOfWork.Set<User>();
            _utilityService = utilityService;
            _userManager = userManager;
        }

        public void Add(Guid transferId, TransferStatusNew stateLogStateCode, string stateLogTitle, string stateLogText)
        {
            var datePersian = DateTime.Now.ToShortShamsiArray();

            string userId = _userManager.GetCurrentUserId().ToString();

            _stateLogs.Add(new DomainClasses.Entity.Other.StateLog
            {
                FkUser = !string.IsNullOrWhiteSpace(userId) ? Guid.Parse(userId) : Guid.Empty,
                StateLogStateCode = (int) stateLogStateCode,
                Userip = _utilityService.GetIpAddress(),
                FkTransfer = transferId,
                StateLogDate = DateTime.Now.Date,
                StateLogDateStr =
                    string.Format("{0} {1} {2} {3}", datePersian[0], DateTime.Now.DayOfWeek.GetDayName(), datePersian[2],
                        int.Parse(datePersian[1]).GetMonthName()),
                StateLogTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" +
                               DateTime.Now.Second,
                StateLogTitle = stateLogTitle,
                StateLogText = stateLogText,
            });
        }

        public async Task<DbResult> AddAsync(Guid transferId, TransferStatusNew stateLogStateCode, string stateLogTitle, string stateLogText)
        {
            var datePersian = DateTime.Now.ToShortShamsiArray();

            var userId = _userManager.GetCurrentUserId().ToString();

            _stateLogs.Add(new DomainClasses.Entity.Other.StateLog
            {
                FkUser = !string.IsNullOrWhiteSpace(userId) ? Guid.Parse(userId) : Guid.Empty,
                StateLogStateCode = (int)stateLogStateCode,
                Userip = _utilityService.GetIpAddress(),
                FkTransfer = transferId,
                StateLogDate = DateTime.Now.Date,
                StateLogDateStr =
                    string.Format("{0} {1} {2} {3}", datePersian[0], DateTime.Now.DayOfWeek.GetDayName(), datePersian[2],
                        int.Parse(datePersian[1]).GetMonthName()),
                StateLogTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" +
                               DateTime.Now.Second,
                StateLogTitle = stateLogTitle,
                StateLogText = stateLogText,
            });

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return new DbResult("امکان ذخیره اطلاعات وجود ندارد.");

            return new DbResult(true);
        }

        public async Task<DbResult> DeleteAsync(long id)
        {
            var model = await _stateLogs.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
                return new DbResult("اطلاعات مورد نظر یافت نشد.");

            _stateLogs.Remove(model);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return new DbResult("امکان ذخیره اطلاعات وجود ندارد.");

            return new DbResult(true);
        }


        public async Task<IEnumerable<StateLogDto>> GetAllAsync(Guid? transferId = null)
        {
            var e = (from stateLog in _stateLogs

                   //join user in _users on new { id = stateLog.FkUser.ToString() } equals
                   //  new { id = user.Id } into userJoin
                     //from user in userJoin.DefaultIfEmpty()

                     select new StateLogDto
                     {
                         Id = stateLog.Id,
                         FkTransfer = stateLog.FkTransfer,
                         StateLogDate = stateLog.StateLogDate,
                         FkUser = stateLog.FkUser,
                         StateLogDateStr = stateLog.StateLogDateStr,
                         StateLogStateCode = stateLog.StateLogStateCode,
                         StateLogText = stateLog.StateLogText,
                         StateLogTime = stateLog.StateLogTime,
                         StateLogTitle = stateLog.StateLogTitle,
                         StateLogType = stateLog.StateLogType,
                         Userip = stateLog.Userip,
                        

                         // Username = user.UserName + " " + user.Fullname,
                     }
            ).AsNoTracking().OrderByDescending(x => x.Id).AsQueryable();
            //var e = _stateLogs;

            if (transferId != null)
                e = e.Where(x => x.FkTransfer == transferId).AsQueryable();

            return await e.ToListAsync();
        }

    }
}
