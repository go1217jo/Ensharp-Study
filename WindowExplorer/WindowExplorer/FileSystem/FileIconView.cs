using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Runtime;

namespace WindowExplorer.FileSystem
{
    class FileIconView
    {
        // 처음 클릭시간 저장
        DateTime mouseLastClick = DateTime.Now.AddSeconds(-1);

        FolderHandler folderHandler;
        StackPanel filePanel;
        ProgramHandler programHandler;
        PathManager pathManager;
        MainWindow window;

        // 클릭 이벤트와 더블 클릭 이벤트 중첩 방지
        int clickCount = 0;

        public FileIconView(MainWindow window, PathManager pathManager)
        {
            folderHandler = new FolderHandler();
            this.window = window;
            filePanel = window.filePanel;
            programHandler = new ProgramHandler();
            this.pathManager = pathManager;
            window.SizeChanged += WindowResizeEvent;
            window.Btn_search.Click += FindFileByName;

            // 가비지 콜렉터
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        }

        /// System.Drawing.Icon을 이용하여 아이콘을 추출하는 함수
        public ImageSource getIcon(string filename)
        {
            ImageSource icon = null;
            
            // 파일이 존재하면
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Exists && !fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
            {
                // 파일 아이콘 불러오기
                using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filename))
                {
                    icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                sysicon.Handle,
                                System.Windows.Int32Rect.Empty,
                                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                }
            }
            else
            {
                // 디렉터리이면
                DirectoryInfo directoryInfo = new DirectoryInfo(filename);
                if (directoryInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    // 숨김 폴더인지 아닌지에 따라 아이콘 이미지를 다르게 함
                    if (directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                        icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,/icons/hiddenFolder.png"));
                    else
                        icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,/icons/closeFolder.png"));
                }
            }
            return icon;
        }

        /// <summary>
        ///  경로에 따라 filePanel에 파일 및 폴더 아이콘을 렌더링한다.
        /// </summary>
        /// <param name="path"> 렌더링하려는 절대경로 </param>
        public void SetFileViewPanel(string path)
        {
            
            List<DirectoryInfo> infors = folderHandler.GetFileSystemList(path);
            // 권한예외가 발생한 경우
            if (infors == null)
            {
                MessageBox.Show("접근 권한이 없습니다.");
                return;
            }
            // 파일 및 이름 목록을 얻어옴
            List<string> nameList = folderHandler.GetDirectoryNameList(infors);

            // 패널을 초기화하고 가비지 콜렉터를 실행
            filePanel.Children.Clear();
            GC.Collect();

            // 현재 넓이에 따라 열에 둘 아이콘 수를 결정함
            int columnCount = (int)filePanel.ActualWidth / 85;

            // filePanel에 넣을 2차원 아이콘 스택 패널을 생성한다.
            for (int idx = 0; idx < infors.Count; idx += columnCount)
            {
                // 행 StackPanel 생성
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;

                for(int pos = idx; pos < idx + columnCount; pos++)
                {
                    if (pos == infors.Count)
                        break;
                    
                    // 개별 아이콘 생성
                    StackPanel file = new StackPanel();
                    file.Orientation = Orientation.Vertical;
                    file.MouseDown += IconClickEvent;
                    file.MouseMove += IconMouseOverEvent;
                    file.MouseLeave += IconMoveLeaveEvent;
                    file.MouseLeftButtonDown += IconDoubleClickEvent;

                    Image image = new Image();
                    image.Margin = new System.Windows.Thickness(0, 20, 20, 20);
                    image.Source = getIcon(infors[pos].FullName);
                    image.Width = 64;
                    image.Height = 64;
                    Label filename = new Label();
                    filename.Content = AdjustTextLength(nameList[pos]);

                    file.Children.Add(image);
                    file.Children.Add(filename);

                    panel.Children.Add(file);
                }
                filePanel.Children.Add(panel);
            }
        }

        /// <summary>
        /// 현재 폴더에서 파일 이름을 검색하는 이벤트 발생
        /// </summary>
        private void FindFileByName(object sender, RoutedEventArgs e)
        {
            string name = window.txt_search.Text;
            window.txt_search.Text = "";

            StackPanel rootPanel = window.filePanel;
            List<StackPanel> searchResult = new List<StackPanel>();

            // 현재 렌더링된 파일 및 폴더명을 검사함
            for (int i = 0; i < rootPanel.Children.Count; i++)
            {
                StackPanel rowPanel = (StackPanel)(filePanel.Children[i]);
                for (int j = 0; j < rowPanel.Children.Count; j++)
                {
                    StackPanel icon = (StackPanel)rowPanel.Children[j];
                    string compared = ((string)(((Label)icon.Children[1]).Content)).ToLower();
                    if (compared.Contains(name.ToLower()))
                        searchResult.Add(icon);
                }
                rowPanel.Children.Clear();
                GC.Collect();
            }
            window.filePanel.Children.Clear();
            GC.Collect();

            if (searchResult.Count == 0)
                MessageBox.Show("검색결과가 없습니다.");
            else
            {
                // 검색된 아이콘들을 가지고 2차원 스택패널을 만듬
                int columnCount = (int)filePanel.ActualWidth / 85;
                for (int idx = 0; idx < searchResult.Count; idx += columnCount)
                {
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;

                    for (int pos = idx; pos < idx + columnCount; pos++)
                    {
                        if (pos == searchResult.Count)
                            break;
                        panel.Children.Add(searchResult[pos]);
                    }
                    window.filePanel.Children.Add(panel);
                }
            }
        }

        /// <summary>
        ///  아이콘의 파일명의 줄내림 조절
        /// </summary>
        /// <param name="filename">파일명</param>
        /// <returns>변환된 파일명</returns>
        private string AdjustTextLength(string filename)
        {
            int pos = 0;
            // 10글자마다 엔터를 삽입
            while (filename.Length > pos)
            {
                if (filename.Length - pos >= 11)
                    filename = filename.Substring(0, pos + 10) + "\n" + filename.Substring(pos + 10);
                pos += 10;
            }
            return filename;
        }

        /// <summary>
        /// 아이콘 클릭 시 발생하는 이벤트, 배경색 변경
        /// </summary>
        private void IconClickEvent(object sender, RoutedEventArgs e)
        {
            if (clickCount > 1)
            {
                clickCount = 0;
                return;
            }
            
            StackPanel panel = (StackPanel)sender;
            // 아이콘들이 나타나는 파일 StackPanel
            StackPanel filePanel = (StackPanel)(((StackPanel)(panel.Parent)).Parent);
            if (filePanel == null)
                return;

            // 한 번에 클릭된 아이콘은 하나기 때문에 나머지 아이콘들의 배경을 하얀색으로 변경
            for (int i = 0; i < filePanel.Children.Count; i++)
            {
                StackPanel rowPanel = (StackPanel)(filePanel.Children[i]);
                for(int j=0;j<rowPanel.Children.Count; j++)
                    ((StackPanel)rowPanel.Children[j]).Background = Brushes.White;
            }

            panel.Background = Brushes.LightSkyBlue;
        }

        // 아이콘 위에 마우스가 올려지면 발생하는 이벤트
        private void IconMouseOverEvent(object sender, RoutedEventArgs e)
        {
            StackPanel panel = (StackPanel)sender;
            if(panel.Background != Brushes.LightSkyBlue)
                panel.Background = Brushes.AliceBlue;
        }

        private void IconMoveLeaveEvent(object sender, RoutedEventArgs e)
        {
            StackPanel panel = (StackPanel)sender;
            if (panel.Background != Brushes.LightSkyBlue)
                panel.Background = Brushes.White;
        }

        // 아이콘 더블 클릭 시 프로그램 실행 또는 폴더 열기
        private void IconDoubleClickEvent(object sender, RoutedEventArgs e)
        {
            if (((DateTime.Now - mouseLastClick).Seconds < 1) && ((DateTime.Now - mouseLastClick).Milliseconds < 200))
            {
                Label filename = (Label)(((StackPanel)sender).Children[1]);

                string path = window.txt_path.Text + "\\" + ((string)filename.Content).Replace("\n", "");
                path = path.Replace("\\\\", "\\");

                // 경로가 폴더 경로이면
                if (new DirectoryInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                {
                    pathManager.MovePath(path);
                    SetFileViewPanel(path);
                }
                else
                    // 폴더가 아니면 실행
                    programHandler.ExecuteProgram(path);
                
                mouseLastClick = DateTime.Now.AddSeconds(-1);
                clickCount = 2;
            }
            else
                mouseLastClick = DateTime.Now;
        }

        // 윈도우 크기가 변경되면 발생하는 이벤트
        private void WindowResizeEvent(object sender, RoutedEventArgs e)
        {
            SetFileViewPanel(pathManager.GetCurrentPath());
        }
    }
}