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
                    case 1:  // 도서 검색 후 대여
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
                        if (index != searchResult.Count)
                            system.Rental((Data.Book)searchResult[index], member);
                        break;
                    case 2:  // 전체 보기에서 선택하여 대여
                        system.Rental(system.ValueOf(system.PrintAllBookList()-1), member);
                        break;
                    case 3:  // 대출 목록에서 반납 및 연장
                        index = drawer.PrintBookList(member.rentalBookList) - 1;
                        if (member.rentalBookList.Count != 0)
                        {
                            if (drawer.YesOrNo("반납하시겠습니까?") == 1)
                                system.Return((Data.Book)member.rentalBookList[index], member);
                            else {
                                if (drawer.YesOrNo("연장하시겠습니까?") == 1)
                                    system.Extension((Data.Book)member.rentalBookList[index]);
                            }
                        }
                        else {
                            Console.WriteLine("\n   대출된 도서가 없습니다.");
                            inputProcessor.PressAnyKey();
                            Console.Clear();
                        }
                        break;
                    case 4:
                        return;
                }
                Console.SetWindowSize(42, 16);
            }
        }        
    }
}
