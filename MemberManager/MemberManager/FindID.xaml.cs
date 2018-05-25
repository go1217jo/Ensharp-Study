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
    /// FindID.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FindID : UserControl
    {
        Data.DAO DB;

        public FindID(Data.DAO DB)
        {
            InitializeComponent();
            this.DB = DB;
            InitCbxEmail();

            txt_email.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_email.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));
            txt_answer.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_answer.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));

            Btn_find.Click += Btn_find_Click;
        }

        private void Btn_find_Click(object sender, RoutedEventArgs e)
        {
            if (txt_email.Text.Length == 0 || txt_email.Text.Equals("이메일"))
            {
                MessageBox.Show("이메일을 입력해주세요.");
                return;
            }

            string email = txt_email.Text + "@" + Cbx_email.SelectedItem.ToString();
            string id = DB.FindIDByEmail(email);
            if (id == null)
                MessageBox.Show("등록되지 않은 이메일입니다.");
            else
                label_id_by_email.Content = "귀하의 아이디는 " + id + " 입니다.";
        }

        public void InitCbxEmail()
        {
            List<string> items = new List<string>(new string[] { "naver.com", "daum.net", "google.com", "hanmail.net", "nate.com", "sju.kr", "sejong.ac.kr" });
            Cbx_email.ItemsSource = items;
            Cbx_email.SelectedIndex = 0;
        }

        public void SetDefaultText(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            string hint;
            if (txtbox.Equals(txt_email))
                hint = "이메일";
            else
                hint = "질문에 대한 답변";

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
            else
                hint = "질문에 대한 답변";

            if (txtbox.Text.Equals(hint))
            {
                txtbox.Text = "";
                txtbox.Foreground = Brushes.Black;
            }
        }

    }
}
