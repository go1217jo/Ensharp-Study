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
   /// MainControl.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class MainControl : UserControl
   {
      TitleImage titleImage;

      public MainControl()
      {
         InitializeComponent();
         this.title_img_panel.Children.Clear();

         titleImage = new TitleImage();
         this.title_img_panel.Children.Add(titleImage);
      }

      private void btn_image_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void btn_recent_Click(object sender, RoutedEventArgs e)
      {

      }

   }
}
