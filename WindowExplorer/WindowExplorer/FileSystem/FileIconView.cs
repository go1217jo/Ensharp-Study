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
            ImageSource icon;
            // 파일이 존재하면
            if (new FileInfo(filename).Exists)
                return null;
            using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filename))
            {
                icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                            sysicon.Handle,
                            System.Windows.Int32Rect.Empty,
                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            return icon;
        }

        public void SetFileViewPanel(string path)
        {
            List<DirectoryInfo> infors = folderHandler.GetDirectoryList(path);
            filePanel.Children.Clear();

            for (int idx = 0; idx < infors.Count; idx += 5)
            {
                StackPanel panel = new StackPanel();
                for(int pos = idx; pos < idx + 5; pos++)
                {
                    if (pos == infors.Count)
                        break;
                    Image image = new Image();
                    image.Source = getIcon(infors[pos].FullName);
                    image.Width = 32;
                    image.Height = 32;
                    panel.Children.Add(image);
                }
                filePanel.Children.Add(panel);
            }
        }
    }
}