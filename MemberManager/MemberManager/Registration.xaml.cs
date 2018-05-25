using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
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
        // 1이면 남자, 2이면 여자
        int sex = 0;

        // 이메일 인증 여부
        bool certification = false;
        string certificationNo = null;
        Mail mail;

        Data.DAO DB = null;

        public Registration(Data.DAO DB)
        {
            InitializeComponent();
            Init();
            this.DB = DB;
        }

        public void InitCbxEmail()
        {
            List<string> items = new List<string>(new string[] { "naver.com", "daum.net", "google.com", "hanmail.net", "nate.com", "sju.kr", "sejong.ac.kr" });
            Cbx_email.ItemsSource = items;
            Cbx_email.SelectedIndex = 0;
        }

        public void InitCbxMonth()
        {
            List<string> items = new List<string>(new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" });
            Cbx_month.ItemsSource = items;
            Cbx_month.SelectedIndex = 0;
        }

        public void Init()
        {
            // 콤보박스 아이템 초기화
            InitCbxEmail();
            InitCbxMonth();

            // 일반 텍스트박스 힌트 이벤트
            txt_email.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_email.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));
            txt_name.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_name.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));
            txt_ID.AddHandler(LostFocusEvent, new RoutedEventHandler(SetDefaultText));
            txt_ID.AddHandler(GotFocusEvent, new RoutedEventHandler(SetBlank));

            // 비밀번호 텍스트박스 힌트 이벤트 
            txt_PW.AddHandler(LostFocusEvent, new RoutedEventHandler(PasswordHintEnable));
            label_PW.AddHandler(MouseDownEvent, new RoutedEventHandler(PasswordHintUnable));
            txt_PW_check.AddHandler(LostFocusEvent, new RoutedEventHandler(PasswordHintEnable));
            label_PW_check.AddHandler(MouseDownEvent, new RoutedEventHandler(PasswordHintUnable));

            // 성별 선택 이벤트
            man_choice.AddHandler(MouseDownEvent, new RoutedEventHandler(ChoiceSex));
            woman_choice.AddHandler(MouseDownEvent, new RoutedEventHandler(ChoiceSex));

            // 비밀번호 일치 확인 이벤트
            txt_PW_check.AddHandler(KeyUpEvent, new RoutedEventHandler(IsEqualPassword));

            // 알림 이벤트
            txt_ID.AddHandler(GotFocusEvent, new RoutedEventHandler(IsThereAlarm));
            txt_name.AddHandler(GotFocusEvent, new RoutedEventHandler(IsThereAlarm));
            txt_email.AddHandler(GotFocusEvent, new RoutedEventHandler(IsThereAlarm));
            txt_PW.AddHandler(GotFocusEvent, new RoutedEventHandler(IsEmptyID));

            // 가입하기 버튼 이벤트
            Btn_register.Click += Btn_register_Click;

            // 생년월일 예외 이벤트
            txt_year.AddHandler(KeyUpEvent, new RoutedEventHandler(ShowBirthAlarm));
            txt_day.AddHandler(KeyUpEvent, new RoutedEventHandler(ShowBirthAlarm));

            // 인증메일 보내기 이벤트
            label_SendMail.AddHandler(MouseDownEvent, new RoutedEventHandler(SendCheckMail));
            Btn_certificate.Click += Btn_Certificate_Click;
            txt_certificate.AddHandler(KeyUpEvent, new RoutedEventHandler(UnlockButton));
        }

        public void IsThereAlarm(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Foreground.Equals(Brushes.Red))
            {
                textBox.Foreground = Brushes.Black;
                textBox.Text = "";
            }
        }

        public void UnlockButton(object sender, RoutedEventArgs e)
        {
            if (txt_certificate.Text.Length == 8)
                Btn_certificate.IsEnabled = true;
            else
                Btn_certificate.IsEnabled = false;
        }

        public void IsEmptyID(object sender, RoutedEventArgs e)
        {
            if (txt_ID.Text.Length == 0 || txt_ID.Text.Equals("아이디"))
            {
                txt_ID.Foreground = Brushes.Red;
                txt_ID.Text = "아이디가 입력되지 않았습니다.";
            }
        }

        public void ShowBirthAlarm(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(txt_year.Text, "^[0-9]+$") || !Regex.IsMatch(txt_day.Text, "^[0-9]+$"))
            {
                birth_alarm.Content = "제대로 입력해주세요.";
                return;
            }

            if (int.Parse(txt_year.Text) < 1920)
                birth_alarm.Content = "정말이세요?";
            else if (int.Parse(txt_day.Text) >= 32 || int.Parse(txt_year.Text) >= 2019 || int.Parse(txt_day.Text) == 0)
                birth_alarm.Content = "아닐텐데요";
            else
                birth_alarm.Content = "";
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
                if (DB.FindIDByEmail(txt_email.Text) != null)
                    MessageBox.Show("중복된 이메일입니다.");
            }
            
            mail.SetToAddress(txt_email.Text + '@' + Cbx_email.SelectedItem.ToString());
            certificationNo = ReturnRandomString();
            mail.SendEmail("En# 스터디 본인확인 이메일입니다.", "인증번호는 " + certificationNo + "입니다.");

            label_SendMail.Content = "발송됨";
            Btn_certificate.IsEnabled = true;

        }

        public void Btn_Certificate_Click(object sender, RoutedEventArgs e)
        {
            if(txt_certificate.Text.Equals(certificationNo))
            {
                MessageBox.Show("인증되었습니다!");
                certification = true;
                label_SendMail.Content = "보내기";
                label_SendMail.Foreground = Brushes.Black;
                Btn_certificate.IsEnabled = false;
                mail.Close();
            }
            else
                MessageBox.Show("인증 번호를 확인해주세요!");
        }

        public string ReturnRandomString()
        {
            Random rand = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 8).Select(x => input[rand.Next(0, input.Length)]);
            return new string(chars.ToArray());
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

        public void ChoiceSex(object sender, RoutedEventArgs e)
        {
            Label label = (Label)sender;
            if (label.Equals(man_choice))
            {
                man_choice.Foreground = Brushes.Green;
                man_choice_border.BorderBrush = Brushes.Green;
                woman_choice.Foreground = Brushes.Black;
                woman_choice_border.BorderBrush = Brushes.LightGray;
                sex = 1;
            }
            else
            {
                woman_choice.Foreground = Brushes.Green;
                woman_choice_border.BorderBrush = Brushes.Green;
                man_choice.Foreground = Brushes.Black;
                man_choice_border.BorderBrush = Brushes.LightGray;
                sex = 2;
            }
        }

        public void IsEqualPassword(object sender, RoutedEventArgs e)
        {
            if (txt_PW_check.Password.Equals(txt_PW.Password))
                txt_PW_check.Background = Brushes.GreenYellow;
            else
                txt_PW_check.Background = Brushes.OrangeRed;
        }

        public bool IsCorrectBirth()
        {
            // 텍스트 박스가 비어있을 경우
            if (txt_day.Text.Length == 0 || txt_year.Text.Length == 0)
                return false;
            // 숫자만 입력되지 않은 경우
            if (!Regex.IsMatch(txt_year.Text, "^[0-9]+$") || !Regex.IsMatch(txt_day.Text, "^[0-9]+$"))
                return false;
            // 일자가 잘못된 경우
            if (int.Parse(txt_day.Text) > 31 || int.Parse(txt_year.Text) > 2018 || int.Parse(txt_day.Text) == 0)
                return false;

            return true;
        }

        public void Btn_register_Click(object sender, RoutedEventArgs e)
        {
            // 아이디가 제대로 입력되었는지 확인
            if (!Regex.IsMatch(txt_ID.Text, "^[a-zA-Z0-9]+$") || txt_ID.Text.Length < 5)
            {
                MessageBox.Show("아이디를 다시 확인해주세요.(영문자+숫자 조합, 5글자 이상)");
                txt_ID.Text = "";
                txt_ID.Focus();
                return;
            }
            else if (DB.IsOverID(txt_ID.Text))
            {
                MessageBox.Show("중복된 아이디입니다!");
                txt_ID.Text = "";
                txt_ID.Focus();
                return;
            }

            // 비밀번호가 제대로 입력되었는지 확인
            if (!txt_PW_check.Background.Equals(Brushes.GreenYellow) || txt_PW.Password.Length == 0)
            {
                MessageBox.Show("비밀번호를 다시 확인해주세요.");
                return;
            }

            // 이름이 제대로 입력되었는지 확인
            if(!Regex.IsMatch(txt_name.Text, "^[가-힣]+$") || txt_name.Text.Length == 0 || txt_name.Text.Equals("이름"))
            {
                MessageBox.Show("이름을 다시 확인해주세요.");
                txt_name.Text = "";
                txt_name.Focus();
                return;
            }
            
            // 성별이 제대로 선택되었는지 확인
            if(sex == 0)
            {
                MessageBox.Show("성별을 선택해주세요.");
                return;
            }
            // 생년월일이 제대로 입력되었는지 확인
            if(!IsCorrectBirth())
            {
                MessageBox.Show("생년월일 입력을 다시 확인해주세요.");
                return;
            }
            // 인증되었는지 확인
            if(!certification)
            {
                MessageBox.Show("본인 확인을 해주세요.");
                return;
            }

            string birthDate = txt_year.Text + Cbx_month.SelectedItem.ToString() + txt_day.Text;
            string email = txt_email.Text + "@" + Cbx_email.SelectedItem.ToString();
            if (DB.InsertMember(txt_ID.Text, txt_PW.Password, txt_name.Text, sex, birthDate, email))
            {
                MessageBox.Show("회원가입 되었습니다.");
                Close();
            }
        }
    }
}
