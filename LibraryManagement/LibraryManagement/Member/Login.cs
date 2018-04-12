using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LibraryManagement.Member
{
    class Login
    {
        Data.MemberManagement manager;
        public UI.ScreenUI drawer;
        public UI.KeyInput inputProcessor;

        public Login(Data.MemberManagement manager) {
            drawer = new UI.ScreenUI();
            this.manager = manager;
            this.inputProcessor = new UI.KeyInput();
        }

        // 로그인, 회원가입 중 선택한 메뉴를 실행
        public void UserMenuChoice(Library.LibrarySystem system) {
            switch(drawer.UserLoginMenuScreen())
            {
                case 1:
                    LoginUser(system);
                    break;
                case 2:
                    new Register(manager);
                    break;
                case 3:
                    return;
            }
        }

        // 관리자로 로그인, 로그인 성공이면 true 반환
        public bool LoginAdmin(Library.LibrarySystem system)
        {
            LoginInfo information = drawer.LoginScreen();

            // 관리자 아이디, 비밀번호 확인
            if(information.ID.Equals("Admin")) {
                if (information.Password.Equals("ensharp605!")) {
                    Console.Clear();
                    Console.WriteLine("\n\n\n\t  관리자님 환영합니다.");
                    Thread.Sleep(500);
                    Console.Clear();
                    // 관리자 권한 화면을 호출
                    new Admin.AdminInterface(system, manager);
                    return true;
                }
                Console.Clear();
                Console.WriteLine("\n\t비밀번호를 확인하세요.");
                inputProcessor.PressAnyKey();
                return false;
            }
            Console.Clear();
            Console.WriteLine("\n\t아이디 또는 비밀번호가 틀립니다.");
            inputProcessor.PressAnyKey();
            return false;
        }

        // 사용자로 로그인, 로그인 성공이면 true 반환
        public bool LoginUser(Library.LibrarySystem system) {
            LoginInfo information = drawer.LoginScreen();
            if(manager.IsThereMember(information.ID)) {
                // 학번으로 회원정보를 찾음
                /*
                Data.Member member = (Data.Member)manager.SearchBy((int)Data.MemberManagement.Format.StudentNoFormat, information.ID);
                if (member.Password.Equals(information.Password)) {
                    Console.WriteLine(member.Name + "님 환영합니다.");
                    Thread.Sleep(500);
                    Console.Clear();
                    // 로그인된 멤버 정보를 파라미터로 전달하고 유저 메뉴를 호출한다
                    new UserInterface(system, member);
                    return true;
                }
                */
                return false;
            }
            return true;
        }       
    }
}
