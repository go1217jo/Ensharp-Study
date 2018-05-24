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
    /// FindPW.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FindPW : UserControl
    {
        public FindPW()
        {
            InitializeComponent();
            findID_Click.AddHandler(MouseMoveEvent, new RoutedEventHandler(Label_MouseUp));
            findID_Click.AddHandler(MouseLeaveEvent, new RoutedEventHandler(Label_MouseLeave));
            txt_ID.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_ID.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));
        }

        private void Label_MouseUp(object sender, RoutedEventArgs e)
        {
            ((Label)sender).FontWeight = FontWeights.UltraBold;
            ((Label)sender).FontSize = 18;
        }

        private void Label_MouseLeave(object sender, RoutedEventArgs e)
        {
            ((Label)sender).FontWeight = FontWeights.Regular;
            ((Label)sender).FontSize = 16;
        }

        public void SetDefaultText(object sender, RoutedEventArgs e)
        {
            if (txt_ID.Text.Length == 0)
            {
                txt_ID.Text = "찾고자 하는 아이디";
                txt_ID.Foreground = Brushes.Gray;
            }
        }

        public void SetBlank(object sender, RoutedEventArgs e)
        {
            if (txt_ID.Text.Equals("찾고자 하는 아이디"))
            {
                txt_ID.Text = "";
                txt_ID.Foreground = Brushes.Black;
            }
        }

    }
}
