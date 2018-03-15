using System.Collections.Generic;

namespace ONI_Common
{
    public static class State
    {
        public static List<ExitCode> ExitCodes = new List<ExitCode>
        {
            new ExitCode
            {
                Code = 0,
                Message = "Success"
            },
            new ExitCode
            {
                Code = 1,
                Message = "Invalid arguments"
            },
            new ExitCode
            {
                Code = 2,
                Message = "View init error"
            },
        };

        internal static ONI_Common.IO.Logger Logger { get; set; } = new ONI_Common.IO.Logger(Paths.CommonLogFileName);
    }
}
