using System;
using Elmah;

namespace NezamEquipment.Common.Extension
{
    public static class LogErrorForElmahExtension
    {
        public static void LogErrorForElmah(this Exception exception)
        {
            ErrorSignal.FromCurrentContext().Raise(exception);
        }
    }
}