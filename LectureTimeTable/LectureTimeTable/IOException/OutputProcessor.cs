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
            if (student.StudentNo == null)
                return null;

            Console.SetCursorPosition(cursorLeft, 12);
            student.Password = inputProcessor.InputPassword(12, screenHeight, cursorLeft, 12);
            if (student.Password == null)
                return null;

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

        public string[] LectureListHeaderScreen()
        {
            int choice = 0, screenHeight;
            string[] conditions = new string[] { "전체", "전체", "전체", "전체", "전체" };
            
            // 커서 좌표 배열
            CursorPoint[] titlePosition = new CursorPoint[] {
                new CursorPoint(17, 1), new CursorPoint(46, 1), new CursorPoint(17, 2), new CursorPoint(46, 2),
                new CursorPoint(64, 2), new CursorPoint(95, 1)
            };
            // 전공 목록 생성
            List<string> majorItems = new List<string>(new string[] {"전체", "컴퓨터공학과", "정보보호학과", "디지털콘텐츠학과", "소프트웨어학과" });
            // 학년 목록 생성
            List<string> gradeItems = new List<string>(new string[] {"전체", "1", "2", "3", "4" });

            screenHeight = ConsoleUI.PrintLectureListHeader();

            while (true) {
                while (true) {
                    // 엔터가 눌렸는지에 대한 상태변수
                    bool isPressedEnter = false;
                    // 선택된 위치로 커서 이동
                    Console.SetCursorPosition(titlePosition[choice].CursorLeft, titlePosition[choice].CursorTop);

                    switch (inputProcessor.PressDirectionKey())
                    {
                        case ConstNumber.ESC:
                            return null;
                        case ConstNumber.UP:
                        case ConstNumber.LEFT:
                            choice--;
                            break;
                        case ConstNumber.DOWN:
                        case ConstNumber.RIGHT:
                            choice++;
                            break;
                        case ConstNumber.ENTER:
                            isPressedEnter = true;
                            break;
                    }

                    if (isPressedEnter)
                        break;

                    // 선택 범위 제한
                    if (choice < 0)
                        choice = 0;
                    if (choice >= 6)
                        choice = 5;
                }

                // 현재 커서 위치
                CursorPoint presentCursor = new CursorPoint(Console.CursorLeft, Console.CursorTop);

                switch (choice) {
                    case ConstNumber.MENULIST_1:
                        conditions[0] = majorItems[ComboBox(majorItems, presentCursor, "                ")];
                        break;
                    case ConstNumber.MENULIST_2:
                        conditions[1] = inputProcessor.ReadAndCheckString(30, screenHeight, presentCursor, ConstNumber.GENERAL_LIMIT);
                        break;
                    case ConstNumber.MENULIST_3:
                        conditions[2] = inputProcessor.ReadAndCheckString(6, screenHeight, presentCursor, ConstNumber.NUMBER_LIMIT);
                        break;
                    case ConstNumber.MENULIST_4:
                        conditions[3] = gradeItems[ComboBox(gradeItems, presentCursor, "   ")];
                        break;
                    case ConstNumber.MENULIST_5:
                        conditions[4] = inputProcessor.ReadAndCheckString(30, screenHeight, presentCursor, ConstNumber.GENERAL_LIMIT);
                        break;
                    case ConstNumber.MENULIST_6:
                        return conditions;
                }
            }
        }

        public void LectureListScreen(HandleExcel.ExcelHandler LectureData, string[] conditions) {
            Console.WriteLine("\n ------------------------------------------------------------------------------------------------------------");
            for (int idx = 1; idx < 12; idx++)
            {
                Console.Write(LectureData.ToStringPresentData(1, idx));
            }
            Console.Write("\n ------------------------------------------------------------------------------------------------------------\n  ");
            Console.ReadKey();
        }

        public int ComboBox(List<string> items, CursorPoint cursor, string blankString) {
            int choice = 0, inputKey;

            while(true)
            {
                // 현재 선택 중인 문자열을 출력
                Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
                Console.Write(blankString);
                Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
                Console.Write(items[choice]);

                inputKey = inputProcessor.PressDirectionKey();
                switch (inputKey)
                {
                    case ConstNumber.LEFT:
                    case ConstNumber.UP:
                        choice--;
                        break;
                    case ConstNumber.RIGHT:
                    case ConstNumber.DOWN:
                        choice++;
                        break;
                    case ConstNumber.ESC:
                        return ConstNumber.ESC;
                    // Enter가 입력되면 선택 인덱스를 출력
                    default:
                        return choice;
                }

                // 선택 범위 제한
                if (choice < 0)
                    choice = 0;
                if (choice >= items.Count)
                    choice = items.Count - 1;
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
        
        
    }
}
