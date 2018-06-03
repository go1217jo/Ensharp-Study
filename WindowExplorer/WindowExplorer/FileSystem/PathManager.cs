using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WindowExplorer.FileSystem
{
    class PathManager
    {
        string currentPath;
        MainWindow window;
        StringComparison comp;

        // 뒤로가기를 가능하게 하는 스택
        Stack<string> backPathStack;
        // 앞으로 가기를 가능하게 하는 스택
        Stack<string> frontPathStack;

        public PathManager(MainWindow window)
        {
            this.window = window;
            currentPath = "C:\\";

            // 대소문자 미구분자
            comp = StringComparison.OrdinalIgnoreCase;

            backPathStack = new Stack<string>();
            frontPathStack = new Stack<string>();
        }

        public string ProcessInput(string input)
        {
            // /는 \와 동일한 기능
            input = input.Replace('/', '\\');
            input = input.Trim();
            input = Exception.ChangeDirectoryException(input);
            return input;
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
        ///  이동하고자 하는 해당 경로의 절대 경로 문자열을 반환
        /// </summary>
        /// <param name="movePath"> 이동하고자 하는 경로 </param>
        /// <param name="currentPath"> 현재 있는 경로 </param>
        /// <returns> 경로가 변경되었는지 반환, 변경=true </returns>
        public bool ChangeDirectory(string movePath)
        {
            // 이동된 결과 경로
            string movedPath = currentPath;

            // 앞의 공백 제거
            movePath = movePath.TrimStart(' ');

            if (movePath.Length == 0) {
                window.txt_path.Text = currentPath;
                return false;
            }

            // UNC 경로 예외처리
            if (Exception.UNCPathException(movePath)) {
                window.txt_path.Text = currentPath;
                return false;
            }

            // 지원하지 않는 경로
            string path = Exception.NotSupportedPathException(movePath);
            if (path == null) {
                window.txt_path.Text = currentPath;
                return false;
            }
            movePath = path;

            // 존재하는 절대 경로이면 그대로 반환
            DirectoryInfo directory = new DirectoryInfo(movePath);
            if (!movePath.Contains("..") && !movePath.Contains(".\\") && !movePath.Equals(".") && directory.Exists)
            {
                movePath = movePath.Replace("c:\\", "C:\\");
                movePath = movePath.Replace("d:\\", "D:\\");
                MovePath(movePath);
                return true;
            }

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
                    if (!IsRootDirectory(movedPath))
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
                    if (IsRootDirectory(movedPath))
                        temp = movedPath + unitPath[idx];
                    else
                        // 루트 디렉터리가 아니면 구분자를 붙임
                        temp = movedPath + '\\' + unitPath[idx];

                    // 이동 예정 폴더가 현재 폴더 내에 존재한다면 이동
                    bool exist = false;

                    for (int child = 0; child < childDirectories.Length; child++)
                    {
                        // 같은 것이 존재
                        if (childDirectories[child].Equals(temp, comp))
                        {
                            movedPath = childDirectories[child];
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) {
                        window.txt_path.Text = currentPath;
                        return false;
                    }
                }
            }
            MovePath(movedPath);
            return true;
        }
        
        public string GetCurrentPath()
        {
            return currentPath;
        }
        
        public void MovePath(string movedPath)
        {
            if(backPathStack.Count == 0)
                window.Btn_Back.Source = new BitmapImage(new Uri("pack://application:,,/icons/left_arrow.png"));
            if (currentPath.Equals(movedPath))
            {
                window.txt_path.Text = movedPath;
                return;
            }
            backPathStack.Push(currentPath);
            currentPath = movedPath;
            window.txt_path.Text = movedPath;
        }

        public void GoToBack()
        {
            if (backPathStack.Count == 0)
                return;
            window.Btn_Back.Source = new BitmapImage(new Uri("pack://application:,,/icons/mouseover_left_arrow.png"));
            if(frontPathStack.Count == 0)
                window.Btn_Front.Source = new BitmapImage(new Uri("pack://application:,,/icons/right_arrow.png"));
            frontPathStack.Push(currentPath);
            currentPath = backPathStack.Pop();
            window.txt_path.Text = currentPath;
            if (backPathStack.Count == 0)
                window.Btn_Back.Source = new BitmapImage(new Uri("pack://application:,,/icons/block_left_arrow.png"));
            else
                window.Btn_Back.Source = new BitmapImage(new Uri("pack://application:,,/icons/left_arrow.png"));
        }

        public void GoToFront()
        {
            if (frontPathStack.Count == 0)
                return;
            window.Btn_Front.Source = new BitmapImage(new Uri("pack://application:,,/icons/mouseover_right_arrow.png"));
            if(backPathStack.Count == 0)
                window.Btn_Back.Source = new BitmapImage(new Uri("pack://application:,,/icons/left_arrow.png"));
            backPathStack.Push(currentPath);
            currentPath = frontPathStack.Pop();
            window.txt_path.Text = currentPath;
            if (frontPathStack.Count == 0)
                window.Btn_Front.Source = new BitmapImage(new Uri("pack://application:,,/icons/block_right_arrow.png"));
            else
                window.Btn_Front.Source = new BitmapImage(new Uri("pack://application:,,/icons/right_arrow.png"));
        }
    }
}
