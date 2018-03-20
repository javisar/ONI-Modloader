namespace ONI_Common
{
    using ONI_Common.IO;
    using System.Collections.Generic;

    public static class CommonState
    {
        public static List<ExitCode> ExitCodes = new List<ExitCode>
                                                 {
                                                 new ExitCode { Code = 0, Message = "Success" },
                                                 new ExitCode
                                                 {
                                                 Code    = 1,
                                                 Message = "Invalid arguments"
                                                 },
                                                 new ExitCode
                                                 {
                                                 Code    = 2,
                                                 Message = "View init error"
                                                 },
                                                 };

        internal static Logger Logger { get; set; } = new IO.Logger(Paths.CommonLogFileName);
    }
}