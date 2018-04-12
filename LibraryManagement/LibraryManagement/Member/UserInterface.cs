using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LibraryManagement.Member
{
    class UserInterface
    {
        Data.Member member;
        UI.ScreenUI drawer;
        Library.LibrarySystem system;
        UI.KeyInput inputProcessor;

        public UserInterface(Library.LibrarySystem system, Data.Member member)
        {
            this.member = member;
            drawer = new UI.ScreenUI();
            // 사용자 권한을 가진 도서관 시스템을 생성
            this.system = system;
            // 시스템 권한을 사용자권한으로 변경
            this.system.SetAuthority(1);
            // 입력에 대한 처리 객체
            inputProcessor = new UI.KeyInput();
            // 사용자 메인 플로우
            UserMain();
        }

        public void UserMain()
        {
            ArrayList searchResult;
            int index;
            // 메뉴 선택에 따라 함수를 실행한다.
            while (true)
            {
                switch (drawer.UserMenuScreen())
                {
                    case 1:
                        searchResult = system.SearchBook();
                        if (searchResult.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\n검색 결과가 없습니다.");
                            inputProcessor.PressAnyKey();
                            Console.Clear();
                            break;
                        }
                        index = drawer.PrintBookList(searchResult) - 1;
                        system.Rental((Data.Book)searchResult[index], member);
                        break;
                    case 2:
                        system.Rental(system.ValueOf(system.PrintAllBookList()-1), member);
                        break;
                    case 3:
                        drawer.PrintBookList(member.rentalBookList);
                        break;
                    case 4:
                        return;
                }
                Console.SetWindowSize(42, 16);
            }
        }        
    }
}
