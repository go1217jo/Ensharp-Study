using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace WindowExplorer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        TreeView.FileSystemViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            txt_path.Text = "C:\\";
            viewModel = new TreeView.FileSystemViewModel(this);
        }
    }
}
