using System.Text.RegularExpressions;
using log4net;

namespace DaysOn.Common.Utilities.Extension
{
    public static class Log4NetExtension
    {
        public static void LogErr(this ILog logger, string errorText)
        {
            string result = Regex.Replace(errorText, @"\r\n?|\n", " |||");
            logger.Error(result);
        }

        public static void LogDeb(this ILog logger, string errorText)
        {
            string result = Regex.Replace(errorText, @"\r\n?|\n", " |||");
            logger.Debug(result);
        }

        public static void LogInf(this ILog logger, string errorText)
        {
            string result = Regex.Replace(errorText, @"\r\n?|\n", " |||");
            logger.Info(result);
        }
    }
}
