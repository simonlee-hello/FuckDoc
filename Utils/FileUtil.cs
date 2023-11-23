using System;
using System.IO;

namespace FuckDoc.Utils
{
    public class FileUtil
    {
        // 计算文件总大小
        public static long GetTotalSize(string[] files)
        {
            long totalSize = 0;
            foreach (var filePath in files)
            {
                try
                {
                    var fileInfo = new FileInfo(filePath);
                    totalSize += fileInfo.Length;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to obtain file information {filePath}: {ex.Message}");
                }
            }
            return totalSize;
        }

        public static void DeleteFile(string path)
        {
            if (IsFileExists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Delete failed for path {path}: {ex.Message}");
                }
            }
        }

        public static bool IsFileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
    
}