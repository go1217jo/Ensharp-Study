using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// InformationModification.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InformationModification : UserControl
    {
        Data.DAO DB;
        string id;

        public InformationModification(string id, Data.DAO DB)
        {
            InitializeComponent();
            this.DB = DB;
            this.id = id;
            Btn_modification.Click += Btn_modification_Click;
            Btn_delete.Click += Btn_delete_Click;
        }

        private void Btn_modification_Click(object sender, RoutedEventArgs e)
        {
            if (txt_name.Text.Length == 0 || !Regex.IsMatch(txt_name.Text, "^[가-힣]+$"))
            {
                MessageBox.Show("이름의 형식에 맞지 않습니다.");
                return;
            }
            if(txt_password.Password.Length == 0)
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                return;
            }
            DB.ModifyName(id, txt_name.Text);
            DB.ModifyPassword(id, txt_password.Password);
            MessageBox.Show("수정 되었습니다!");
        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            DB.DeleteMember(id);
            MessageBox.Show("회원 탈퇴되었습니다.\n로그인 화면으로 돌아가면 탈퇴가 완료됩니다.");
        }
    }
}
