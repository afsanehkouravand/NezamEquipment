using System;
using System.Threading.Tasks;
using NezamEquipment.Common.Extension;
using NezamEquipment.ServiceLayer.Entity.Other.SmsLog;
using NezamEquipment.Web.DependencyResolution;
using DNTScheduler;

namespace NezamEquipment.Web.DNTScheduler.JobsTask
{
    public class SmsLogTask : ScheduledTaskTemplate
    {
        /// <summary>
        /// If you have multiple jobs at the same time, this value indicates the order of their execution.
        /// </summary>
        public override int Order => 2;

        public override bool RunAt(DateTime utcNow)
        {
            if (IsShuttingDown || Pause)
                return false;

            return utcNow.Second % 30 == 0;
        }

        public override async Task RunAsync()
        {
            //if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SendSms"]))
            //    return;
            
            try
            {
                var service = ObjectFactory.Container.GetInstance<ISmsLogService>();
                await service.SendScheduleAsync();
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
            }
        }

        public override string Name => "ارسال پیامک ها";
    }
}