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

            // 创建一个 ZIP 文件
            using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                // 递归处理目录中的文件和子目录
                ProcessDirectoryRecursively(rootPath, zip, fileFilter);
            }

            Console.WriteLine($"All files compressed successfully: {zipFilePath}; Total size is {FileUtil.GetFileSize(zipFilePath)}");
            
        }

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
                    AddFileToZip(zip, file.FullName, fileFilter.FlagInfo.OutputPath);
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

        private static void AddFileToZip(ZipArchive zip, string sourceFilePath, string outputPath)
        {
            
                // Calculate the relative path inside the ZIP file
                var relativePath = FileUtil.GetRelativePath(outputPath, sourceFilePath);
                // Normalize the path separators in case the sourceFilePath uses backslashes
                relativePath = relativePath.Replace("\\", "/");

                // 在 ZIP 文件中创建一个条目并从源文件复制内容
                // zip.CreateEntryFromFile(sourceFilePath, relativePath);
                // 创建 ZIP 文件中的条目
                var entry = zip.CreateEntry(relativePath);

                // 打开文件流并将文件内容写入 ZIP 条目
                using (var entryStream = entry.Open())
                using (var sourceStream = File.OpenRead(sourceFilePath))
                {
                    sourceStream.CopyTo(entryStream);
                }

        }
    }
}
