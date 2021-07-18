using System;
using System.Diagnostics;
using System.Net;
using NezamEquipment.Common.Extension;
using DNTScheduler;
using NezamEquipment.Web.DNTScheduler.JobsTask;

namespace NezamEquipment.Web.DNTScheduler
{
    public static class ScheduledTasksRegistry
    {
        public static void Init()
        {
            ScheduledTasksCoordinator.Current.AddScheduledTasks(
                new DeleteOldErrorLogsTask(),new SmsLogTask());

            ScheduledTasksCoordinator.Current.OnUnexpectedException = (exception, scheduledTask) =>
            {
                //todo: log the exception.
                Trace.WriteLine(scheduledTask.Name + ":" + exception.Message);
            };

            ScheduledTasksCoordinator.Current.Start();
        }

        public static void End()
        {
            ScheduledTasksCoordinator.Current.Dispose();
        }

        public static void WakeUp(string pageUrl)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    client.Headers.Add("User-Agent", "ScheduledTasks 1.0");
                    client.DownloadData(pageUrl);
                }
            }
            catch (Exception ex)
            {
                ex.LogErrorForElmah();

                //todo: log ex
                Trace.WriteLine(ex.Message);
            }
        }
    }
}