using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using FuckDoc.Filter;
using FuckDoc.Utils;

namespace FuckDoc.Process
{
    public static class Process
    {
        public static void WalkAndProcess(Option.FlagInfo info)
        {
            var fileFilter = new FileFilter(info);
            CompressFilesInDirectory(info.RootPath, info.OutputPath, fileFilter);
        }

        private static void CompressFilesInDirectory(string rootPath, string zipFilePath, FileFilter fileFilter)
        {
           
            // 判断当前目录是否在SkipDirs中，如果是，则跳过处理该目录
            if (FileUtil.SkipDirectory(rootPath, fileFilter.FlagInfo.RootPath, fileFilter.FlagInfo.SkipDirs))
            {
                Console.WriteLine($"Skipping directory: {rootPath}");
                return;
            }
            
            if (fileFilter.FlagInfo.GetSize == "true")
            {
                var errorLogs = new List<string>();
                var totalFileSize = CalcTotalFileSizeRecursively(rootPath, errorLogs, fileFilter);
                Console.WriteLine($"Total size is {TransformUtil.BytesToSize(totalFileSize)}");
                if (errorLogs.Count > 0)
                {
                    string errorLogFilePath = Path.Combine(Environment.CurrentDirectory, $"errors-{DateTime.Now:yyyyMMdd-HHmmss}.log");
                    File.WriteAllLines(errorLogFilePath, errorLogs);
                }
                Environment.Exit(0);
            }
        
            // 创建一个 ZIP 文件
            using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                // 递归处理目录中的文件和子目录
                var errorLogs = new List<string>();
                ProcessDirectoryRecursively(rootPath, zip, fileFilter, errorLogs);
                if (errorLogs.Count > 0)
                {
                    string errorLogFilePath = Path.Combine(Environment.CurrentDirectory, $"errors-{DateTime.Now:yyyyMMdd-HHmmss}.log");
                    File.WriteAllLines(errorLogFilePath, errorLogs);
                }
            }
        
            Console.WriteLine($"All files compressed successfully: {zipFilePath}; Total size is {FileUtil.GetFileSize(zipFilePath)}");
            
        }
        
        // 爬取并计算总大小
        private static long CalcTotalFileSizeRecursively(string directoryPath, List<string> errorLogs, FileFilter fileFilter)
        {
            var directory = new DirectoryInfo(directoryPath);
            long totalSize = 0;
        
            foreach (var file in directory.GetFiles())
            {
                if (!fileFilter.Filter(file.FullName, file)) continue;
                try
                {
                    totalSize += file.Length;
                }
                catch (Exception e)
                {
                    errorLogs.Add($"Skipped file {file.FullName} due to exception: {e.Message}");
                }
            }
        
            foreach (var subDirectory in directory.GetDirectories())
            {
                try
                {
                    totalSize += CalcTotalFileSizeRecursively(subDirectory.FullName, errorLogs, fileFilter);
                }
                catch (Exception e)
                {
                    errorLogs.Add($"Skipped subDir {subDirectory.FullName} due to exception: {e.Message}");
                }
            }
        
            return totalSize;
        }
        
        // 循环爬取并压缩文件到zip
        private static void ProcessDirectoryRecursively(string directoryPath, ZipArchive zip, FileFilter fileFilter, List<string> errorLogs)
        {
            var directory = new DirectoryInfo(directoryPath);
        
            foreach (var file in directory.GetFiles())
            {
                if (!fileFilter.Filter(file.FullName, file)) continue;
                try
                {
                    AddFileToZip(zip, file.FullName);
                }
                catch (Exception e)
                {
                    errorLogs.Add($"Skipped file {file.FullName} due to exception: {e.Message}");
                }
            }
        
            foreach (var subDirectory in directory.GetDirectories())
            {
                try
                {
                    ProcessDirectoryRecursively(subDirectory.FullName, zip, fileFilter, errorLogs);
                }
                catch (Exception e)
                {
                    errorLogs.Add($"Skipped subDir {subDirectory.FullName} due to exception: {e.Message}");
                }
            }
        }
        
        private static void AddFileToZip(ZipArchive zip, string sourceFilePath)
        {
                var fileInfo = new FileInfo(sourceFilePath);
            
                // // 创建 ZIP 文件中的条目
                // var entry = zip.CreateEntry(sourceFilePath);
                // 创建 ZIP 文件中的条目，并保留相对路径
                var entry = zip.CreateEntry(sourceFilePath.Substring(sourceFilePath.IndexOf(fileInfo.DirectoryName, StringComparison.Ordinal)));
                // 设置条目的修改时间，以保留文件的原始时间戳
                entry.LastWriteTime = fileInfo.LastWriteTime;
        
                // 打开文件流并将文件内容写入 ZIP 条目
                using (var entryStream = entry.Open())
                using (var sourceStream = File.OpenRead(sourceFilePath))
                {
                    sourceStream.CopyTo(entryStream);
                }
        
        }
    }
}
