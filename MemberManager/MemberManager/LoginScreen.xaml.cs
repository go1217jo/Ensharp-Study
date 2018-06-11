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

            txtID.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txtID.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));

            txtPW.AddHandler(LostFocusEvent, new RoutedEventHandler(PasswordHintEnable));
            txtPW.AddHandler(GotFocusEvent, new RoutedEventHandler(IfFocusPasswordHintEnable));
            label_PW.AddHandler(MouseDownEvent, new RoutedEventHandler(PasswordHintUnable));
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

        public void SetDefaultText(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            if (txtbox.Text.Length == 0)
            {
                txtbox.Text = "아이디";
                txtbox.Foreground = Brushes.Gray;
            }
        }

        public void SetBlank(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;

            if (txtbox.Text.Equals("아이디"))
            {
                txtbox.Text = "";
                txtbox.Foreground = Brushes.Black;
            }
        }

        public void PasswordHintUnable(object sender, RoutedEventArgs e)
        {
            Label label = (Label)sender;
            label.Visibility = Visibility.Hidden;
            txtPW.Focus();
        }

        public void IfFocusPasswordHintEnable(object sender, RoutedEventArgs e)
        {
            label_PW.Visibility = Visibility.Hidden;
            txtPW.Focus();
        }

        public void PasswordHintEnable(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)(sender);
            if (passwordBox.Password.Length == 0)
                label_PW.Visibility = Visibility.Visible;
        }
    }
}
