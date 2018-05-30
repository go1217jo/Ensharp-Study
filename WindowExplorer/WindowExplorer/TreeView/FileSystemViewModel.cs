using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Media;
using System.Windows.Media.Imaging;

namespace WindowExplorer.TreeView
{
    class FileSystemViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        FileSystem.FolderHandler folderHandler = new FileSystem.FolderHandler();
        MainWindow window;
        List<TreeViewItem> rootTrees;

        public FileSystemViewModel(MainWindow window)
        {
            this.window = window;
            rootTrees = new List<TreeViewItem>();
            InitTree();
        }

        /// <summary>
        /// 드라이브 이미지와 이름으로 초기화된 노드를 반환
        /// </summary>
        /// <param name="driveName"> 드라이브 이름 </param>
        /// <returns> stackpanel 객체 반환 </returns>
        public StackPanel GetTreeRootNode(string driveName)
        {
            StackPanel root = new StackPanel();
            Image image = new Image();
            Label name = new Label();

            image.Source = new BitmapImage(new Uri("pack://application:,,/Images/drive.png"));
            image.Width = 18;
            image.Height = 18;
            name.Content = driveName;

            root.Orientation = Orientation.Horizontal;
            root.Children.Add(image);
            root.Children.Add(name);

            return root;
        }

        /// <summary>
        /// 드라이브 정보를 찾아 트리를 초기화한다
        /// </summary>
        public void InitTree()
        {
            // 루트 디렉터리에 드라이브 이름을 넣는다
            DriveInfo[] drives = DriveInfo.GetDrives();
            for (int idx = 0; idx < drives.Length; idx++) {
                TreeViewItem rootItem = new TreeViewItem();
                StackPanel root = GetTreeRootNode(drives[idx].Name);
                rootItem.Header = root;
                rootTrees.Add(rootItem);
            }
            // 트리 초기화
            window.DirectoryTreeView.ItemsSource = rootTrees;
        }
    }
}
