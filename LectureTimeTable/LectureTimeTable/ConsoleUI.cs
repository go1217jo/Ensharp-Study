using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable
{
    class ConsoleUI
    {
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
            Console.WriteLine("\t       신청강좌 조회");
            Console.ForegroundColor = ConsoleColor.White;

            if (choice == ConstNumber.MENULIST_4)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t        뒤 로 가 기");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\t      ===============");

            return screenHeight;
        }

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

    }
}
