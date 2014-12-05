using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Motr.IO
{
    public class DirectoryMgr
    {
        /// <summary>
        /// 获取某路径下的所有文件信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(String path)
        {
            return GetFiles(path, "*");
        }
        /// <summary>
        /// 获取某路径下的所有文件信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="searchPattern">搜索字符串，如用于搜索所有以单词“System”开头的目录的“System*”。</param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(String path, String searchPattern)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileSystemInfo[] fsiArray = di.GetFileSystemInfos(searchPattern);
            List<FileInfo> fileList = new List<FileInfo>();
            foreach (FileSystemInfo fsi in fsiArray)
            {
                if (fsi is FileInfo)
                    fileList.Add(fsi as FileInfo);
            }
            return fileList;
        }
    }
}
