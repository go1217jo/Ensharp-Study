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
        /// <summary>
        /// 디렉터리 목록을 반환
        /// </summary>
        /// <param name="parentPath"> 이 경로 밑의 폴더 목록을 반환 </param>
        /// <returns> 디렉터리 목록 </returns>
        public List<string> GetDirectoryList(string parentPath)
        {
            string[] entries = Directory.GetFileSystemEntries(parentPath);
            foreach (string entry in entries)
            {

            }
            return new List<string>();
        }
    }
}
