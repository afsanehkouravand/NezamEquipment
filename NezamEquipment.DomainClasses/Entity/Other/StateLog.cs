using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NezamEquipment.DomainClasses.Entity.Other
{
    public class StateLog
    {
        public long Id { get; set; }
        public DateTime? StateLogDate { get; set; }
        public string StateLogDateStr { get; set; }
        public string StateLogTime { get; set; }
        [Index]
        public Guid? FkTransfer { get; set; }
        [Index]
        public long? StateLogStateCode { get; set; }
        public string StateLogTitle { get; set; }
        public string StateLogText { get; set; }
        public Guid? FkUser { get; set; }
        public string Userip { get; set; }
        public int? StateLogType { get; set; }
    }
}
