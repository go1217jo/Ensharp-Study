﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Management;
using Command.Function;

namespace Command
{
    /// <summary>
    ///  cmd에서 지원하는 각종 명령어들을 정의한다.
    /// </summary>
    class MainFunctions
    {
        OutputProcessor output;
        StringComparison comp;
        SubFunctions sub;

        public MainFunctions(OutputProcessor output)
        {
            this.output = output;
            // 대소문자 미구분자
            comp = StringComparison.OrdinalIgnoreCase;
            sub = new SubFunctions(output);
        }

        // 지원하는 명령어 리스트를 반환한다.
        public List<string> GetCmdList()
        {
            return new List<string>(new string[] { "cmd", "cd", "dir", "cls", "help", "copy", "move", "mkdir", "rmdir", "exit"});
        }

        /// <summary>
        /// 윈도우 버전 정보를 출력한다,
        /// </summary>
        public void VersionInfomation()
        {
            OperatingSystem system = Environment.OSVersion;
            Console.WriteLine("Microsoft Windows [Version " + "10.0.16299.431" + "]\n(c) 2017 Microsoft Corporation.All rights reserved.");
        }

        /// <summary>
        ///  이동하고자 하는 해당 경로의 절대 경로 문자열을 반환
        /// </summary>
        /// <param name="movePath"> 이동하고자 하는 경로 </param>
        /// <param name="currentPath"> 현재 있는 경로 </param>
        /// <returns> 변환된 절대 경로 </returns>
        public string ChangeDirectory(string movePath, string currentPath)
        {
            // 이동된 결과 경로
            string movedPath = currentPath;

            // 앞의 공백 제거
            movePath = movePath.TrimStart(' ');
            
            if(movePath.Length == 0)
            {
                Console.WriteLine(currentPath);
                return currentPath;
            }

            // UNC 경로 예외처리
            if (Exception.UNCPathException(movePath))
                return currentPath;

            // 존재하는 절대 경로이면 그대로 반환
            DirectoryInfo directory = new DirectoryInfo(movePath);
            if (!movePath.Contains("..") && !movePath.Contains(".\\") && !movePath.Equals(".") && directory.Exists)
                return directory.FullName;

            // 상대경로이거나 존재하지 않는 절대경로인 경우
            string[] unitPath = movePath.Split('\\');

            for (int idx = 0; idx < unitPath.Length; idx++)
            {
                if (unitPath[idx].Length == 0)
                    continue;
                // ...(.이 3 이상인 경우 무시된 경로가 되도록 한다.
                if (unitPath[idx].Contains("..."))
                    unitPath[idx] = ".";
                
                // path == .. 이면 상위 디렉터리
                if (unitPath[idx].Equals(".."))
                {
                    if(!sub.IsRootDirectory(movedPath))
                        movedPath = Directory.GetParent(movedPath).FullName;
                }
                // 하위 경로의 폴더로 이동
                else if (!unitPath[idx].Equals("."))
                {
                    // 하위 디렉터리 목록
                    string[] childDirectories = Directory.GetDirectories(movedPath);

                    // 하위 경로에 폴더가 존재하면 이동
                    string temp;  // 이동 예정 경로
                    // 루트 디렉터리면
                    if(sub.IsRootDirectory(movedPath))
                        temp = movedPath + unitPath[idx];
                    else
                        // 루트 디렉터리가 아니면 구분자를 붙임
                        temp = movedPath + '\\' + unitPath[idx];

                    // 이동 예정 폴더가 현재 폴더 내에 존재한다면 이동
                    bool exist = false;
                    
                    for(int child = 0; child < childDirectories.Length; child++)
                    {
                        // 같은 것이 존재
                        if (childDirectories[child].Equals(temp, comp))
                        {
                            movedPath = childDirectories[child];
                            exist = true;
                            break;
                        }
                    }
                    if(!exist)
                        return null;
                }
            }
            return movedPath;
        }

        /// <summary>
        /// dir 명령어가 입력되었을 때, currentPath의 파일 및 폴더 목록이 출력된다.
        /// </summary>
        /// <param name="path"> 이동 경로 </param>
        /// <param name="currentPath"> 현재 있는 경로 </param>
        public void FileList(string path, string currentPath)
        {
            string currentDirectory = currentPath;
            int directoryCount = 0, fileCount = 0, driveIndex = 0;

            // 기본 드라이브
            DriveInfo[] drive = DriveInfo.GetDrives();
            if (currentPath[0].Equals('D') || currentPath[0].Equals('d'))
                driveIndex = 1;

            // 드라이브의 남은 용량
            long directoryByteSize = drive[driveIndex].AvailableFreeSpace;
            // 파일 바이트 크기
            long fileByteSize = 0;

            // volumelabel 출력
            string volumeName = drive[driveIndex].VolumeLabel;
            if (volumeName == "")
                Console.WriteLine(" {0}드라이브의 볼륨에는 이름이 없습니다.", drive[driveIndex].Name[0]);
            else
                Console.WriteLine(" 이름: {0}", volumeName);

            Console.WriteLine(" 볼륨 일련 번호: {0}\n", sub.GetVolumeNumber(drive[driveIndex].Name));

            if(path.Length != 0)
            {
                // 절대경로를 구함
                currentDirectory = ChangeDirectory(path, currentPath);
                // 없는 경로
                if (currentDirectory == null)
                    return;
            }

            Console.WriteLine(" {0} 디렉터리\n", currentDirectory);

            string[] entries = Directory.GetFileSystemEntries(currentDirectory);
            string[] directories = Directory.GetDirectories(currentDirectory);

            // 루트 경로가 아니면
            if(!sub.IsRootDirectory(currentDirectory)) { 
                // .과 ..을 출력
                sub.DirRoot(currentDirectory);
                directoryCount += 2;
            }

            for(int idx = 0; idx < entries.Length; idx++)
            {
                // System file이나 folder면 생략
                DirectoryInfo information = new DirectoryInfo(entries[idx]);
                if (information.Attributes.HasFlag(FileAttributes.NotContentIndexed) | information.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                Console.Write(Directory.GetLastWriteTime(entries[idx]).ToString("yyyy-MM-dd tt hh:mm") + "    ");
                // 해당 경로가 폴더면
                if(directories.Contains(entries[idx]))
                {
                    Console.Write(output.PrintFixString("<DIR>", 15, Constant.LEFT));
                    // 폴더명을 출력, 현재 폴더가 루트인 경우 폴더명 뒤에 \가 포함되므로 따로 경우를 다룬다.
                    Console.WriteLine(entries[idx].Substring(currentDirectory.Length + (sub.IsRootDirectory(currentDirectory) ? 0 : 1)));
                    directoryCount++;
                }
                else
                {
                    // 해당 경로가 파일이면
                    long currentFileSize = new FileInfo(entries[idx]).Length;
                    Console.Write(output.PrintFixString(output.InsertComma(currentFileSize.ToString()), 14, Constant.RIGHT) + ' ');
                    // 파일명을 출력, 현재 폴더가 루트인 경우 폴더명 뒤에 \가 포함되므로 따로 경우를 다룬다.
                    Console.WriteLine(entries[idx].Substring(currentDirectory.Length + (sub.IsRootDirectory(currentDirectory) ? 0 : 1)));
                    fileByteSize += currentFileSize;
                    fileCount++;
                }
            }
            // 파일과 디렉터리에 대한 개수와 크기 출력
            Console.Write(output.PrintFixString(fileCount + "", 16, Constant.RIGHT) + "개 파일");
            Console.WriteLine(output.PrintFixString(output.InsertComma(fileByteSize.ToString()), 20, Constant.RIGHT) + " 바이트");

            Console.Write(output.PrintFixString(directoryCount + "", 16, Constant.RIGHT) + "개 디렉터리");
            Console.WriteLine(output.PrintFixString(output.InsertComma(directoryByteSize.ToString()), 17, Constant.RIGHT) + " 바이트 남음");
        }

        /// <summary>
        /// 도움말을 출력하며, 인수가 주어지면 그에 해당하는 상세 도움말이 출력된다.
        /// </summary>
        /// <param name="parameter"> 상세보기할 명령어 </param>
        public void PrintHelp(string parameter)
        {
            StreamReader reader;
            // 앞의 공백 제거
            parameter = parameter.TrimStart(' ');

            // 인수가 없다면 전체 요약 출력
            if (parameter.Length == 0)
                reader = new StreamReader("..\\..\\help\\all.txt", Encoding.Default);
            else
            {
                // 인수에 해당하는 도움말 텍스트 파일을 출력
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

        /// <summary>
        /// 입력된 경로의 절대경로를 반환한다.
        /// </summary>
        /// <param name="path"> 절대경로로 변경하고자 하는 경로 문자열 </param>
        /// <param name="currentPath"> 현재 있는 경로 </param>
        /// <returns> 변환된 절대 경로 </returns>
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

                // 경로의 마지막에 \\ 제거, 루트 디렉터리인 경우 제외
                if (!sub.IsRootDirectory(absolutePath))
                {
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
                else
                    absolutePath = path;
            }
            return absolutePath;
        }

        /// <summary>
        /// src에서 dest로 파일을 이동한다.
        /// 파일에서 파일 이동(이름변경), 파일을 폴더 안으로 이동, 폴더를 폴더 안으로 이동 지원
        /// </summary>
        /// <param name="src"> 원본 파일 및 폴더 </param>
        /// <param name="dest"> 이동 대상 </param>
        /// <param name="currentPath"> 현재 있는 경로 </param>
        public void Move(string src, string dest, string currentPath)
        {
            // 절대 경로 얻기
            string fromPath = GetAbsolutePath(src, currentPath);
            if (fromPath == null) return;
            string toPath = GetAbsolutePath(dest, currentPath);
            if (toPath == null) return;

            // 한 개의 파일만 이동하는 경우
            if (!src.Contains("*"))
            {
                DirectoryInfo fromPathInfo = new DirectoryInfo(fromPath);
                DirectoryInfo toPathInfo = new DirectoryInfo(toPath);

                // 원본 경로의 폴더가 존재하지 않는다면
                if (fromPathInfo.Attributes.HasFlag(FileAttributes.Directory) && !fromPathInfo.Exists)
                {
                    Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                    return;
                }

                // fromPath가 폴더인 경우
                if (fromPathInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    // toPath가 존재하면
                    if (toPathInfo.Exists)
                    {
                        // toPath가 폴더이면
                        if (toPathInfo.Attributes.HasFlag(FileAttributes.Directory))
                        {
                            string[] fromSplit = fromPath.Split('\\');
                            toPath = toPathInfo.FullName + '\\' + fromSplit[fromSplit.Length - 1];
                            toPathInfo = new DirectoryInfo(toPath);  // 바뀐 디렉터리 정보
                            if(toPathInfo.Exists)
                            {
                                // 덮어쓰지 않는다면
                                if (sub.Overwrite(toPathInfo.FullName) == Constant.NO) {
                                    Console.WriteLine("        0개의 디렉터리을 이동하였습니다.");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // toPath가 파일이면
                            if (sub.Overwrite(toPathInfo.FullName) == Constant.NO) {
                                Console.WriteLine("        0개의 디렉터리을 이동하였습니다.");
                                return;
                            }
                            else
                                // 덮어쓰기면 해당 파일을 삭제하고 파일명으로 폴더이름을 함
                                File.Delete(toPathInfo.FullName);
                        }
                    }
                    Directory.Move(fromPath, toPath);
                    Console.WriteLine("        1개의 디렉터리을 이동하였습니다.");
                }
                // fromPath가 파일인 경우
                else
                {
                    // toPath가 폴더인 경우 그 안에 이동
                    if(toPathInfo.Attributes.HasFlag(FileAttributes.Directory) && toPathInfo.Exists)
                    {
                        string[] fromSplit = fromPath.Split('\\');
                        toPath = toPathInfo.FullName + '\\' + fromSplit[fromSplit.Length - 1];
                    }
                    FileInfo toPathFileInfo = new FileInfo(toPath);  // 파일 정보
                    if (toPathFileInfo.Exists)
                    {
                        // 덮어쓰지 않는다면
                        if (sub.Overwrite(toPathFileInfo.FullName) == Constant.NO)
                        {
                            Console.WriteLine("        0개의 파일을 이동하였습니다.");
                            return;
                        }
                        else
                            File.Delete(toPathFileInfo.FullName);
                    }

                    Directory.Move(fromPath, toPath);
                    Console.WriteLine("        1개의 파일을 이동하였습니다.");
                }
            }
        }

        /// <summary>
        /// src에서 dest로 복사
        /// 파일에서 파일 복사 지원
        /// </summary>
        /// <param name="src"> 원본 파일 </param>
        /// <param name="dest"> 복사하고자 하는 경로 및 파일 </param>
        /// <param name="currentPath"> 현재 있는 경로 </param>
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
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            
            // 동등한 파일을 복사 시도할 때
            if(fromPath.Equals(toPath))
            {
                Console.WriteLine("같은 파일로 복사할 수 없습니다.");
                Console.WriteLine("        0개 파일이 복사되었습니다.");
                return;
            }

            // 복사될 파일명이 존재하는지 검사
            if(new FileInfo(toPath).Exists)
            {
                if (sub.Overwrite(toPath) == Constant.NO)
                    return;
            }

            // toPath가 폴더 경로이면 fromPath의 파일명과 같은 이름으로 폴더 내에 복사
            DirectoryInfo toPathInfo = new DirectoryInfo(toPath);
            if(toPathInfo.Attributes.HasFlag(FileAttributes.Directory) && toPathInfo.Exists)
            {
                string filename = sub.GetFileName(fromPath);
                if (filename != null)
                    toPath = Path.Combine(toPath, filename);
            }

            File.Copy(fromPath, toPath, true);
            Console.WriteLine("        1개 파일이 복사되었습니다.");
        }
    }
}
