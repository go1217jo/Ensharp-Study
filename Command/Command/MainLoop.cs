using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Command
{
    class MainLoop
    {
        OutputProcessor output;
        Functions functions;
        List<string> cmdList;
        string currentPath;
        enum COMMAND { CMD=0, CD, DIR, CLS, HELP, COPY, MOVE, EXIT };

        public MainLoop()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            output = new OutputProcessor();
            currentPath = homeDirectory;
            functions = new Functions(output);
            cmdList = functions.GetCmdList();
        }
                  
        public void Loop()
        {
            functions.VersionInfomation();
            string input;
            string command;
            string[] cmds;
            
            while(true)
            {
                output.PrintCurrentPath(currentPath);
                input = Console.ReadLine();
                input = input.Replace('/', '\\');
                cmds = input.Split(' ');

                // 입력된 명령어
                command = cmds[0].ToLower();
                command = command.TrimEnd('.');

                // 명령어 목록 존재 확인
                if(cmdList.Contains(command))
                {
                    string secondParam;
                    switch (cmdList.IndexOf(command))
                    {
                        case (int)COMMAND.CMD:
                            functions.VersionInfomation();
                            break;
                        case (int)COMMAND.CD:
                            string movedPath = functions.ChangeDirectory(input.Substring(2), currentPath);  // 이동된 경로를 구하면
                            if (movedPath != null) currentPath = movedPath;  // 없는 경로가 아니라면
                            else Console.WriteLine("지정된 경로를 찾을 수 없습니다."); // 없는 경로라면
                            break;
                        case (int)COMMAND.DIR:
                            functions.FileList(input.Substring(3), currentPath);
                            break;
                        case (int)COMMAND.CLS:
                            Console.Clear();
                            break;
                        case (int)COMMAND.COPY:
                            // 인수 개수에 대한 예외처리
                            secondParam = Exception.ArgumentCountException(cmds);
                            if (secondParam != null)
                                functions.Copy(cmds[1], secondParam, currentPath);
                            break;
                        case (int)COMMAND.HELP:
                            functions.PrintHelp(input.Substring(4));
                            break;
                        case (int)COMMAND.MOVE:
                            // 인수 개수에 대한 예외처리
                            secondParam = Exception.ArgumentCountException(cmds);
                            if(secondParam != null)
                                functions.Move(cmds[1], secondParam, currentPath);
                            break;
                        case (int)COMMAND.EXIT:
                            return;
                    }
                }
                else
                    Console.WriteLine("\'" + command + "\'은(는) 내부 또는 외부 명령, 실행할 수 있는 프로그램, 또는 배치 파일이 아닙니다.");

            }
        }
    }
}
