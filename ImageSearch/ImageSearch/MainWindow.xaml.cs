using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageSearch
{
   /// <summary>
   /// MainWindow.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class MainWindow : Window
   {
      MainControl mainControl = new MainControl();
      RecentViewControl recentViewControl = new RecentViewControl();
      ImageViewControl imageViewControl;
      Data.DBHandler DB = new Data.DBHandler();

      public MainWindow()
      {
         InitializeComponent();
         this.ResizeMode = ResizeMode.NoResize; 
         imageViewControl = new ImageViewControl(DB);
         
         MainGrid.Children.Add(mainControl);
         
         mainControl.btn_image.Click += new RoutedEventHandler(Btn_image_Click);
         mainControl.btn_recent.Click += Btn_recent_Click;
         imageViewControl.btn_back.AddHandler(MouseDownEvent, new RoutedEventHandler(Btn_back_Click));
         recentViewControl.btn_back.AddHandler(MouseDownEvent, new RoutedEventHandler(Btn_back_Click));
         recentViewControl.btn_deleteLog.Click += Btn_deleteLog_Click;
         recentViewControl.btn_clear.Click += Btn_clear_Click;
      }

      private void Btn_image_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(imageViewControl);
      }

      private void Btn_recent_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         recentViewControl.list_log.ItemsSource = DB.ViewAllLog();
         MainGrid.Children.Add(recentViewControl);
      }

      private void Btn_back_Click(object sender, RoutedEventArgs e)
      {
         imageViewControl.viewPanel.Children.Clear();
         imageViewControl.txtSearchBox.Text = "";
         imageViewControl.cbx_count.SelectedIndex = 0;

         MainGrid.Children.Clear();
         MainGrid.Children.Add(mainControl);
      }

      private void Btn_deleteLog_Click(object sender, RoutedEventArgs e)
      {
         Data.Log deleteItem = (Data.Log)recentViewControl.list_log.SelectedItem;
         DB.DeleteLog(deleteItem.LogTime);
         recentViewControl.list_log.ItemsSource = DB.ViewAllLog();
      }

      private void Btn_clear_Click(object sender, RoutedEventArgs e)
      {
         DB.ClearTable();
         recentViewControl.list_log.ItemsSource = DB.ViewAllLog();
      }
   }
}
