using System;
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
            
            if (fileFilter.FlagInfo.Size == "1")
            {
                // 定义 errors.log 文件路径
                string errorLogFilePath = Path.Combine(Environment.CurrentDirectory, "errors.log");

                // 打开或创建 errors.log 文件，以便将错误信息写入其中
                using (var errorLogWriter = new StreamWriter(errorLogFilePath, false))
                {
                    var totalFileSize = CalcTotalFileSizeRecursively(rootPath,errorLogWriter, fileFilter);
                    Console.WriteLine($"Total size is {TransformUtil.BytesToSize(totalFileSize)}");
                    Environment.Exit(0);
                }
                
            }
        
            // 创建一个 ZIP 文件
            using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                // 递归处理目录中的文件和子目录
                ProcessDirectoryRecursively(rootPath, zip, fileFilter);
            }
        
            Console.WriteLine($"All files compressed successfully: {zipFilePath}; Total size is {FileUtil.GetFileSize(zipFilePath)}");
            
        }
        
        // 爬取并计算总大小
        private static long CalcTotalFileSizeRecursively(string directoryPath, StreamWriter errorLogWriter,FileFilter fileFilter)
        {
            var directory = new DirectoryInfo(directoryPath);
            long totalSize = 0;
            
            
            
            // 处理目录中的文件
            foreach (var file in directory.GetFiles())
            {
                if (!fileFilter.Filter(file.FullName, file)) continue;
                // Console.WriteLine(file.FullName);
                try
                {
                    // 累积文件大小
                    totalSize += file.Length;
                }
                catch (Exception e)
                {
                    // 写入错误信息到 errors.log 文件
                    errorLogWriter.WriteLine($"Skipped file {file.FullName} due to exception: {e.Message}");
                    errorLogWriter.Flush();
                }
            }
    
            // 递归处理子目录
            foreach (var subDirectory in directory.GetDirectories())
            {
                try
                {
                    // 累积子目录的文件大小
                    totalSize += CalcTotalFileSizeRecursively(subDirectory.FullName,errorLogWriter, fileFilter);
                }
                catch (Exception e)
                {
                    // 写入错误信息到 errors.log 文件
                    errorLogWriter.WriteLine($"Skipped subDir {subDirectory.FullName} due to exception: {e.Message}");
                    errorLogWriter.Flush();
                    
                }
            }
            
            return totalSize;
            
            
        }
        // 循环爬取并压缩文件到zip
        private static void ProcessDirectoryRecursively(string directoryPath, ZipArchive zip, FileFilter fileFilter)
        {
            var directory = new DirectoryInfo(directoryPath);
        
            
            // 处理目录中的文件
            foreach (var file in directory.GetFiles())
            {
        
                
                if (!fileFilter.Filter(file.FullName, file)) continue;
                // Console.WriteLine(file.FullName);
                try
                {
                    AddFileToZip(zip, file.FullName);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Skipped file {file.FullName} due to exception: {e.Message}");
                }
            }
        
            // 递归处理子目录
            foreach (var subDirectory in directory.GetDirectories())
            {
                try
                {
                    ProcessDirectoryRecursively(subDirectory.FullName, zip, fileFilter);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Skipped subDir {subDirectory.FullName} due to exception: {e.Message}");
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
