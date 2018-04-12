using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LibraryManagement.Member
{
    class Register
    {
        UI.ScreenUI drawer;

        public Register(Data.MemberManagement memberList) {
            drawer = new UI.ScreenUI();
            Registration(memberList);
        }

        // 입력된 정보를 가지고 회원을 등록, 학번이 같은 회원정보가 있으면 등록하지 않음
        public void Registration(Data.MemberManagement memberList) {
            Data.Member newMember = drawer.RegistrationScreen();
            if (memberList.IsThereMember(newMember.StudentNo)) {
                Console.WriteLine("이미 존재하는 회원입니다.");
                Console.ReadKey();
                Console.Clear();
            }
            else
                memberList.Insert(newMember);
        }
    }
}
