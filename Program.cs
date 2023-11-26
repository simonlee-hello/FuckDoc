// using FuckDoc.Option;
// using FuckDoc.Utils;

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
            Process.Process.WalkAndProcess(info);
        }
    }
}