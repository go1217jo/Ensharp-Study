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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemberManager
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginScreen login;
        FindID findID;
        FindPW findPW;
        FindPWbyEmail findPwByEmail;
        Registration register;
        Data.DAO DB;
        string IDfindingPassword = null;
        
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;

            DB = new Data.DAO();
            Init();
            MainGrid.Children.Add(login);
        }

        public void Init()
        {
            login = new LoginScreen();
            findID = new FindID(DB);
            findPW = new FindPW();

            login.findID.AddHandler(MouseDownEvent, new RoutedEventHandler(Label_findID_Click));
            login.findPW.AddHandler(MouseDownEvent, new RoutedEventHandler(Label_findPW_Click));
            login.register.AddHandler(MouseDownEvent, new RoutedEventHandler(Label_register_Click));

            findID.Btn_Back_FindID.Click += Btn_Back_Click;
            findPW.Btn_Back_FindPW.Click += Btn_Back_Click;
            
            findPW.findID_Click.AddHandler(MouseDownEvent, new RoutedEventHandler(MoveToFindID));
            findPW.Btn_next.Click += Btn_next_Click;

            login.Btn_Login.Click += Btn_Login_Click;
            

        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (login.txtID.Text.Length == 0 || login.txtID.Text.Equals("아이디"))
            {
                MessageBox.Show("아이디를 입력해주세요.");
                return;
            }
            if (login.txtPW.Password.Length == 0)
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                return;
            }

            Data.MemberVO member = DB.Login(login.txtID.Text, login.txtPW.Password);

            if (member == null)
                MessageBox.Show("아이디 또는 비밀번호를 확인해주세요.");
            else
            {
                MainScreen main = new MainScreen(member);
                main.Btn_logout.Click += Btn_logout_Click;
                main.Btn_modify.Click += Btn_modify_Click;
                MainGrid.Children.Clear();
                MainGrid.Children.Add(main);
            }
        }

        private void Btn_next_Click(object sender, RoutedEventArgs e)
        {
            if (findPW.txt_ID.Text.Length == 0 || findPW.txt_ID.Text.Equals("찾고자 하는 아이디"))
            {
                MessageBox.Show("아이디를 입력해주세요!");
                return;
            }
            else {
                if (!DB.IsOverID(findPW.txt_ID.Text))
                {
                    MessageBox.Show("등록되지 않은 아이디입니다.");
                    return;
                }
            }

            MainGrid.Children.Clear();
            IDfindingPassword = findPW.txt_ID.Text;
            findPwByEmail = new FindPWbyEmail(DB);
            findPwByEmail.Btn_Back.Click += Btn_Back_Click;
            MainGrid.Children.Add(findPwByEmail);
        }

        private void Btn_logout_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(login);
        }

        private void Btn_modify_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new InformationModification());
        }

        private void Label_findID_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(findID);
        }

        private void Label_findPW_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(findPW);
        }

        private void Label_register_Click(object sender, RoutedEventArgs e)
        {
            register = new Registration(DB);
            register.Show();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            Button button = (Button)sender;

            if (button.Equals(findID.Btn_Back_FindID))
            {
                findID.txt_email.Text = "이메일";
                findID.label_id_by_email.Content = "";
            }
            else if (button.Equals(findPW.Btn_Back_FindPW))
                findPW.txt_ID.Text = "찾고자 하는 아이디";

            MainGrid.Children.Add(login);
        }

        private void MoveToFindID(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(findID);
        }
    }
}
