using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace WindowExplorer.FileSystem
{
    class FileIconView
    {
        // 처음 클릭시간 저장
        DateTime mouseLastClick = DateTime.Now.AddSeconds(-1);

        FolderHandler folderHandler;
        StackPanel filePanel;
        ProgramHandler programHandler;
        MainWindow window;

        // 클릭 이벤트와 더블 클릭 이벤트 중첩 방지
        int clickCount = 0;

        public FileIconView(MainWindow window)
        {
            folderHandler = new FolderHandler();
            this.window = window;
            filePanel = window.filePanel;
            programHandler = new ProgramHandler();
            
        }

        /// System.Drawing.Icon을 이용하여 아이콘을 추출하는 함수
        public ImageSource getIcon(string filename)
        {
            ImageSource icon = null;
            // 파일이 존재하면
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Exists && !fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
            {
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
                DirectoryInfo directoryInfo = new DirectoryInfo(filename);
                if (directoryInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    // 숨김 폴더인지 아닌지에 따라 아이콘 이미지를 다르게 함
                    if (directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                        icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,/Images/hiddenFolder.png"));
                    else
                        icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,/Images/closeFolder.png"));
                }
            }
            return icon;
        }

        public void SetFileViewPanel(string path)
        {
            List<DirectoryInfo> infors = folderHandler.GetFileSystemList(path);
            if (infors == null)
            {
                MessageBox.Show("접근 권한이 없습니다.");
                return;
            }
            List<string> nameList = folderHandler.GetDirectoryNameList(infors);

            filePanel.Children.Clear();
            int columnCount = (int)filePanel.ActualWidth / 80;

            for (int idx = 0; idx < infors.Count; idx += columnCount)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;

                for(int pos = idx; pos < idx + columnCount; pos++)
                {
                    if (pos == infors.Count)
                        break;
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

        private string AdjustTextLength(string filename)
        {
            int pos = 0;
            while (filename.Length > pos)
            {
                if (filename.Length - pos >= 11)
                    filename = filename.Substring(0, pos + 10) + "\n" + filename.Substring(pos + 10);
                pos += 10;
            }
            return filename;
        }

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

            for (int i = 0; i < filePanel.Children.Count; i++)
            {
                StackPanel rowPanel = (StackPanel)(filePanel.Children[i]);
                for(int j=0;j<rowPanel.Children.Count; j++)
                    ((StackPanel)rowPanel.Children[j]).Background = Brushes.White;
            }

            panel.Background = Brushes.LightSkyBlue;
        }

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
                    window.txt_path.Text = path;
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
    }
}