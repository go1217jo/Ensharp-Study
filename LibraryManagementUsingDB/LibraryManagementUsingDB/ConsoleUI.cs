using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB
{
   class ConsoleUI
   {
      static IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();

      // 메뉴 문자열 집합
      public static string[] RENTAL_MENU = { "책 검색", "전체 보기", "대출 목록", "돌아 가기" };
      public static string[] ADMIN_MENU = { "회원 관리", "도서 관리", "로그 관리", "돌아 가기" };
      public static string[] BOOK_MENU = { "서적 추가", "서적 삭제", "수량 수정", "전체 보기", "돌아 가기" };
      public static string[] MEMBER_MENU = { "회원 추가", "회원 수정", "회원 삭제", "회원 목록", "돌아 가기" };
      public static string[] BOOK_SEARCH = { "도서명 검색", "출판사 검색", "저자 검색", "돌아 가기"};
      public static string[] BOOK_MODIFY = { "도서명 수정", "출판사 수정", "저자 수정", "돌아 가기" };
      public static string[] MEMBER_MODIFY = { "회원명 수정", "주소 수정", "전화번호 수정", "돌아 가기" };
      public static string[] LOG_MENU = { "로그 보기", "로그 초기화", "돌아 가기" };
      public static string[] Tray_Menu = { " 도서 선택 ", "책 상세보기" };

      // 로그인 화면
      public static int PrintLoginPage()
      {
         const int screenWidth = 50, screenHeight = 16;
         Console.Clear();
         Console.SetWindowSize(screenWidth, screenHeight);
         Console.WriteLine("    _       ______   _____    _____   ___    _ ");
         Console.WriteLine("   | |     |  __  | |  ___|  |_   _| |   ＼ | |");
         Console.WriteLine("   | |     | |  | | | |   _    | |   | |＼＼| |");
         Console.WriteLine("   | |___  | |__| l | |__| |  _| |_  | | ＼ | |");
         Console.WriteLine("   |_____| |______| |______| |_____| |_|  ＼__|");
         Console.WriteLine("\n\n      도서관리시스템에 오신 것을 환영합니다\n");
         Console.WriteLine("                                     _______");
         Console.WriteLine("      학번/아이디 >                 | login |");
         Console.WriteLine("                                    |[ENTER]|");
         Console.WriteLine("      비밀번호    >                 |_______|");

         return screenHeight;
      }

      // 엔샵 로고
      public static void PrintEnsharpLogo()
      {
         Console.Clear();
         Console.WriteLine(" ========================================");
         Console.WriteLine(" || ■■■■  ■■     ■     ■  ■   ||");
         Console.WriteLine(" || ■        ■  ■   ■   ■■■■■ ||");
         Console.WriteLine(" || ■■■■  ■   ■  ■    ■   ■   ||");
         Console.WriteLine(" || ■        ■    ■ ■  ■■■■■  ||");
         Console.WriteLine(" || ■■■■  ■     ■■   ■   ■    ||");
         Console.WriteLine(" ========================================");
      }

      // 책 로고
      public static void PrintLogo()
      {
         Console.Clear();
         Console.WriteLine(" ========================================");
         Console.Write(" ||               ");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write(" ▲▲");
         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine("     Book       ||");
         Console.Write(" ||            ");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("■■ ■■");
         Console.ForegroundColor = ConsoleColor.White;
         Console.Write(" Management ");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("■");
         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine(" ||");
         Console.Write(" ||     EN#    ");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("■■■■■         ■■ ");
         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine("||");
         Console.Write(" ||   ");
         Console.ForegroundColor = ConsoleColor.DarkYellow;
         Console.Write("■■■");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("       ■■■■■■■■■  ");
         Console.ForegroundColor = ConsoleColor.White;
         Console.Write("||\n || ");
         Console.ForegroundColor = ConsoleColor.Red;
         Console.Write("■■■■■");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("     ■■■■■■■■    ");
         Console.ForegroundColor = ConsoleColor.White;
         Console.Write("||\n || ");
         Console.ForegroundColor = ConsoleColor.Red;
         Console.Write("■■■■■");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("     ■ ■      ■ ■    ");
         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine("||");
         Console.WriteLine(" ========================================");
      }

      // 메뉴 화면 출력
      public static int PrintMenu(string[] text, int choice)
      {
         const int screenWidth = 42, screenHeight = 18;

         Console.SetWindowSize(screenWidth, screenHeight);

         Console.WriteLine("\n\t      ===============");

         // 메뉴 문자열 출력
         for (int menu = 0; menu < text.Length; menu++)
         {
            if (choice == menu)
               Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(outputProcessor.PrintFixString(text[menu], screenWidth + 1));
            Console.ForegroundColor = ConsoleColor.White;
         }

         Console.WriteLine("\t      ===============");

         return screenHeight;
      }

      public static void PrintRegistration()
      {
         Console.SetWindowSize(42, 30);
         Console.Clear();
         PrintLogo();
         Console.WriteLine("\n\t\t<회원 가입>");
         Console.WriteLine("\n       이름 > ");
         Console.WriteLine("\n       학번 > ");
         Console.WriteLine("\n       주소 > ");
         Console.WriteLine("\n   전화번호 > ");
         Console.WriteLine("\n   비밀번호 > ");

         Console.SetCursorPosition(3, 28);
         Console.Write("ESC : 돌아가기");
      }

      // API를 통해 책을 검색하는 화면
      public static void PrintAPISearchBook()
      {
         Console.SetWindowSize(42, 25);
         Console.Clear();
         PrintLogo();
         Console.WriteLine("\n\t\t<도서 검색>");
         Console.WriteLine("\n    검색어 > ");
         Console.WriteLine("\n 검색 개수 > ");
         
         Console.SetCursorPosition(3, 23);
         Console.Write("ESC : 돌아가기");
      }

      public static void PrintBookListHeader()
      {
         Console.WriteLine("\n ====================================================================================================================================================================");
         Console.WriteLine("           ISBN                                  도서명                                  출판사                         저자                가격  수량   출판일");
         Console.WriteLine(" ====================================================================================================================================================================");
      }
   }
}
