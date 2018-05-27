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
                cmds = input.Split(' ');

                // 입력된 명령어
                command = cmds[0].ToLower();

                // 명령어 목록 존재 확인
                if(cmdList.Contains(command))
                {
                    switch (cmdList.IndexOf(command))
                    {
                        case (int)COMMAND.CMD:
                            functions.VersionInfomation();
                            break;
                        case (int)COMMAND.CD:
                            currentPath = functions.ChangeDirectory(cmds[1], currentPath);
                            break;
                        case (int)COMMAND.DIR:
                            if (cmds.Length == 1) functions.FileList("", currentPath);
                            else functions.FileList(cmds[1], currentPath);
                            break;
                        case (int)COMMAND.CLS:
                            Console.Clear();
                            break;
                        case (int)COMMAND.HELP:
                            if (cmds.Length == 1) functions.PrintHelp("");
                            else functions.PrintHelp(cmds[1]);
                            break;
                        case (int)COMMAND.EXIT:
                            return;
                    }
                }
            }
        }


    }
}
