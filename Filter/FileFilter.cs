using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FuckDoc.Filter
{
    public class FileFilter
    {
        public readonly Option.FlagInfo FlagInfo;
        

        public FileFilter(Option.FlagInfo flagInfo)
        {
            FlagInfo = flagInfo;
        }

        // 过滤文件
        public bool Filter(string path, FileSystemInfo d)
        {
            return DateFilter(d) && FilenameFilter(d) && KeywordFilter(path) && ExtFilter(d);
        }

        // 根据后缀进行过滤
        private bool ExtFilter(FileSystemInfo d)
        {
            if (string.IsNullOrEmpty(FlagInfo.Extension))
            {
                return true;
            }

            var ext = Path.GetExtension(d.Name);
            var extensionsSet = Utils.TransformUtil.StringToHashSet(FlagInfo.Extension);
            // if (FlagInfo.Extension == "all")
            // {
            //     extensionsSet = new HashSet<string>
            //     {
            //         ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".csv", ".pptx", ".ppt", ".zip", ".rar", ".7z", ".tar",
            //         ".gz", ".tgz"
            //     };
            // }

            return extensionsSet.Contains(ext);
        }

        // 如果有日期限制，检查修改时间是否在指定日期之后
        private bool DateFilter(FileSystemInfo d)
        {
            if (string.IsNullOrEmpty(FlagInfo.AfterDateStr))
            {
                return true;
            }

            if (DateTime.TryParseExact(FlagInfo.AfterDateStr, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var afterDate))
            {
                return afterDate == DateTime.MinValue || d.LastWriteTime > afterDate;
            }

            Console.WriteLine($"Failed to parse after date: {FlagInfo.AfterDateStr}");
            Environment.Exit(0);
            return false; // Unreachable
        }

        // 文件名匹配过滤
        private bool FilenameFilter(FileSystemInfo d)
        {
            if (string.IsNullOrEmpty(FlagInfo.FileName))
            {
                return true;
            }

            var queryStrings = FlagInfo.FileName.Split(',');
            return queryStrings.Any(queryString => d.Name.ToLower().Contains(queryString.ToLower()));
        }

        // 文件内容关键字匹配过滤
        private bool KeywordFilter(string path)
        {
            if (string.IsNullOrEmpty(FlagInfo.Keyword))
            {
                return true;
            }

            try
            {
                using (var file = new StreamReader(path))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        // 将文件内容转换为小写，然后进行比较
                        if (line.ToLower().Contains(FlagInfo.Keyword.ToLower()))
                        {
                            file.Close();
                            return true;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error when reading file: {ex.Message}");
            }

            return false;
        }
    }
}