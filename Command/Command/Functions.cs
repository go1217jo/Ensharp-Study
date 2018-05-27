using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Command
{
    class Functions
    {
        OutputProcessor output;

        public Functions(OutputProcessor output)
        {
            this.output = output;
        }

        public List<string> GetCmdList()
        {
            return new List<string>(new string[] { "cmd", "cd", "dir", "cls", "help", "copy", "move", "exit"});
        }

        public void VersionInfomation()
        {
            Console.WriteLine("Microsoft Windows [Version 10.0.16299.431]\n(c) 2017 Microsoft Corporation.All rights reserved.");
        }
              
        public string ChangeDirectory(string movePath, string currentPath)
        {
            // 이동된 결과 경로
            string movedPath = currentPath;

            // 존재하는 절대 경로이면 그대로 반환
            DirectoryInfo directory = new DirectoryInfo(movePath);
            if (!movePath.Equals("..") && !movePath.Equals(".") && directory.Exists)
                return directory.FullName;

            // 상대경로이거나 존재하지 않는 절대경로인 경우
            string[] unitPath = movePath.Split('\\');

            for (int idx = 0; idx < unitPath.Length; idx++)
            {
                // path == .. 이면 상위 디렉터리
                if (unitPath[idx].Equals(".."))
                {
                    if(!movedPath.Equals("C:\\"))
                        movedPath = Directory.GetParent(movedPath).FullName;
                }
                // 하위 경로의 폴더로 이동
                else if (!unitPath[idx].Equals("."))
                {
                    // 하위 경로에 폴더가 존재하면 이동
                    List<string> childDirectories = new List<string>(Directory.GetDirectories(movedPath));

                    // 이동 예정 경로
                    string temp;
                    if (movedPath.Equals("C:\\"))
                        temp = movedPath + unitPath[idx];
                    else
                        temp = movedPath + '\\' + unitPath[idx];

                    if (childDirectories.Contains(temp))
                        movedPath = temp;
                    else
                    {
                        // 없는 경로라면
                        Console.WriteLine("\n지정된 경로를 찾을 수 없습니다.\n");
                        return currentPath;
                    }
                    
                }
            }
            return movedPath;
        }

        // 디렉터리 크기 구하기
        public long DirectorySize(DirectoryInfo dInfo, bool includeSubDir)
        {
            long totalSize = dInfo.EnumerateFiles().Sum(file => file.Length);
            if (includeSubDir) totalSize += dInfo.EnumerateDirectories().Sum(dir => DirectorySize(dir, true));
            return totalSize;
        }

        public void FileList(string path, string currentPath)
        {
            string currentDirectory = currentPath;
            // 폴더 바이트 크기
            long directoryByteSize = 0;
            // 파일 바이트 크기
            long fileByteSize = 0;

            if(path.Length != 0)
            {
                currentDirectory = ChangeDirectory(path, currentPath);
                // 없는 경로
                if (currentDirectory.Equals(currentPath) && !path.Equals("."))
                    return;
            }

            string[] entries = Directory.GetFileSystemEntries(currentDirectory);
            string[] directories = Directory.GetDirectories(currentDirectory);
            
            for(int idx = 0; idx < entries.Length; idx++)
            {
                Console.Write(Directory.GetLastWriteTime(entries[idx]) + "    ");
                // 해당 경로가 폴더면
                if(directories.Contains(entries[idx]))
                {
                    Console.Write(output.PrintFixString("<DIR>", 15, Constant.LEFT));
                    Console.WriteLine(entries[idx].Substring(currentPath.Length));
                    directoryByteSize += DirectorySize(new DirectoryInfo(entries[idx]), true);
                }
                else
                {
                    // 해당 경로가 파일이면
                    long currentFileSize = new FileInfo(entries[idx]).Length;
                    Console.Write(output.PrintFixString(currentFileSize.ToString(), 14, Constant.RIGHT) + ' ');
                    Console.WriteLine(entries[idx].Substring(currentPath.Length));
                    fileByteSize += currentFileSize;
                }
            }
            Console.Write(output.PrintFixString((entries.Length - directories.Length) + "", 16, Constant.RIGHT) + "개 파일");
        }

    }
}
