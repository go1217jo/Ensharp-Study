using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace WindowExplorer.FileSystem
{
    class ProgramHandler
    {
        Process process;
        Dictionary<string, string> programPath;


        public ProgramHandler()
        {
            process = new Process();
            programPath = new Dictionary<string, string>();
            programPath.Add("pdf", "C:\\Program Files (x86)\\Adobe\\Acrobat Reader DC\\Reader\\AcroRd32.exe");
            programPath.Add("txt", "C:\\Windows\\System32\\notepad.exe");
            programPath.Add("zip", "C:\\Program Files\\Bandizip\\Bandizip.exe");
            programPath.Add("html", "C:\\Program Files (x86)\\Naver\\Naver Whale\\Application\\whale.exe");
            programPath.Add("pptx", "C:\\Program Files (x86)\\Microsoft Office\\root\\Office16\\POWERPNT.EXE");
            programPath.Add("xlsx", "C:\\Program Files (x86)\\Microsoft Office\\root\\Office16\\EXCEL.EXE");
            programPath.Add("docx", "C:\\Program Files (x86)\\Microsoft Office\\root\\Office16\\WINWORD.EXE");
        }

        // 확장자에 따른 실행 프로그램 경로를 반환
        public string ProgramByExtension(string extension)
        {
            // 실행할 파일이 exe가 아니라면
            if (!extension.Equals("exe"))
            {
                if (programPath.ContainsKey(extension))
                    return programPath[extension];
                else
                    return programPath["txt"];
            }
            return null;
        }

        public void ExecuteProgram(string filePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            string programName = ProgramByExtension(new DirectoryInfo(filePath).Extension);
            if (programName == null)
                startInfo.FileName = filePath;
            else
            {
                startInfo.FileName = programName;
                startInfo.Arguments = filePath;
            }
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(ProcessExited);
            process.Start();
        }

        public void ProcessExited(object source, EventArgs e)
        {
            process.WaitForExit(1000);
            process.Dispose();
        }
    }
}
