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

namespace MemberManager
{
   /// <summary>
   /// LoginScreen.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class LoginScreen : UserControl
   {
      public LoginScreen()
      {
         InitializeComponent();
         List<Label> clickableLabels = new List<Label>(new Label[] { findID, findPW, register });
         for (int idx = 0; idx < clickableLabels.Count; idx++)
         {
            clickableLabels[idx].AddHandler(MouseMoveEvent, new RoutedEventHandler(Label_MouseUp));
            clickableLabels[idx].AddHandler(MouseLeaveEvent, new RoutedEventHandler(Label_MouseLeave));
         }
      }

      private void Label_MouseUp(object sender, RoutedEventArgs e)
      {
         ((Label)sender).FontWeight = FontWeights.UltraBold;
         ((Label)sender).FontSize = 20;
      }

      private void Label_MouseLeave(object sender, RoutedEventArgs e)
      {
         ((Label)sender).FontWeight = FontWeights.Regular;
         ((Label)sender).FontSize = 18;
      }      
   }
}
