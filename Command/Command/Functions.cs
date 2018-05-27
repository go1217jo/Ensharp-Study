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
            if (!movePath.Contains("..") && !movePath.Equals(".") && directory.Exists)
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
                    // 루트 디렉터리면
                    if (movedPath.Equals("C:\\"))
                        temp = movedPath + unitPath[idx];
                    else
                        temp = movedPath + '\\' + unitPath[idx];

                    // 이동 예정 폴더가 현재 폴더 내에 존재한다면 이동
                    if (childDirectories.Contains(temp))
                        movedPath = temp;
                    else
                        return null;
                }
            }
            return movedPath;
        }

        // 디렉터리 크기 구하기
        public long DirectorySize(DirectoryInfo dInfo, bool includeSubDir)
        {
            long totalSize = 0;
            try
            {
                totalSize = dInfo.EnumerateFiles().Sum(file => file.Length);
            } catch(UnauthorizedAccessException e) { }

            if (includeSubDir) totalSize += dInfo.EnumerateDirectories().Sum(dir => DirectorySize(dir, true));
            return totalSize;
        }
        
        // 파일 목록 출력
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
                if (currentDirectory == null)
                    return;
            }

            string[] entries = Directory.GetFileSystemEntries(currentDirectory);
            string[] directories = Directory.GetDirectories(currentDirectory);
            
            for(int idx = 0; idx < entries.Length; idx++)
            {
                Console.Write(Directory.GetLastWriteTime(entries[idx]).ToString("yyyy-MM-dd tt hh:mm") + "    ");
                // 해당 경로가 폴더면
                if(directories.Contains(entries[idx]))
                {
                    Console.Write(output.PrintFixString("<DIR>", 15, Constant.LEFT));
                    Console.WriteLine(entries[idx].Substring(currentDirectory.Length + 1));
                    directoryByteSize += DirectorySize(new DirectoryInfo(entries[idx]), false);
                }
                else
                {
                    // 해당 경로가 파일이면
                    long currentFileSize = new FileInfo(entries[idx]).Length;
                    Console.Write(output.PrintFixString(currentFileSize.ToString(), 14, Constant.RIGHT) + ' ');
                    Console.WriteLine(entries[idx].Substring(currentDirectory.Length + 1));
                    fileByteSize += currentFileSize;
                }
            }
            Console.Write(output.PrintFixString((entries.Length - directories.Length) + "", 16, Constant.RIGHT) + "개 파일");
            Console.WriteLine(output.PrintFixString(fileByteSize.ToString(), 20, Constant.RIGHT) + " 바이트");

            Console.Write(output.PrintFixString(directories.Length + "", 16, Constant.RIGHT) + "개 디렉터리");
            Console.WriteLine(output.PrintFixString(directoryByteSize.ToString(), 17, Constant.RIGHT) + " 바이트 남음");
        }

        public void PrintHelp(string parameter)
        {
            StreamReader reader;
            if (parameter.Length == 0)
                reader = new StreamReader("..\\..\\help\\all.txt", Encoding.Default);
            else
            {
                if(new FileInfo("..\\..\\help\\" + parameter.ToLower() + ".txt").Exists)
                    reader = new StreamReader("..\\..\\help\\" + parameter.ToLower() + ".txt", Encoding.Default);
                else
                {
                    Console.WriteLine("이 명령은 도움말 유틸리티가 지원하지 않습니다. \"" + parameter + " /?\"를 사용해 보십시오.");
                    return;
                }
            }
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();
        }

        // 입력된 경로의 절대경로를 반환한다.
        public string GetAbsolutePath(string path, string currentPath)
        {
            string absolutePath = "";
            string[] splits = path.Split('\\');

            // 파일명만 입력된 경우(현재 폴더 내 파일)
            if (splits.Length == 1)
                absolutePath = currentPath + '\\' + path;
            // 다른 경로 내 파일인 경우, 해당 파일의 절대 경로를 구함
            else
            {
                // 먼저 파일이 존재하는 폴더의 절대 경로를 구함
                for (int idx = 0; idx < splits.Length - 1; idx++)
                    absolutePath += (splits[idx] + '\\');
                // 경로의 마지막에 \\ 제거
                absolutePath = absolutePath.Remove(absolutePath.Length - 1);

                // 절대경로 반환
                absolutePath = ChangeDirectory(absolutePath, currentPath);

                // 경로가 존재하지 않는다면
                if (absolutePath == null)
                {
                    Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                    return null;
                }
                absolutePath += ('\\' + splits[splits.Length - 1]);
            }
            return absolutePath;
        }

        // src에서 dest로 파일을 이동한다.
        public void Move(string src, string dest, string currentPath)
        {
            // 절대 경로 얻기
            string fromPath = GetAbsolutePath(src, currentPath);
            string toPath = GetAbsolutePath(dest, currentPath);
            
            // 파일이 없는 경우 종료
            if (fromPath == null || toPath == null || Exception.NotExistFileException(fromPath))
            {
                Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                return;
            }

            Directory.Move(fromPath, toPath);
            Console.WriteLine("        1개의 파일을 이동하였습니다.");
        }

        

        public void Copy(string src, string dest, string currentPath)
        {
            string fromPath = GetAbsolutePath(src, currentPath);
            string toPath = GetAbsolutePath(dest, currentPath);

            // 파일이 없는 경우 종료
            if (fromPath == null || toPath == null || Exception.NotExistFileException(fromPath))
            {
                Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                return;
            }

            // 복사하려는 파일의 경로 폴더가 존재하지 않으면 생성
            string targetPath = toPath.Remove(toPath.LastIndexOf('\\'));
            if (Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            
            // 동등한 파일을 복사 시도할 때
            if(fromPath.Equals(toPath))
            {
                Console.WriteLine("같은 파일로 복사할 수 없습니다.");
                Console.WriteLine("        0개 파일이 복사되었습니다.");
                return;
            }

            if(new FileInfo(toPath).Exists)
            {
                // 덮어씌운다는 질문에 대해 올바른 대답을 할 때까지 반복
                bool right = false;
                while (!right)
                {
                    Console.Write(toPath.Substring(toPath.LastIndexOf('\\') + 1) + "을(를) 덮어쓰시겠습니까? (Yes/No/All): ");
                    string answer = Console.ReadLine().ToLower();
                    switch (answer[0])
                    {
                        case 'y':
                            right = true;
                            break;
                        case 'n':
                            Console.WriteLine("        0개 파일이 복사되었습니다.");
                            return;
                        case 'a':
                            right = true;
                            break;
                    }
                }
            }
            File.Copy(fromPath, toPath, true);
            Console.WriteLine("        1개 파일이 복사되었습니다.");
        }
    }
}
