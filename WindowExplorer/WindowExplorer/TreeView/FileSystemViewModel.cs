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
                // 드라이브 하위 폴더를 추가
                AddChildrenTree(rootItem, drives[idx].Name);
                rootTrees.Add(rootItem);
            }
            // 트리 초기화
            window.DirectoryTreeView.ItemsSource = rootTrees;
        }

        /// <summary>
        /// 현재 노드 밑에 자식 트리를 추가한다.
        /// </summary>
        /// <param name="parentNode"> 현재 TreeItem 노드 </param>
        /// <param name="parentPath"> 현재 노드 경로 </param>
        public void AddChildrenTree(TreeViewItem parentNode, string parentPath)
        {
            List<DirectoryInfo> infors = folderHandler.GetDirectoryList(parentPath);
            List<string> foldernames = folderHandler.GetDirectoryNameList(infors);

            for (int idx = 0; idx < infors.Count; idx++) { 
                StackPanel node = new StackPanel();
                Image image = new Image();
                image.Width = 18;
                image.Height = 18;
                Label foldername = new Label();

                // 숨김 폴더인지 아닌지에 따라 아이콘 이미지를 다르게 함
                if (infors[idx].Attributes.HasFlag(FileAttributes.Hidden))
                    image.Source = new BitmapImage(new Uri("pack://application:,,/Images/hiddenFolder.png"));
                else
                    image.Source = new BitmapImage(new Uri("pack://application:,,/Images/closeFolder.png"));

                foldername.Content = foldernames[idx];
                node.Orientation = Orientation.Horizontal;
                node.Children.Add(image);
                node.Children.Add(foldername);

                parentNode.Items.Add(node);
            }
        }
    }
}
