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
      public static string[] RENTAL_MENU = { "책 검색", "전체 보기", "대출 목록", "돌아 가기" };
      public static string[] ADMIN_MENU = { "회원 관리", "도서 관리", "돌아 가기" };
      public static string[] BOOK_MENU = { "서적 추가", "서적 삭제", "서적 수정", "전체 보기", "돌아 가기" };
      public static string[] MEMBER_MENU = { "회원 추가", "회원 수정", "회원 삭제", "회원 목록", "돌아 가기" };
      public static string[] BOOK_SEARCH = { "도서명 검색", "출판사 검색", "저자 검색"};

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

      // 책 로고
      public static void PrintLogo()
      {
         Console.Clear();
         Console.WriteLine(" ========================================");
         Console.Write(" ||               ");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write(" ▲▲");
         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine("                 ||");
         Console.Write(" ||    Book    ");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("■■ ■■");
         Console.ForegroundColor = ConsoleColor.White;
         Console.Write("    EN#     ");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("■");
         Console.ForegroundColor = ConsoleColor.White;
         Console.WriteLine(" ||");
         Console.Write(" || Management ");
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
         Console.ForegroundColor = ConsoleColor.Cyan;
         Console.Write("■■■■■");
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write("     ■■■■■■■■    ");
         Console.ForegroundColor = ConsoleColor.White;
         Console.Write("||\n || ");
         Console.ForegroundColor = ConsoleColor.Cyan;
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
         Console.WriteLine();
         Console.WriteLine("\t      ===============");

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

      public static void RegistrationScreen()
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
   }
}
