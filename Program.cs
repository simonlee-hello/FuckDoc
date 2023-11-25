using FuckDoc.Option;
using FuckDoc.Utils;

namespace FuckDoc
{
    public static class Program
    {
        // TODO: 内存问题
        private static void Main()
        {
            var info = new FlagInfo();
            info.InitFlag();
            LogUtil.PrintFlagInfo(info);
            Process.Process.WalkAndProcess(info);
            // Test2();
            
        }

        // static void Test()
        // {
        //     var r = Utils.FileUtil.IsFileExists("/Users/simon/Desktop/tmp/test/test.exe");
        //     Console.WriteLine(r);
        //     var size =Utils.FileUtil.GetTotalSize(new[] { "/Users/simon/Desktop/tmp/test/test.exe" });
        //     Console.WriteLine(Utils.TransformUtil.BytesToSize(size));
        //     // Utils.FileUtil.DeleteFile("/Users/simon/Desktop/tmp/test/test.exe");
        //     string result = Utils.TransformUtil.TransformSlash("\\users\\alsdf\\asdf");
        //     Console.WriteLine(result);
        //     var convertStringToList = Utils.TransformUtil.ConvertStringToList("a,b,c,d,1234");
        //     foreach (var s in convertStringToList)
        //     {
        //         Console.WriteLine(s);    
        //     }
        //
        //     var stringToHashSet = Utils.TransformUtil.StringToHashSet("pdf,doc,xlsx");
        //     foreach (var s in stringToHashSet)
        //     {
        //         Console.WriteLine(s);    
        //     }
        // }
        
    }
}