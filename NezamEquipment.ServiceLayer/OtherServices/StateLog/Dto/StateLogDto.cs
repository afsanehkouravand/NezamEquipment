using System;

namespace NezamEquipment.ServiceLayer.OtherServices.StateLog.Dto
{
    public class StateLogDto
    {
        public long Id { get; set; }
        public DateTime? StateLogDate { get; set; }
        public string StateLogDateStr { get; set; }
        public string StateLogTime { get; set; }
        public Guid? FkTransfer { get; set; }
        public long? StateLogStateCode { get; set; }
        public string StateLogTitle { get; set; }
        public string StateLogText { get; set; }
        public Guid? FkUser { get; set; }
        public string Username { get; set; }
        public string Userip { get; set; }
        public int? StateLogType { get; set; }
    }
}
