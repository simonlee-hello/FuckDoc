using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FuckDoc.Option
{
    public class FlagInfo
    {
        public string Size { get; private set; }
        public string OutputPath { get; private set; }
        public string AfterDateStr { get; private set; }
        public string RootPath { get; private set; }
        public string SkipDirs { get; private set; }
        public string FileName { get; private set; }
        public string Keyword { get; private set; }
        public string Extension { get; private set; }
        public void InitFlag()
        {
            // 实现 InitFlag 的逻辑
            GetFlag();
            // 如果命令行参数包含 --help 或 -h，则显示帮助信息并退出
            if (Environment.GetCommandLineArgs().Contains("--help") || Environment.GetCommandLineArgs().Contains("-h"))
            {
                ShowHelp();
                Environment.Exit(0);
            }

            // 初始化 RootPath
            if (string.IsNullOrEmpty(RootPath))
            {
                // 获取家目录
                var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                Console.WriteLine($"Current user home path: {homeDir}");
                RootPath = homeDir;
            }
            // 初始化 SkipDirs
            if (string.IsNullOrEmpty(SkipDirs))
            {
                SkipDirs = "C:\\Windows, C:\\Program Files, C:\\Program Files (x86), C:\\inetpub, C:\\Users\\Public";
            }

            // 初始化Extension
            switch (Extension)
            {
                
                case "all":
                    Extension = "pdf,docx,doc,xlsx,xls,csv,pptx,ppt,zip,rar,7z,tar,gz,tgz,bak,bz2,txt";
                    break;
                case "documents":
                    Extension = "pdf,docx,doc,xlsx,xls,csv,pptx,ppt";
                    break;
                case "packages":
                    Extension = "zip,rar,7z,tar,gz,tgz,bak,bz2";
                    break;
                case "images":
                    Extension = "jpg,jpeg,png,gif,bmp";
                    break;
                case "videos":
                    Extension = "mp4,mkv,avi,mov";
                    break;
            }

            // 检查目录是否存在
            if (!Directory.Exists(RootPath))
            {
                Console.Error.WriteLine($"Directory does not exist: {RootPath}");
                Environment.Exit(0);
            }

            // 先判断输出文件路径是否存在
            if (!File.Exists(OutputPath)) return;
            Console.Error.WriteLine($"Output file exists, please rename: {OutputPath}");
            Environment.Exit(0);


        }

        private void GetFlag()
        {
            var args = Environment.GetCommandLineArgs().Skip(1).ToList();

            Size = GetArgumentValue(args, "s") ?? "0";
            OutputPath = GetArgumentValue(args, "o") ?? "output.zip";
            AfterDateStr = GetArgumentValue(args, "t") ?? "";
            RootPath = GetArgumentValue(args, "d") ?? "";
            SkipDirs = GetArgumentValue(args, "x") ?? "";
            FileName = GetArgumentValue(args, "f") ?? "";
            Keyword = GetArgumentValue(args, "k") ?? "";
            Extension = GetArgumentValue(args, "e") ?? "all";
        }
        private static string GetArgumentValue(List<string> args, string option)
        {
            var index = args.IndexOf($"-{option}");
            if (index != -1 && index + 1 < args.Count)
            {
                return args[index + 1];
            }
            return null;
        }
        private static void ShowHelp()
        {
            Console.WriteLine("Usage: FuckDoc [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -s        Get total file size (global option) (default 0)");
            Console.WriteLine("  -o        Zip output path (global option) (default output.zip)");
            Console.WriteLine("  -t        Only query and pack files after the date, like '2023-10-01' (global option) (default \"\")");
            Console.WriteLine("  -d        Root path to query (global option) (default CurrentUserHOME)");
            Console.WriteLine("  -x        Paths to skip query (global option) (default C:\\Windows, C:\\Program Files, C:\\Program Files (x86), C:\\inetpub, C:\\Users\\Public)");
            Console.WriteLine("  -f        Query files by filename (only for QueryByFileName), eg. '-f config -f config,password,secret'");
            Console.WriteLine("  -k        Query files in content by keyword (only for QueryByKeyword), eg. '-k config -k password:,secret:,token:'");
            Console.WriteLine("  -e        Query files by extension, eg. '-e pdf,doc,zip' (default all:pdf,docx,doc,xlsx,xls,csv,pptx,ppt,zip,rar,7z,tar,gz,tgz,bak,bz2,txt)");
            Console.WriteLine("  -h        Show this help message");
        }
    }
    
}