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
      ImageViewControl imageViewControl = new ImageViewControl();

      public MainWindow()
      {
         InitializeComponent();
         MainGrid.Children.Add(mainControl);

         mainControl.btn_image.Click += new RoutedEventHandler(Btn_image_Click);
         mainControl.btn_recent.Click += Btn_recent_Click;
         imageViewControl.btn_back.Click += Btn_back_Click;
      }

      private void Btn_image_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(imageViewControl);
      }

      private void Btn_recent_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void Btn_back_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(mainControl);
      }
   }
}
