using System;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Transfer.TransferMessage.Dto;
using NezamEquipment.ServiceLayer._Identity.User;
using AutoMapper;
using EFSecondLevelCache;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.TransferMessage
{
    public class TransferMessageService : BaseSaveDbResult ,ITransferMessageService
    {
        private readonly IDbSet<DomainClasses.Entity.Transfer.TransferMessage> _transferMessages;
        private readonly IDbSet<User> _users;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;

        public TransferMessageService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper,
            IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _transferMessages = _unitOfWork.Set<DomainClasses.Entity.Transfer.TransferMessage>();
            _users = _unitOfWork.Set<User>();
        }

        public DomainClasses.Entity.Transfer.TransferMessage New(string message)
        {
            var req = HttpContext.Current.Request;
            var m = new DomainClasses.Entity.Transfer.TransferMessage()
            {
                CreatedOn = DateTime.Now,
                Message = message,
                ClientIp = req.UserHostAddress,
                MachineIp = req.ServerVariables["HTTP_X_FORWARDED_FOR"],
                Agent = req.UserAgent,
                Browser = $"{req.Browser.Browser} {req.Browser.Version}",
                MacAddress = GetMacAddress(),
            };

            if (_userManager.GetCurrentUserId() != Guid.Empty)
            {
                m.CreatedByUserId = _userManager.GetCurrentUserId();
            }

            return m;
        }

        public async Task<TransferMessageDto> GetAsync(Guid? transferId = null,
            bool? suspension = null, bool? verification = null, TransferStep? step = null)
        {
            var e = Dto().OrderByDescending(x=>x.CreatedOn).AsQueryable();

            if (transferId != null)
                e = e.Where(x => x.TransferId == transferId).AsQueryable();

            if (suspension != null)
                e = e.Where(x => x.Suspension == suspension).AsQueryable();

            if (verification != null)
                e = e.Where(x => x.Verification == verification).AsQueryable();

            if (step != null)
                e = e.Where(x => x.Step == step).AsQueryable();

            return await e.Cacheable().FirstOrDefaultAsync();
        }

        public async Task<GetAllTupleResult<TransferMessageDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            Guid? transferId = null)
        {
            var e = Dto();

            if (transferId != null)
                e = e.Where(x => x.TransferId == transferId).AsQueryable();

            return await e.ToGetAllTupleResult<TransferMessageDto, TransferMessageDto>(getAllTupleDto, _mapper);
        }

        private IQueryable<TransferMessageDto> Dto()
        {
            return (from transferMessage in _transferMessages
                join user in _users on transferMessage.CreatedByUserId equals user.Id into userJoin
                from user in userJoin.DefaultIfEmpty()
                select new TransferMessageDto()
                {
                    Id = transferMessage.Id,
                    TransferId = transferMessage.TransferId,
                    CreatedByUserFullname = user.FirstName + " " + user.LastName,
                    CreatedOn = transferMessage.CreatedOn,
                    CreatedByUserId = transferMessage.CreatedByUserId,
                    Message = transferMessage.Message,
                    CreatedByUserPosition = user.Position,
                    Step = transferMessage.Step,
                    Verification = transferMessage.Verification,
                    Suspension = transferMessage.Suspension,
                    MessageType = transferMessage.MessageType,
                }).AsNoTracking();
        }

        private static string GetMacAddress()
        {
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            var sMacAddress = string.Empty;
            foreach (var adapter in nics)
            {
                if (sMacAddress == string.Empty)
                {
                    //var properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

    }
}
