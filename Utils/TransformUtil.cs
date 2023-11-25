using System.Collections.Generic;

namespace FuckDoc.Utils
{
    public static class TransformUtil
    {
        // 将输入的字符串解析为数字和单位，然后根据单位将其转换为字节数
        public static long SizeToBytes(string maxSize)
        {
            // 去掉空格并转为大写
            var maxSizeUpper = maxSize.Replace(" ", "").ToUpper();

            // 分离数字和单位
            var numStr = maxSizeUpper;
            var unit = "";
            foreach (var c in maxSizeUpper)
            {
                if (c < '0' || c > '9')
                {
                    var i = maxSizeUpper.IndexOf(c);
                    numStr = maxSizeUpper.Substring(0, i);
                    unit = maxSizeUpper.Substring(i);
                    break;
                }
            }

            // 解析数字部分
            if (double.TryParse(numStr, out double num))
            {
                // 根据单位转换为字节
                switch (unit)
                {
                    case "KB":
                        return (long)(num * 1024);
                    case "MB":
                        return (long)(num * 1024 * 1024);
                    case "GB":
                        return (long)(num * 1024 * 1024 * 1024);
                    default:
                        // 默认情况下，认为是字节
                        return (long)num;
                }
            }
            return 0;
        }

        public static string BytesToSize(long bytes)
        {
            // 根据字节数大小，确定使用的单位
            string unit = "Bytes";
            double value = bytes;

            if (bytes >= 1024 * 1024 * 1024)
            {
                unit = "GB";
                value = bytes / (1024.0 * 1024 * 1024);
            }
            else if (bytes >= 1024 * 1024)
            {
                unit = "MB";
                value = bytes / (1024.0 * 1024);
            }
            else if (bytes >= 1024)
            {
                unit = "KB";
                value = bytes / 1024.0;
            }

            return $"{value:F2} {unit}";
        }

        public static List<string> ConvertStringToList(string input)
        {
            // 以逗号分隔字符串
            var parts = input.Split(',');

            // 初始化结果列表
            var result = new List<string>(parts.Length);

            // 去掉每个部分的前后空白，并添加到结果列表
            foreach (var part in parts)
            {
                var trimmedPart = part.Trim();
                result.Add(trimmedPart);
            }

            return result;
        }

        // 将字符串转为集合，例如："docx,pdf,xlsx" -> {".pdf", ".docx", ".doc", ".xlsx"}
        public static HashSet<string> StringToHashSet(string supportedExtensions)
        {
            var set = new HashSet<string>();

            // 将以逗号分隔的字符串分割成数组
            var list = supportedExtensions.Split(',');

            // 遍历数组并将每个扩展名添加到集合中
            foreach (var ext in list)
            {
                // 去除空格
                var trimmedExt = ext.Trim();
                if (!string.IsNullOrEmpty(trimmedExt))
                {
                    // 将扩展名转换为小写并添加到集合中
                    set.Add("." + trimmedExt.ToLower());
                }
            }

            return set;
        }

        public static string TransformSlash(string input)
        {
            return input.Replace(@"\", "/");
        }
    }
}