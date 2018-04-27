using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable.IOException
{
    class OutputProcessor
    {
        InputProcessor inputProcessor;

        public OutputProcessor()
        {
            inputProcessor = new InputProcessor();
        }

        // 로그인 데이터를 받아 저장
        public StudentManagement.Student LoginScreen()
        {
            StudentManagement.Student student = new StudentManagement.Student();
            int cursorLeft = 20;
            int screenHeight = ConsoleUI.PrintLoginPage();
            // 학번 입력 위치로 커서 이동
            Console.SetCursorPosition(cursorLeft, 10);
            student.StudentNo = inputProcessor.InputStudentNoFormat(cursorLeft);
            Console.SetCursorPosition(cursorLeft, 12);
            student.Password = inputProcessor.InputPassword(12, screenHeight, cursorLeft, 12);

            return student;            
        }

        public int MainMenuScreen()
        {
            int choice = ConstNumber.MENULIST_1;

            ConsoleUI.PrintLogo();
            while (true)
            {
                // 출력 위치로 이동
                Console.SetCursorPosition(0, 8);
                // 메뉴 화면 출력
                ConsoleUI.PrintMainMenu(choice);

                // 현재 선택 중인 항목에 커서를 위치시킨다.
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 항목의 인덱스를 반환한다
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 위치를 제한한다.
                if (Console.CursorTop < 9)
                    Console.SetCursorPosition(Console.CursorLeft, 9);
                if (Console.CursorTop > 11)
                    Console.SetCursorPosition(Console.CursorLeft, 11);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;
            }
        }

        public int SearchMenuScreen()
        {
            int choice = ConstNumber.MENULIST_1;

            ConsoleUI.PrintLogo();
            while (true)
            {
                // 출력 위치로 이동
                Console.SetCursorPosition(0, 8);
                // 메뉴 화면 출력
                ConsoleUI.PrintSearchMenu(choice);

                // 현재 선택 중인 항목에 커서를 위치시킨다.
                Console.SetCursorPosition(0, 9 + choice);

                // 엔터를 치면 선택된 항목의 인덱스를 반환한다
                if (inputProcessor.ChoiceByKey())
                    return choice;

                // 커서의 위아래 위치를 제한한다.
                if (Console.CursorTop < 9)
                    Console.SetCursorPosition(Console.CursorLeft, 9);
                if (Console.CursorTop > 12)
                    Console.SetCursorPosition(Console.CursorLeft, 12);

                // 커서 위치에 따른 메뉴 선택
                choice = Console.CursorTop - 9;
            }
        }

        // 엔터 누르면 다른 화면으로 이동하게 하는 함수
        public void PressAnyKey(string comment)
        {
            Console.Clear();
            Console.WriteLine("\n\t" + comment);
            while (true)
            {
                if (Console.ReadKey().KeyChar != 0)
                {  // 아무키나 누를 때까지 반복
                    Console.Clear();
                    break;
                }
            }
        }
        
        public void LectureListHeader()
        {
            Console.SetWindowSize(110, 35);
            Console.WriteLine(" ============================================================================================================");
            Console.Write("  개설학과전공 >                 ");

            Console.Write(" 교과목명 >                                         ");
            Console.WriteLine("         [조회] ");
            Console.Write("      학수번호 >              ");
            Console.Write("        학년 >  ");
            Console.Write("       교수명 >                                           ");
            Console.WriteLine("\n ============================================================================================================");
            Console.ReadKey();
        }
    }
}
