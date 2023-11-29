using System;
using FuckDoc.Option;

namespace FuckDoc.Utils
{
    public static class LogUtil
    {
        

        public static void PrintFlagInfo(FlagInfo info)
        {
            // 输出前带日志级别的格式化输出
            LogIfNotNull("Debug", FormatMessage("Size", info.Size));
            LogIfNotNull("Debug", FormatMessage("OutputPath", info.OutputPath));
            LogIfNotNull("Debug", FormatMessage("AfterDateStr", info.AfterDateStr));
            LogIfNotNull("Debug", FormatMessage("RootPath", info.RootPath));
            LogIfNotNull("Debug", FormatMessage("SkipDirs", info.SkipDirs));
            LogIfNotNull("Debug", FormatMessage("FileName", info.FileName));
            LogIfNotNull("Debug", FormatMessage("Keyword", info.Keyword));
            LogIfNotNull("Debug", FormatMessage("Extension", info.Extension));
        }
        
        private static void LogIfNotNull(string logLevel, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"[{logLevel}] {message}");
            }
        }
        private static string FormatMessage(string property, string value)
        {
            return !string.IsNullOrEmpty(value) ? $"{property}: {value}" : null;
        }
    }
    
}