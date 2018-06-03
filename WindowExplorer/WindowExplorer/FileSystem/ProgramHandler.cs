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
        public void ExecuteProgram(string filePath)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filePath;
                        
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(ProcessExited);
            process.Start();
        }

        public void ProcessExited(object source, EventArgs e)
        {
            ((Process)source).WaitForExit(1000);
            ((Process)source).Dispose();
        }
    }
}
