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
    /// FindPWbyEmail.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FindPWbyEmail : UserControl
    {
        Data.DAO DB;
        Mail mail;
        string certificationNo;

        public FindPWbyEmail(Data.DAO DB)
        {
            InitializeComponent();
            this.DB = DB;
            
            InitCbxEmail();
            txt_email.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_email.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));
            Btn_certificate.Click += Btn_Certificate_Click;
            Btn_resubmit.Click += Btn_resubmit_Click;
            Btn_submit.Click += SendCheckMail;
        }

        private void Btn_resubmit_Click(object sender, RoutedEventArgs e)
        {
            certificationNo = ReturnRandomString();
            mail.SendEmail("En# 스터디 본인확인 이메일입니다.", "인증번호는 " + certificationNo + "입니다.");

            label_mailStatus.Content = "재발송됨";
        }

        public void SetDefaultText(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            
            if (txtbox.Text.Length == 0)
            {
                txtbox.Text = "이메일";
                txtbox.Foreground = Brushes.Gray;
            }
        }

        public void SetBlank(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            
            if (txtbox.Text.Equals("이메일"))
            {
                txtbox.Text = "";
                txtbox.Foreground = Brushes.Black;
            }
        }

        public string ReturnRandomString()
        {
            Random rand = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 8).Select(x => input[rand.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        public void InitCbxEmail()
        {
            List<string> items = new List<string>(new string[] { "naver.com", "daum.net", "google.com", "hanmail.net", "nate.com", "sju.kr", "sejong.ac.kr" });
            Cbx_email.ItemsSource = items;
            Cbx_email.SelectedIndex = 0;
        }

        public void SendCheckMail(object sender, RoutedEventArgs e)
        {
            mail = new Mail("go1217jo@gmail.com");
            if (txt_email.Text.Equals("이메일"))
            {
                MessageBox.Show("이메일을 입력해주세요.");
                return;
            }
            else
            {
                string email = txt_email.Text + "@" + Cbx_email.SelectedItem.ToString();
                if (DB.FindIDByEmail(email) == null)
                {
                    MessageBox.Show("등록되지 않은 이메일입니다.");
                    return;
                }
            }

            mail.SetToAddress(txt_email.Text + '@' + Cbx_email.SelectedItem.ToString());
            certificationNo = ReturnRandomString();
            mail.SendEmail("En# 스터디 본인확인 이메일입니다.", "인증번호는 " + certificationNo + "입니다.");

            label_mailStatus.Content = "발송됨";

            Btn_resubmit.IsEnabled = true;
            Btn_certificate.IsEnabled = true;
        }

        public void Btn_Certificate_Click(object sender, RoutedEventArgs e)
        {
            if (txt_certificate.Text.Equals(certificationNo))
            {
                MessageBox.Show("인증되었습니다!");
                label_mailStatus.Content = "";
                Btn_certificate.IsEnabled = false;
                Btn_resubmit.IsEnabled = false;
                mail.Close();
                string password = DB.FindPasswordByEmail(mail.GetToAddress());
                Label_Show_Password.Content = "귀하의 비밀번호는 " + password + " 입니다.";
            }
            else
                MessageBox.Show("인증 번호를 확인해주세요!");
        }
    }

}
