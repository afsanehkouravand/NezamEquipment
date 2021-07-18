using System;
using System.IO;
using System.Linq;
using NezamEquipment.Common.Extension;
using DNTScheduler;

namespace NezamEquipment.Web.DNTScheduler.JobsTask
{
    public class DeleteOldErrorLogsTask : ScheduledTaskTemplate
    {
        /// <summary>
        /// If you have multiple jobs at the same time, this value indicates the order of their execution.
        /// </summary>
        public override int Order => 1;

        public override bool RunAt(DateTime utcNow)
        {
            if (IsShuttingDown || Pause)
                return false;

            var now = utcNow.AddHours(3.5); // 
            return now.Hour == 4 && now.Minute == 30 && now.Second == 1;
        }

        public override void Run()
        {
            if (IsShuttingDown || Pause)
                return;

            var di = new DirectoryInfo("~/App_Data/Elmah".MapPath());
            foreach (var file in di.GetFiles().OrderByDescending(x => x.CreationTime).Skip(1000))
            {
                file.Delete();
            }
        }

        public override string Name => "حذف خطاهای قدیمی هر 24 ساعت";
    }
}