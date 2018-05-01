using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable
{
   // 데이터 처리 없이 콘솔UI만 출력하는 클래스
   class ConsoleUI
   {
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
         Console.WriteLine("\n\n      학사정보시스템에 오신 것을 환영합니다\n");
         Console.WriteLine("                                     _______");
         Console.WriteLine("      학번/아이디 >                 | login |");
         Console.WriteLine("                                    |[ENTER]|");
         Console.WriteLine("      비밀번호    >                 |_______|");

         return screenHeight;
      }

      // 엔샵 로고
      public static void PrintLogo()
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

      // 메인메뉴 화면 출력
      public static int PrintMainMenu(int choice)
      {
         const int screenWidth = 42, screenHeight = 16;
            
         Console.SetWindowSize(screenWidth, screenHeight);
         Console.WriteLine();
         Console.WriteLine("\t      ===============");

         if (choice == ConstNumber.MENULIST_1)
               Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t       관심과목 담기");
         Console.ForegroundColor = ConsoleColor.White;

         if (choice == ConstNumber.MENULIST_2)
               Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t        수 강 신 청");
         Console.ForegroundColor = ConsoleColor.White;

         if (choice == ConstNumber.MENULIST_3)
               Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t        뒤 로 가 기");
         Console.ForegroundColor = ConsoleColor.White;

         Console.WriteLine("\t      ===============");

         return screenHeight;
      }

      // 수강신청-강의조회 화면
      public static int PrintSearchMenu(int choice)
      {
         const int screenWidth = 42, screenHeight = 17;

         Console.SetWindowSize(screenWidth, screenHeight);
         Console.WriteLine();
         Console.WriteLine("\t      ===============");

         if (choice == ConstNumber.MENULIST_1)
               Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t       관심과목 검색");
         Console.ForegroundColor = ConsoleColor.White;

         if (choice == ConstNumber.MENULIST_2)
               Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t         강의 조회");
         Console.ForegroundColor = ConsoleColor.White;

         if (choice == ConstNumber.MENULIST_3)
               Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t        시간표 조회");
         Console.ForegroundColor = ConsoleColor.White;

         if (choice == ConstNumber.MENULIST_4)
               Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t        뒤 로 가 기");
         Console.ForegroundColor = ConsoleColor.White;

         Console.WriteLine("\t      ===============");

         return screenHeight;
      }

      // 강의 리스트 출력 헤드 (검색 기능)
      public static int PrintLectureListHeader()
      {
         int screenHeight = 45;
         Console.Clear();
         Console.SetWindowSize(183, screenHeight);
         Console.WriteLine(" ==================================================================================================================================================================================");
         Console.Write("  개설학과전공 >                 ");
         Console.Write("  교과목명 >                                       ");
         Console.Write("학수번호 >              ");
         Console.Write("  학년 >  ");
         Console.Write("    교수명 >                                       ");
         Console.Write(" [조회] ");
         Console.WriteLine("\n ==================================================================================================================================================================================");

         return screenHeight;
      }

      // 관심과목 담기 메뉴 화면
      public static int PrintInterestedSubjectMenu(int choice)
      {
         int screenHeight = 15;

         Console.SetWindowSize(42, screenHeight);
         Console.WriteLine("\t      ===============");

         if (choice == ConstNumber.MENULIST_1)
            Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t         강의 조회");
         Console.ForegroundColor = ConsoleColor.White;

         if (choice == ConstNumber.MENULIST_2)
            Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t       담긴 관심과목");
         Console.ForegroundColor = ConsoleColor.White;

         if (choice == ConstNumber.MENULIST_3)
            Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine("\t        뒤 로 가 기");
         Console.ForegroundColor = ConsoleColor.White;

         Console.WriteLine("\t      ===============");

         return screenHeight;

      }

      // 시간표 출력 화면
      public static void PrintTimeTable()
      {
         int screenHeight = 35;
         Console.Clear();
         Console.SetWindowSize(183, screenHeight);

         Console.WriteLine("\n                             월               |               화               |               수               |               목               |               금               |");
         Console.WriteLine("--------------=====================================================================================================================================================================");
         Console.WriteLine(" 09:00-09:30 |");
         Console.WriteLine(" 10:00-10:30 |");
         Console.WriteLine(" 10:30-11:00 |");
         Console.WriteLine(" 11:00-11:30 |");
         Console.WriteLine(" 11:30-12:00 |");
         Console.WriteLine(" 12:00-12:30 |");
         Console.WriteLine(" 12:30-13:00 |");
         Console.WriteLine(" 13:00-13:30 |");
         Console.WriteLine(" 13:30-14:00 |");
         Console.WriteLine(" 14:00-14:30 |");
         Console.WriteLine(" 14:30-15:00 |");
         Console.WriteLine(" 15:00-15:30 |");
         Console.WriteLine(" 15:30-16:00 |");
         Console.WriteLine(" 16:00-16:30 |");
         Console.WriteLine(" 16:30-17:00 |");
         Console.WriteLine(" 17:00-17:30 |");
         Console.WriteLine(" 17:30-18:00 |");
         Console.WriteLine(" 18:00-18:30 |");
         Console.WriteLine(" 18:30-19:00 |");
         Console.WriteLine(" 19:00-19:30 |");
         Console.WriteLine(" 19:30-20:00 |");
         Console.WriteLine(" 20:00-20:30 |");
         Console.WriteLine(" 20:30-21:00 |");
      }
   }
}
