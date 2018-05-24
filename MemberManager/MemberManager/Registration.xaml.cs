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
    /// Registration.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            Init();
        }

        public void InitCbxEmail()
        {
            List<string> items = new List<string>(new string[] { "naver.com", "daum.net", "google.com", "hanmail.net", "nate.com", "sju.kr", "sejong.ac.kr" });
            Cbx_email.ItemsSource = items;
            Cbx_email.SelectedIndex = 0;
        }

        public void InitCbxMonth()
        {
            List<string> items = new List<string>(new string[] { "1", "2","3","4","5","6","7", "8", "9", "10", "11", "12" });
            Cbx_month.ItemsSource = items;
            Cbx_month.SelectedIndex = 0;
        }

        public void Init()
        {
            InitCbxEmail();
            InitCbxMonth();
            txt_email.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_email.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));
            txt_name.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_name.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));
            txt_ID.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_ID.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));

            txt_PW.AddHandler(LostFocusEvent, new RoutedEventHandler(PasswordHintEnable));
            label_PW.AddHandler(MouseDownEvent, new RoutedEventHandler(PasswordHintUnable));
            txt_PW_check.AddHandler(LostFocusEvent, new RoutedEventHandler(PasswordHintEnable));
            label_PW_check.AddHandler(MouseDownEvent, new RoutedEventHandler(PasswordHintUnable));
        }

        public void PasswordHintUnable(object sender, RoutedEventArgs e)
        {
            Label label = (Label)sender;
            label.Visibility = Visibility.Hidden;

            if (label.Equals(label_PW))
                txt_PW.Focus();
            else
                txt_PW_check.Focus();
        }

        public void PasswordHintEnable(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)(sender);
            if (passwordBox.Password.Length == 0)
            {
                if (passwordBox.Equals(txt_PW))
                    label_PW.Visibility = Visibility.Visible;
                else
                    label_PW_check.Visibility = Visibility.Visible;
            }
        }

        public void SetDefaultText(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            string hint;
            if (txtbox.Equals(txt_email))
                hint = "이메일";
            else if (txtbox.Equals(txt_name))
                hint = "이름";
            else
                hint = "아이디";

            if (txtbox.Text.Length == 0)
            {
                txtbox.Text = hint;
                txtbox.Foreground = Brushes.Gray;
            }
        }

        public void SetBlank(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            string hint;
            if (txtbox.Equals(txt_email))
                hint = "이메일";
            else if (txtbox.Equals(txt_name))
                hint = "이름";
            else
                hint = "아이디";

            if (txtbox.Text.Equals(hint))
            {
                txtbox.Text = "";
                txtbox.Foreground = Brushes.Black;
            }
        }
    }
}
