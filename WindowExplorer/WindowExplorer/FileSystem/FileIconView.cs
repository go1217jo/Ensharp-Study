using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;

namespace WindowExplorer.FileSystem
{
    class FileIconView
    {
        FolderHandler folderHandler;
        StackPanel filePanel;

        public FileIconView(MainWindow window)
        {
            folderHandler = new FolderHandler();
            filePanel = window.filePanel;
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
                    Image image = new Image();
                    image.Margin = new System.Windows.Thickness(0, 20, 20, 20);
                    image.Source = getIcon(infors[pos].FullName);
                    image.Width = 64;
                    image.Height = 64;
                    Label filename = new Label();
                    filename.Content = nameList[pos];

                    file.Children.Add(image);
                    file.Children.Add(filename);

                    panel.Children.Add(file);
                }
                filePanel.Children.Add(panel);
            }
        }

    }
}