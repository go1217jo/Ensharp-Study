using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace LibraryManagement.Admin
{
    class AdminInterface
    {
        UI.ScreenUI drawer;
        Library.LibrarySystem system;
        Data.MemberManagement membermanager;
        UI.KeyInput inputProcessor;

        // 생성자, 매개변수로 도서관 시스템과 회원 관리자 클래스를 받음
        public AdminInterface(Library.LibrarySystem system, Data.MemberManagement membermanager)
        {
            
            drawer = new UI.ScreenUI();
            this.system = system;        
            this.membermanager = membermanager;
            inputProcessor = new UI.KeyInput();

            // 관리자 메뉴 메인 flow
            AdminMain();
        }

        public void AdminMain()
        {
            // 메뉴 선택에 따라 함수를 실행한다.
            // 1 : 회원 관리, 2 : 도서 관리, 3 : 되돌아가기
            while (true)
            {
                // 관리자 메뉴를 출력하고 사용자의 선택을 반환받는다.
                switch (drawer.AdminMenuScreen())
                {
                    case 1:
                        // 회원 관리
                        MemberManagement();
                        break;
                    case 2:
                        // 도서 관리
                        BookManagement();
                        break;
                    case 3:
                        return;
                }
            }
        }

        // 회원들을 관리한다.
        public void MemberManagement()
        {
            ArrayList searchResult;
            while (true)
            {
                // 회원 관리 화면을 출력하여 사용자의 선택을 받는다.
                // 1 : 회원 등록, 2 : 회원 수정, 3 : 회원 삭제, 4 : 회원 목록 5 : 되돌아가기
                switch (drawer.MemberManagementScreen())
                {
                    case 1:
                        // 회원 가입 화면을 출력하고 회원을 등록시킨다.
                        new Member.Register(membermanager);
                        break;
                    case 2:
                        // 학번으로 회원을 먼저 검색한다.
                        Console.Write("\n   검색할 학번 > ");
                        searchResult = membermanager.SearchBy((int)Data.MemberManagement.Format.StudentNoFormat, inputProcessor.ReadAndCheckString(8, 18, 17, 2, true));
                        Console.Clear();
                        // 검색된 회원의 정보를 수정한다.
                        membermanager.ModifyMember((Data.Member)searchResult[0]);
                        break;
                    case 3:
                        // 학번으로 회원을 먼저 검색한다.
                        Console.Write("\n   검색할 학번 > ");
                        searchResult = membermanager.SearchBy((int)Data.MemberManagement.Format.StudentNoFormat, inputProcessor.ReadAndCheckString(8, 18, 17, 2, true));
                        Console.Clear();
                        // 검색된 회원이 있다면
                        if (searchResult.Count != 0)
                        {
                            // 검색된 회원 삭제
                            membermanager.Delete(searchResult[0]);
                            Console.WriteLine("\n   삭제되었습니다.");
                            inputProcessor.PressAnyKey();
                            Console.Clear();
                        }
                        // 검색된 회원이 없다면 삭제 실패
                        else {
                            Console.WriteLine("\n   삭제 실패하였습니다.");
                            inputProcessor.PressAnyKey();
                            Console.Clear();
                        }
                        Console.Clear();
                        break;
                    case 4:
                        // 현재 회원들의 전체 목록을 출력한다.
                        membermanager.PrintMemberList();
                        break;
                    case 5:
                        // 되돌아가기
                        return;
                }
            }
        }

        // 서적을 관리한다.
        public void BookManagement()
        {
            ArrayList searchResult;
            int index;

            while (true)
            {
                // 도서 관리화면을 출력하고 사용자의 선택을 반환한다.
                // 1. 서적 추가, 2 : 서적 삭제, 3 : 서적 수정, 4 : 전체 보기, 5 : 되돌아가기
                switch (drawer.BookManagementScreen())
                {
                    case 1:
                        // 서적 추가화면을 출력하고 입력받은 도서를 현재 시스템
                        system.InsertBook(drawer.AddBookScreen());
                        break;
                    case 2:
                        // 서적을 검색하고 그 결과를 ArrayList형식으로 반환한다.
                        searchResult = system.SearchBook();
                        // 검색 결과가 없다면
                        if (searchResult.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\n검색 결과가 없습니다.");
                            inputProcessor.PressAnyKey();
                            Console.Clear();
                            break;
                        }
                        // 검색 결과를 출력하고 도서를 선택받는다.
                        index = drawer.PrintBookList(searchResult) - 1;
                        if(index != searchResult.Count)
                            // 선택된 도서를 삭제한다.
                            system.DeleteBook((Data.Book)searchResult[index]);
                        break;
                    case 3:
                        // 서적을 검색하고 수정한다.
                        if (system.SearchAndModifyBook())
                            Console.WriteLine("\n성공적으로 수정하였습니다.");
                        else
                            Console.WriteLine("\n수정 실패하였습니다.");
                        inputProcessor.PressAnyKey();
                        Console.Clear();
                        break;
                    case 4:
                        // 전체 서적 목록을 출력한다.
                        system.PrintAllBookList();
                        break;
                    case 5:
                        return;
                }
            }
        }
    }
}
