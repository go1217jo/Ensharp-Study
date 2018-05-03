using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.IOException
{
   class OutputProcessor
   {
      public InputProcessor inputProcessor = new InputProcessor();

      // 로그인 데이터를 받아 Login 함수에서 처리
      public Data.Student LoginScreen()
      {
         Data.Student student = new Data.Student();
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

      public int MenuScreen(string[] menuString)
      {
         int choice = ConstNumber.MENULIST_1;

         ConsoleUI.PrintLogo();
         while (true)
         {
            // 출력 위치로 이동
            Console.SetCursorPosition(0, 9);
            // 메뉴 화면 출력
            ConsoleUI.PrintMenu(menuString, choice);

            // 현재 선택 중인 항목에 커서를 위치시킨다.
            Console.SetCursorPosition(0, 10 + choice);

            // 엔터를 치면 선택된 항목의 인덱스를 반환한다
            if (inputProcessor.ChoiceByKey())
               return choice;

            // 커서의 위아래 위치를 제한한다.
            if (Console.CursorTop < 10)
               Console.SetCursorPosition(Console.CursorLeft, 10);
            if (Console.CursorTop > 10 + menuString.Length - 1 )
               Console.SetCursorPosition(Console.CursorLeft, 10 + menuString.Length - 1);

            // 커서 위치에 따른 메뉴 선택
            choice = Console.CursorTop - 10;
         }
      }

      // 고정된 자리수의 문자열 출력
      public string PrintFixString(string toPrint, int limit)
      {
         if (toPrint == null)
            toPrint = "";
         // 바이트 수를 받아옴
         int presentLength = Encoding.Default.GetBytes(toPrint).Length;

         if ((limit - presentLength) % 2 == 1)
            toPrint = " " + toPrint;
         // 앞뒤 공백 붙이기
         for (int i = 0; i < (limit - presentLength) / 2; i++)
            toPrint = " " + toPrint + " ";
         return toPrint;
      }

      public int ComboBox(List<string> items, CursorPoint cursor, string blankString)
      {
         int choice = 0, inputKey;

         while (true)
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
         Console.WriteLine("\n  " + comment);
         while (true)
         {
            if (Console.ReadKey().KeyChar != 0)
            {  // 아무키나 누를 때까지 반복
               Console.Clear();
               break;
            }
         }
      }

      // 예 아니오를 선택하게 하는 화면 출력, 두 번째 줄 알림 출력 뒤 호출
      // 예 : 1, 아니오 : 2
      public int YesOrNo(string alert)
      {
         int choice = 1;
         Console.SetWindowSize(42, 16);
         Console.Clear();
         Console.WriteLine("\n   " + alert + "\n");
         while (true)
         {
            Console.SetCursorPosition(0, 3);
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
         }
      }
   }
}
