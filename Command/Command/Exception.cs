using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Command
{
    class Exception
    {
        // 인수 개수에 따른 예외처리
        public static string ArgumentCountException(string[] cmdSplit)
        {
            // 인수가 1개만 있을 경우, 두 번째 인수를 첫 번째 인수와 동일하게 설정
            if (cmdSplit.Length == 2)
                return cmdSplit[1];
            // 정상적으로 인수가 2개가 들어왔을 경우
            else if (cmdSplit.Length == 3)
                return cmdSplit[2];
            // 인수가 없거나 너무 많을 경우
            else
            {
                Console.WriteLine("명령 구문이 올바르지 않습니다.");
                return null;
            }
        }

        public static bool NotExistFileException(string absolutePath)
        {
            // 파일 정보 객체
            FileInfo srcFile = new FileInfo(absolutePath);

            // 파일의 존재 여부 반환, 있으면 false
            return !srcFile.Exists;
        }

        public static bool UNCPathException(string relativePath)
        {
            // UNC 경로 형식일 경우
            if (Regex.IsMatch(relativePath, @"^[\\][\\]"))
            {
                Console.WriteLine("CMD에서 현재 디렉터리로 UNC 경로를 지원하지 않습니다.");
                return true;
            }
            else
                return false;
        }
    }
}
