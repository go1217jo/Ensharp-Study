using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LibraryManagement.UI
{
    class ScreenUI
    {
        KeyInput inputProcessor;

        public ScreenUI() {
            inputProcessor = new KeyInput();
        }

        // 책장 모양의 배너를 출력한다
        public static void LibraryBanner()
        {
            Console.WriteLine(" ========================================");
            for (int iter = 0; iter < 3; iter++) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ||");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(" ▥▥▥▥▥▥▥▥▥▥▥▥▥▥▥▥▥ ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("||");
                if(iter != 2)
                    Console.WriteLine(" ||------------------------------------||");
            }
            Console.WriteLine(" ========================================\n");

            // 책장 배너 가운데로 커서를 위치시킨 뒤 다음 출력 위치로 커서를 이동시킨다.
            Console.SetCursorPosition(15, 3);
            Console.Write(" 도 서 관 리 ");
            Console.SetCursorPosition(0, 8);
        }

        // 입력된 line만큼 엔터를 친다.
        public void GotoLine(int line) {
            for (int i = 0; i < line; i++)
                Console.WriteLine();
        }

        // 첫 메뉴 화면을 출력한다.
        public int FirstMenuScreen() {
            int choice = 1;
            Console.SetWindowSize(42, 16);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\t        <모드 설정>");
                Console.WriteLine("\t ========================");
                if(choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t        관리자 화면");  // CursorTop : 10
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t        사용자 화면");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t        시스템 종료");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행에 커서를 위치시킨다.
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 행의 인덱스를 반환한다
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 위치를 제한한다.
                if (Console.CursorTop < 10)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 10);
                if (Console.CursorTop > 12)
                    Console.SetCursorPosition(Console.CursorLeft, 12);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;
                
                Console.Clear();
            }
        }

        // 유저 로그인 회원가입 선택
        public int UserLoginMenuScreen()
        {
            int choice = 1;
            Console.SetWindowSize(42, 16);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\t          <사용자>");
                Console.WriteLine("\t ========================");
                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t           로그인");  // CursorTop : 10
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t          회원가입");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t          돌아가기");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행의 위치로 이동
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 행의 인덱스가 반환된다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 위치를 제한한다
                if (Console.CursorTop < 10)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 10);
                if (Console.CursorTop > 12)
                    Console.SetCursorPosition(Console.CursorLeft, 12);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;
                
                Console.Clear();
            }
        }

        // 로그인 화면 출력
        public Member.LoginInfo LoginScreen() {
            string id, password;
            Console.SetWindowSize(42, 16);
            LibraryBanner();
            Console.WriteLine("\n\tID(학번) > ");
            Console.WriteLine("\tPassword > ");

            // 각각 항목들에 대해 문자열을 입력받는다.
            Console.SetCursorPosition(19, 9);
            id = inputProcessor.ReadAndCheckString(8, 16, 19, 9, true);
            Console.SetCursorPosition(19, 10);
            password = inputProcessor.ReadAndCheckString(18, 16, 19, 10);
            
            return new Member.LoginInfo(id, password);
        }

        public Data.Member RegistrationScreen() {
            Data.Member newMember = new Data.Member();
            Console.SetWindowSize(42, 30);
            Console.Clear();
            LibraryBanner();
            Console.WriteLine("\n\t\t<회원 가입>");
            Console.WriteLine("\n       이름 > ");
            Console.WriteLine("\n       학번 > ");
            Console.WriteLine("\n       주소 > ");
            Console.WriteLine("\n   전화번호 > ");
            Console.WriteLine("\n   비밀번호 > ");

            // 각각 항목들에 대해 문자열을 입력받는다.
            Console.SetCursorPosition(14, 11);
            newMember.Name = inputProcessor.ReadAndCheckString(10, 30, 14, 11, true);
            Console.SetCursorPosition(14, 13);
            newMember.StudentNo = inputProcessor.ReadAndCheckString(8, 30, 14, 13, true);
            Console.SetCursorPosition(14, 15);
            newMember.Address = inputProcessor.ReadAndCheckString(15, 30, 14, 15, false);
            Console.SetCursorPosition(14, 17);
            newMember.PhoneNumber = inputProcessor.ReadAndCheckString(18, 30, 14, 17, true);
            Console.SetCursorPosition(14, 19);
            newMember.Password = inputProcessor.ReadAndCheckString(10, 30, 14, 19, true);
            Console.Clear();
            return newMember;
        }

        // 유저로 로그인했을 때 보이는 메뉴화면
        public int UserMenuScreen()
        {
            int choice = 1;
            Console.SetWindowSize(42, 16);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\t        <대출/반납>");
                Console.WriteLine("\t ========================");
                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         검     색");  // CursorTop : 10
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         전체 보기");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         대출 목록");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 4)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         돌아 가기");  // CursorTop : 13
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행으로 커서를 옮긴다
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 인덱스를 반환한다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 이동 구간을 제한한다.
                if (Console.CursorTop < 10)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 10);
                if (Console.CursorTop > 13)
                    Console.SetCursorPosition(Console.CursorLeft, 13);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;
                
                Console.Clear();
            }
        }

        // 관리자로 로그인했을 때 보이는 메뉴화면
        public int AdminMenuScreen()
        {
            int choice = 1;
            Console.SetWindowSize(42, 16);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\t           <메뉴>");
                Console.WriteLine("\t ========================");
                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         회원 관리");  // CursorTop : 10
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         도서 관리");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         돌아 가기");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행으로 커서를 옮긴다
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 인덱스를 반환한다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 이동 구간을 제한한다.
                if (Console.CursorTop < 10)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 10);
                if (Console.CursorTop > 12)
                    Console.SetCursorPosition(Console.CursorLeft, 12);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;

                Console.Clear();
            }
        }

        // 고정된 자리수의 문자열 출력
        public string PrintFixString(string toPrint, int charNumber)
        {
            // 바이트 수를 받아옴
            int presentLength = Encoding.Default.GetBytes(toPrint).Length;

            // 뒤에 공백 붙이기
            for (int i = 0; i < charNumber - presentLength; i++)
                toPrint = toPrint + " ";
            return toPrint;
        }

        // 관리자 서적 관리 메뉴 화면
        public int BookManagementScreen()
        {
            int choice = 1;
            Console.SetWindowSize(42, 18);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\t           <메뉴>");
                Console.WriteLine("\t ========================");
                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         서적 추가");  // CursorTop : 10
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         서적 삭제");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         서적 수정");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 4)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         전체 보기");  // CursorTop : 13
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 5)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         돌아 가기");  // CursorTop : 14
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행으로 커서를 옮긴다
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 인덱스를 반환한다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 이동 구간을 제한한다.
                if (Console.CursorTop < 10)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 10);
                if (Console.CursorTop > 14)
                    Console.SetCursorPosition(Console.CursorLeft, 14);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;
                
                Console.Clear();
            }
        }

        // 관리자 회원 관리 메뉴 화면
        public int MemberManagementScreen()
        {
            int choice = 1;
            Console.SetWindowSize(42, 18);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\t           <메뉴>");
                Console.WriteLine("\t ========================");
                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         회원 추가");  // CursorTop : 10
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         회원 수정");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         회원 삭제");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 4)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         회원 목록");  // CursorTop : 13
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 5)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t         돌아 가기");  // CursorTop : 14
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행으로 커서를 옮긴다
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 인덱스를 반환한다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 이동 구간을 제한한다.
                if (Console.CursorTop < 10)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 10);
                if (Console.CursorTop > 14)
                    Console.SetCursorPosition(Console.CursorLeft, 14);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;
                
                Console.Clear();
            }
        }

        // 도서 추가화면 출력
        public Data.Book AddBookScreen()
        {
            Data.Book newBook = new Data.Book();
            Console.SetWindowSize(42, 25);
            Console.Clear();
            LibraryBanner();
            Console.WriteLine("\n\t\t<도서 추가>");
            Console.WriteLine("\n     도서명 > ");
            Console.WriteLine("\n     출판사 > ");
            Console.WriteLine("\n       저자 > ");

            // 각각 항목들에 대해 문자열을 입력받는다.
            Console.SetCursorPosition(14, 11);
            newBook.Name = inputProcessor.ReadAndCheckString(25, 25, 14, 11, false);
            Console.SetCursorPosition(14, 13);
            newBook.Company = inputProcessor.ReadAndCheckString(15, 25, 14, 13, true);
            Console.SetCursorPosition(14, 15);
            newBook.Writer = inputProcessor.ReadAndCheckString(20, 25, 14, 15, false);

            Console.Clear();
            return newBook;
        }

        public int BookSearchScreen()
        {
            int choice = 1;
            Console.SetWindowSize(42, 18);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\t        <도서 검색>");
                Console.WriteLine("\t    아래 항목으로 검색");
                Console.WriteLine("\t ========================");

                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t          도서명");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)    
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t           저자");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t          출판사");  // CursorTop : 13
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행으로 커서를 옮긴다
                Console.SetCursorPosition(0, 10 + choice);

                // 엔터를 치면 선택된 인덱스를 반환한다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 이동 구간을 제한한다.
                if (Console.CursorTop < 11)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 11);
                if (Console.CursorTop > 13)
                    Console.SetCursorPosition(Console.CursorLeft, 13);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 10;
                
                Console.Clear();
            }
        }

        public int MemberModifyScreen()
        {
            int choice = 1;
            Console.SetWindowSize(42, 18);
            while (true)
            {
                LibraryBanner();
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("\n\t    수정할 항목 선택");
                Console.WriteLine("\t ========================");

                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t           이름");  // CursorTop : 11
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t           주소");  // CursorTop : 12
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t       핸드폰 번호");  // CursorTop : 13
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 4)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t        비밀번호");  // CursorTop : 14
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t ========================");

                // 첫 행으로 커서를 옮긴다
                Console.SetCursorPosition(0, 10 + choice);

                // 엔터를 치면 선택된 인덱스를 반환한다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 이동 구간을 제한한다.
                if (Console.CursorTop < 11)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 11);
                if (Console.CursorTop > 14)
                    Console.SetCursorPosition(Console.CursorLeft, 14);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 10;

                Console.Clear();
            }
        }

        public int PrintBookList(ArrayList bookList) {
            int choice = 1;
            Console.SetWindowSize(103, 39);
            while (true)
            {
                Console.WriteLine("\n ====================================================================================================");
                Console.WriteLine("      도서번호                도서명                출판사             저자             대출 여부     ");
                Console.WriteLine(" ====================================================================================================");
                for (int i = 0; i < bookList.Count; i++)
                {
                    Data.Book book = (Data.Book)bookList[i];
                    // 선택된 행이면 빨간색으로 표시
                    if (choice - 1 == i)
                        Console.ForegroundColor = ConsoleColor.Red;
                    // 행 내용을 글자 간격에 맞춰 출력
                    Console.Write("       {0}", PrintFixString(book.BookNo, 14));
                    Console.Write(" {0}", PrintFixString(book.Name, 25));
                    Console.Write(" {0}", PrintFixString(book.Company, 15));
                    Console.Write(" {0}", PrintFixString(book.Writer, 20));
                    if (book.Rental)
                        Console.WriteLine("     대출 중    ");
                    else
                        Console.WriteLine("     보유 중    ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                // 첫 행으로 커서를 옮긴다
                Console.SetCursorPosition(0, 3 + choice);

                // 엔터를 치면 선택된 인덱스를 반환한다.
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 이동 구간을 제한한다.
                if (Console.CursorTop < 4)
                    Console.SetCursorPosition(Console.CursorLeft - 1, 4);
                if (Console.CursorTop > 3 + ((bookList.Count == 0) ? 1 : bookList.Count))
                    Console.SetCursorPosition(Console.CursorLeft, 3 + ((bookList.Count==0) ? 1:bookList.Count));

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 3;

                Console.Clear();
            }
        }

        // 예 아니오를 선택하게 하는 화면 출력, 두 번째 줄 알림 출력 뒤 호출
        public int YesOrNo(string alert)
        {
            int choice = 1;
            Console.SetWindowSize(42, 16);
            while (true)
            {
                Console.WriteLine("\n   " + alert + "\n");
                if (choice == 1)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t   예");  // CursorTop : 4
                Console.ForegroundColor = ConsoleColor.White;
                if (choice == 2)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t 아니오");  // CursorTop : 5
                Console.ForegroundColor = ConsoleColor.White;
                
                Console.SetCursorPosition(0, 3 + choice);

                if (inputProcessor.ChoiceByKey())
                    return choice;

                if (Console.CursorTop < 4)
                    Console.SetCursorPosition(Console.CursorLeft, 4);
                if (Console.CursorTop > 5)
                    Console.SetCursorPosition(Console.CursorLeft, 5);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 3;

                Console.Clear();
            }
        }
    }
}
