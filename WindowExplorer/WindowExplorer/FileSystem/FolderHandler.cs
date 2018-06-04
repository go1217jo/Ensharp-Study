using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowExplorer.FileSystem
{
    class FolderHandler
    {
        public FolderHandler() { }

        /// <summary>
        /// 디렉터리이면서 탐색기에 나타나지 않는 폴더인지 아닌지 확인
        /// </summary>
        /// <param name="info"> 파일 및 디렉터리 정보 객체 </param>
        /// <returns>  디렉터리이면서 탐색기에 나타나지 않는 폴더가 아니면 true를 그렇지 않으면 false </returns>
        public bool IsDirectory(DirectoryInfo info)
        {
            if (info.Attributes.HasFlag(FileAttributes.Directory) && !info.Attributes.HasFlag(FileAttributes.NotContentIndexed)
                && !info.Attributes.HasFlag(FileAttributes.System))
                return true;
            return false;
        }

        /// <summary>
        /// 디렉터리 목록을 반환
        /// </summary>
        /// <param name="parentPath"> 이 경로 밑의 폴더 목록을 반환 </param>
        /// <returns> 디렉터리 목록 </returns>
        public List<DirectoryInfo> GetDirectoryList(string parentPath)
        {
            List<DirectoryInfo> fileSystemList = GetFileSystemList(parentPath);
            List<DirectoryInfo> directoriesList = new List<DirectoryInfo>();
            if (fileSystemList == null)
                return null;

            for (int idx = 0; idx < fileSystemList.Count; idx++)
            {
                if(IsDirectory(fileSystemList[idx]))
                    directoriesList.Add(fileSystemList[idx]);
            }
            
            return directoriesList;
        }

        /// <summary>
        /// 디렉터리 이름 목록을 반환한다.
        /// </summary>
        /// <param name="directories"> DirectoryInfo 객체 배열 </param>
        /// <returns>디렉터리 이름 목록</returns>
        public List<string> GetDirectoryNameList(List<DirectoryInfo> directories)
        {
            List<string> nameList = new List<string>();
            foreach (DirectoryInfo info in directories)
                nameList.Add(info.Name);
            return nameList;
        }

        /// <summary>
        /// 파일 및 디렉터리 목록을 반환
        /// </summary>
        /// <param name="parentPath"> 이 경로 밑의 폴더 목록을 반환 </param>
        /// <returns> 파일 및 디렉터리 목록 </returns>
        public List<DirectoryInfo> GetFileSystemList(string parentPath)
        {
            List<DirectoryInfo> filesList = new List<DirectoryInfo>();
            string[] entries = null;
            if (!new DirectoryInfo(parentPath).Exists)
                return null;
            try
            {
                entries = Directory.GetFileSystemEntries(parentPath);
            } catch (UnauthorizedAccessException e) { }

            if (entries == null)
                return null;

            foreach (string entry in entries)
            {
                DirectoryInfo info = new DirectoryInfo(entry);
                if (info.Attributes.HasFlag(FileAttributes.NotContentIndexed) | info.Attributes.HasFlag(FileAttributes.System) )
                    continue;
                filesList.Add(info);
            }
            return filesList;
        }
    }
}
