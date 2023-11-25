using System;
using System.Collections.Generic;
using System.IO;

namespace FuckDoc.Utils
{
    public static class FileUtil
    {
        public static string GetFileSize(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                return TransformUtil.BytesToSize(fileInfo.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to obtain file information {filePath}: {ex.Message}");
                return null; // 文件不存在或获取信息失败时返回0
            }
        }
        // // 计算文件总大小
        // public static long GetTotalSize(string[] files)
        // {
        //     long totalSize = 0;
        //     foreach (var filePath in files)
        //     {
        //         try
        //         {
        //             var fileInfo = new FileInfo(filePath);
        //             totalSize += fileInfo.Length;
        //         }
        //         catch (Exception ex)
        //         {
        //             Console.WriteLine($"Unable to obtain file information {filePath}: {ex.Message}");
        //         }
        //     }
        //     return totalSize;
        // }

        // public static void DeleteFile(string path)
        // {
        //     if (IsFileExists(path))
        //     {
        //         try
        //         {
        //             File.Delete(path);
        //         }
        //         catch (Exception ex)
        //         {
        //             Console.WriteLine($"Delete failed for path {path}: {ex.Message}");
        //         }
        //     }
        // }

        // public static bool IsFileExists(string filePath)
        // {
        //     return File.Exists(filePath);
        // }

        public static bool SkipDirectory(string directoryPath, string rootPath, string skipDirs)
        {
            if (string.IsNullOrEmpty(skipDirs))
            {
                return false;
            }

            var skipDirList = TransformUtil.ConvertStringToList(skipDirs);

            foreach (var skipDir in skipDirList)
            {
                var fullPath = Path.Combine(rootPath, skipDir.Trim());
                if (directoryPath.StartsWith(fullPath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        public static string GetRelativePath(string basePath, string targetPath)
        {
            var baseUri = new Uri(basePath);
            var targetUri = new Uri(targetPath);

            var relativeUri = baseUri.MakeRelativeUri(targetUri);

            return Uri.UnescapeDataString(relativeUri.ToString());
        }

    }
    
}