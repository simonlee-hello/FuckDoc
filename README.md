# Fdoc
信息收集工具 对目标机器上的文档进行收集并打包

## 特色功能

- 可限定大小，当文件大于该值则不进行打包操作

## TODO

-[x] 增加只对指定目录压缩的功能 -d 就是指定目录；-f就是指定文件；-k就是文件内容；-e就是后缀
-[x] 增加模式选项：1、指定目录压缩；2、后缀压缩；3、近似文件名压缩；4、近似内容
-[ ] 文件多时，会栈溢出，需要边爬取文件边进行打包

```shell
Usage: FuckDoc [options]
Options:
  -s        Get total file size (global option) (default 0)
  -o        Zip output path (global option) (default output.zip)
  -t        Only query and pack files after the date, like '2023-10-01' (global option) (default "")
  -d        Root path to query (global option)
  -x        Paths to skip query (global option)
  -f        Query files by filename (only for QueryByFileName), eg. '-f config -f config,password,secret'
  -k        Query files in content by keyword (only for QueryByKeyword), eg. '-k config -k password:,secret:,token:'
  -e        Query files by extension, eg. '-e pdf,doc,zip'
  -h        Show this help message
```

文件后缀查询


```
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
```

```shell
FuckDoc -d C:\test -o output.zip #打包C:\test文件夹下所有文件
FuckDoc -d C:\test -o output.zip -e all #打包C:\test文件夹下所有符合以上后缀的文件
FuckDoc -d C:\test -o output.zip -e pdf #打包C:\test文件夹下所有pdf后缀的文件
Fuckdoc -d C:\test -s 1 -e all #获取符合条件的文件的总大小
```


通过文件名进行近似查询

```shell
Fdoc -d C:\ -max 10GB -o output.zip -f password,secret,config
```


