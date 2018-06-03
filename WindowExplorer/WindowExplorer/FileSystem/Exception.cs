using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowExplorer.FileSystem
{
    class Exception
    {
        // 파일이 존재하지 않으면 true
        public static bool NotExistFileException(string absolutePath)
        {
            // 파일 정보 객체
            FileInfo srcFile = new FileInfo(absolutePath);

            // 파일의 존재 여부 반환, 있으면 false
            return !srcFile.Exists;
        }

        // unc경로 형식일 경우 true
        public static bool UNCPathException(string relativePath)
        {
            // UNC 경로 형식일 경우
            if (Regex.IsMatch(relativePath, @"^[\\][\\]"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// cd 공백 없이 바로 뒤 \이 붙으면 명령어를 인식함
        /// </summary>
        /// <param name="input"> 가공된 입력문자열 (앞뒤 공백이 제거된) </param>
        /// <returns></returns>
        public static string ChangeDirectoryException(string input)
        {
            if (Regex.IsMatch(input, @"^[c][d][\\]") || Regex.IsMatch(input, @"^[c][d][.]"))
                input = input.Insert(2, " ");
            return input;
        }

        /// <summary>
        ///  지원되지 않는 경로에 대한 예외
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NotSupportedPathException(string path)
        {
            path = path.Trim().ToLower();
            if (Regex.IsMatch(path, @"^[c][d]"))
                return null;
            else if (Regex.IsMatch(path, @"^[c][::]") && !path.Contains("c:\\"))
                return ".";
            else
                return path;
        }
    }
}
