using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Command
{
    /// <summary>
    ///  프로그램 내 main loop를 정의한다.
    /// </summary>
    class MainLoop
    {
        OutputProcessor output;
        MainFunctions functions;
        List<string> cmdList;
        // 현재 있는 경로
        string currentPath;
        enum COMMAND { CMD=0, CD, DIR, CLS, HELP, COPY, MOVE, MKDIR, RMDIR, EXIT };

        public MainLoop()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            output = new OutputProcessor();
            currentPath = homeDirectory;
            functions = new MainFunctions(output);
            cmdList = functions.GetCmdList();
        }

        /// <summary>
        /// 명렁어 입력에 대한 처리
        /// </summary>
        /// <param name="input"> 입력된 문자열 </param>
        /// <returns> 가공된 문자열 </returns>
        public string ProcessInput(string input)
        {
            // /는 \와 동일한 기능
            input = input.Replace('/', '\\');
            input = input.Trim();
            input = Exception.ChangeDirectoryException(input);
            return input;
        }
        
        /// <summary>
        ///  프로그램 실행 중 계속 실행되는 main loop
        /// </summary>
        public void Loop()
        {
            functions.VersionInfomation();
            string input;
            string command;
            string[] cmds;
            
            while(true)
            {
                output.PrintCurrentPath(currentPath);
                input = ProcessInput(Console.ReadLine());
                cmds = input.Split(' ');

                // 입력된 명령어
                command = cmds[0].ToLower();
                command = command.TrimEnd('.');

                // 명령어 목록 존재 확인, 존재하지 않으면 건너뜀
                if (!cmdList.Contains(command))
                {
                    Console.WriteLine("\'" + command + "\'은(는) 내부 또는 외부 명령, 실행할 수 있는 프로그램, 또는 배치 파일이 아닙니다.");
                    continue;
                }
                else
                {
                    string modifiedPath = Exception.NotSupportedPathException(input.Substring(command.Length));
                    if (modifiedPath == null)
                    {
                        Console.WriteLine("파일 이름, 디렉터리 이름 또는 볼륨 레이블 구문이 잘못되었습니다.");
                        continue;
                    }
                    else if (modifiedPath.Equals("."))
                        input = command + " .";
                }

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
                        secondParam = Exception.ArgumentCountException(cmds, currentPath);
                        if (secondParam != null)
                            functions.Copy(cmds[1], secondParam, currentPath);
                        break;
                    case (int)COMMAND.HELP:
                        functions.PrintHelp(input.Substring(4));
                        break;
                    case (int)COMMAND.MOVE:
                        // 인수 개수에 대한 예외처리
                        secondParam = Exception.ArgumentCountException(cmds, currentPath);
                        if(secondParam != null)
                            functions.Move(cmds[1], secondParam, currentPath);
                        break;
                    case (int)COMMAND.EXIT:
                        return;
                }
            }
        }
    }
}
