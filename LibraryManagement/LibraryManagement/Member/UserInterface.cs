using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Member
{
    class UserInterface
    {
        Data.Member member;
        UI.ScreenUI drawer;
        Library.LibrarySystem system;

        public UserInterface(Library.LibrarySystem system, Data.Member member)
        {
            this.member = member;
            drawer = new UI.ScreenUI();
            // 사용자 권한을 가진 도서관 시스템을 생성
            this.system = system;
            // 시스템 권한을 사용자권한으로 변경
            this.system.SetAuthority(1);
            // 유저 메뉴 메인 flow
            UserMain();
        }

        public void UserMain()
        {
            // 메뉴 선택에 따라 함수를 실행한다.
            while (true)
            {
                switch (drawer.UserMenuScreen())
                {
                    case 1:
                        system.SearchBook();
                        break;
                    case 2:
                        system.PrintAllBookList();
                        break;
                    case 3:
                        system.UserMyPage();
                        break;
                    case 4:
                        return;
                }
                Console.SetWindowSize(42, 16);
            }
        }        
    }
}
