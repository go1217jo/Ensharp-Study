using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Command.Function
{
    class SubFunctions
    {
        OutputProcessor output;
        StringComparison comp;

        public SubFunctions(OutputProcessor output)
        {
            this.output = output;
            // 대소문자 미구분자
            comp = StringComparison.OrdinalIgnoreCase;
        }
        
        /// <summary>
        ///  볼륨일련번호 얻기
        /// </summary>
        /// <param name="driveName"> 드라이브 이름 </param>
        /// <returns> 볼륨 일련번호 </returns>
        public string GetVolumeNumber(string driveName)
        {
            ManagementObject manageObject = new ManagementObject("win32_logicaldisk.deviceid=\"" + driveName[0] + ":\"");
            manageObject.Get();

            string serial = manageObject["VolumeSerialNumber"].ToString();
            return serial.Remove(4) + '-' + serial.Substring(4);
        }

        /// <summary>
        /// dir 입력할 때 .과 .. 출력
        /// </summary>
        /// <param name="currentPath"> 현재 경로</param>
        public void DirRoot(string currentPath)
        {
            string currentLastModifiedDate = new DirectoryInfo(currentPath).LastWriteTime.ToString("yyyy-MM-dd tt hh:mm" + "    ");
            string parentLastModifiedDate = new DirectoryInfo(currentPath).Parent.LastWriteTime.ToString("yyyy-MM-dd tt hh:mm" + "    ");
            Console.Write(currentLastModifiedDate);
            Console.Write(output.PrintFixString("<DIR>", 15, Constant.LEFT));
            Console.WriteLine(".");
            Console.Write(parentLastModifiedDate);
            Console.Write(output.PrintFixString("<DIR>", 15, Constant.LEFT));
            Console.WriteLine("..");
        }

        // 루트 디렉터리면 true 반환
        public bool IsRootDirectory(string path)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.Name.Equals(path, comp))
                    return true;
            }
            return false;
        }
                
        /// <summary>
        /// 덮어쓰기의 여부를 묻는다
        /// </summary>
        /// <param name="toPath"> target path </param>
        /// <returns> 대답을 반환 </returns>
        public int Overwrite(string toPath)
        {
            // 덮어씌운다는 질문에 대해 올바른 대답을 할 때까지 반복
            while (true)
            {
                Console.Write(toPath.Substring(toPath.LastIndexOf('\\') + 1) + "을(를) 덮어쓰시겠습니까? (Yes/No/All): ");
                string answer = Console.ReadLine().ToLower();
                switch (answer[0])
                {
                    case 'y': return Constant.YES;
                    case 'n': return Constant.NO;
                    case 'a': return Constant.ALL;
                }
            }
        }

        public string GetFileName(string absolutePath)
        {
            // 현재 입력된 절대 경로가 디렉터리가 아니면
            if (!new DirectoryInfo(absolutePath).Attributes.HasFlag(FileAttributes.Directory))
            {
                string[] splits = absolutePath.Split('\\');
                return splits[splits.Length - 1];
            }
            else
                return null;
        }

    }
}
