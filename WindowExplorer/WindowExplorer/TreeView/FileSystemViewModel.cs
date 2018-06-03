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
using System.Windows;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Media;
using System.Windows.Input;

namespace WindowExplorer.TreeView
{
    class FileSystemViewModel
    {
        FileSystem.FolderHandler folderHandler = new FileSystem.FolderHandler();
        MainWindow window;
        List<TreeViewItem> rootTrees;
        FileSystem.FileIconView iconView;
        FileSystem.PathManager pathManager;

        public FileSystemViewModel(MainWindow window)
        {
            this.window = window;
            this.window.txt_path.KeyDown += ChangeDirectoryEvent;
            this.window.txt_path.LostFocus += TextPathLostFocusEvent;
            rootTrees = new List<TreeViewItem>();
            InitTree();
            pathManager = new FileSystem.PathManager(window);
            iconView = new FileSystem.FileIconView(window, pathManager);
            window.Btn_Back.MouseUp += GoToBackEvent;
            window.Btn_Front.MouseUp += GoToFrontEvent;
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
            System.Windows.Controls.Label name = new System.Windows.Controls.Label();

            image.Source = new BitmapImage(new Uri("pack://application:,,/Images/drive.png"));
            image.Width = 18;
            image.Height = 18;
            name.Content = driveName;

            root.Orientation = System.Windows.Controls.Orientation.Horizontal;
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
                root.MouseLeftButtonDown += SetFileViewEvent;
                rootItem.Header = root;

                // 드라이브 하위 폴더를 추가
                AddChildrenTree(rootItem, drives[idx].Name);
                rootItem.Expanded += TreeViewExpandEvent;
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
            // 이미 자식 노드가 추가 되어있으면 함수를 실행하지 않는다.
            if (parentNode.ItemsSource != null)
                return;

            List<DirectoryInfo> infors = folderHandler.GetDirectoryList(parentPath);
            if (infors == null)
                return;
            List<string> foldernames = folderHandler.GetDirectoryNameList(infors);
            List<TreeViewItem> childNodes = new List<TreeViewItem>();

            for (int idx = 0; idx < infors.Count; idx++) {
                TreeViewItem nodeItem = new TreeViewItem();
                StackPanel node = new StackPanel();
                Image image = new Image();
                image.Width = 18;
                image.Height = 18;
                System.Windows.Controls.Label foldername = new System.Windows.Controls.Label();

                // 숨김 폴더인지 아닌지에 따라 아이콘 이미지를 다르게 함
                if (infors[idx].Attributes.HasFlag(FileAttributes.Hidden))
                    image.Source = new BitmapImage(new Uri("pack://application:,,/Images/hiddenFolder.png"));
                else
                    image.Source = new BitmapImage(new Uri("pack://application:,,/Images/closeFolder.png"));

                foldername.Content = foldernames[idx];
                node.Orientation = System.Windows.Controls.Orientation.Horizontal;
                node.Children.Add(image);
                node.Children.Add(foldername);
                node.MouseLeftButtonDown += SetFileViewEvent;

                nodeItem.Header = node;
                nodeItem.Expanded += TreeViewExpandEvent;
                childNodes.Add(nodeItem);
            }
            parentNode.ItemsSource = childNodes;
        }

        /// <summary>
        /// 현재 트리를 확장하면 실행되는 이벤트 메서드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TreeViewExpandEvent(object sender, RoutedEventArgs e)
        {
            TreeViewItem nodeItem = (TreeViewItem)sender;
            
            string path = GetFullPath(nodeItem).TrimEnd('\\');

            List<TreeViewItem> childNodes = (List<TreeViewItem>)nodeItem.ItemsSource;
            if (childNodes == null)
                return;
            for (int idx = 0; idx < childNodes.Count; idx++)
            {
                string childPath = path + '\\' + ((System.Windows.Controls.Label)(((StackPanel)(childNodes[idx].Header)).Children[1])).Content;
                DirectoryInfo info = new DirectoryInfo(childPath);
                if (childNodes[idx].ItemsSource != null)
                    continue;
                // 불러올 수 없는 폴더가 아니면
                if(!info.Attributes.HasFlag(FileAttributes.NotContentIndexed))
                    AddChildrenTree(childNodes[idx], childPath);
            }
            // 트리 갱신
            window.DirectoryTreeView.ItemsSource = rootTrees;
        }

        public string GetFullPath(TreeViewItem nodeItem)
        {
            string fullPath = "";
            TreeViewItem parentItem = nodeItem;
            while(parentItem != null)
            {
                StackPanel header = new StackPanel();
                header = (StackPanel)parentItem.Header;
                fullPath = "\\" + ((System.Windows.Controls.Label)header.Children[1]).Content + fullPath;

                parentItem = (TreeViewItem)GetSelectedTreeViewItemParent(parentItem);
            }
            fullPath = fullPath.Replace("\\\\", "\\");
            return fullPath.TrimStart(new char[] { '\\', ' ' });
        }

        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            if (parent == null)
                return null;
            while (parent != null && !(parent is TreeViewItem))
                parent = VisualTreeHelper.GetParent(parent);

            return parent as ItemsControl;
        }

        public void SetFileViewEvent(object sender, RoutedEventArgs e)
        {
            StackPanel item = (StackPanel)sender;
            TreeViewItem node = (TreeViewItem)item.Parent;
            string path = GetFullPath(node);
            pathManager.ChangeDirectory(path);
            iconView.SetFileViewPanel(path);
        }
                        
        public void ChangeDirectoryEvent(object sender, KeyEventArgs e)
        {
            // 엔터가 입력되었을 때 텍스트박스에 입력된 경로로 이동           
            if(e.Key == Key.Enter)
            {
                if( pathManager.ChangeDirectory(window.txt_path.Text))
                    iconView.SetFileViewPanel(window.txt_path.Text);
            }
        }

        public void TextPathLostFocusEvent(object sender, RoutedEventArgs e)
        {
            window.txt_path.Text = pathManager.GetCurrentPath();
        }

        public void GoToBackEvent(object sender, RoutedEventArgs e)
        {
            pathManager.GoToBack();
            iconView.SetFileViewPanel(pathManager.GetCurrentPath());
        }

        public void GoToFrontEvent(object sender, RoutedEventArgs e)
        {
            pathManager.GoToFront();
            iconView.SetFileViewPanel(pathManager.GetCurrentPath());
        }
    }
}
