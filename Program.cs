// using FuckDoc.Option;
// using FuckDoc.Utils;

using System;
using System.Diagnostics;
using FuckDoc.Option;
using FuckDoc.Utils;

namespace FuckDoc
{
    public static class Program
    {
        private static void Main()
        {
            var info = new FlagInfo();
            info.InitFlag();
            LogUtil.PrintFlagInfo(info);
            Stopwatch stopwatch = Stopwatch.StartNew();
            Process.Process.WalkAndProcess(info);
            stopwatch.Stop();
            TimeSpan duration = stopwatch.Elapsed;
            Console.WriteLine($"Execution time: {duration}");
        }
    }
}