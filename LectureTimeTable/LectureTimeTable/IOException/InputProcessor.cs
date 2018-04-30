using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LectureTimeTable.IOException
{
   class InputProcessor
   {
        
      // 방향키 이외의 입력들에 대해 예외처리를 하며 방향키가 들어오면 설정해놓은 상수를 반환한다.
      public int PressDirectionKey()
      {
         ConsoleKeyInfo inputKey;
         while (true)
         {
            inputKey = Console.ReadKey(true);
            switch (inputKey.Key)
            {
               case ConsoleKey.Escape:
                  return ConstNumber.ESC;
               case ConsoleKey.LeftArrow:
                  return ConstNumber.LEFT;
               case ConsoleKey.RightArrow:
                  return ConstNumber.RIGHT;
               case ConsoleKey.UpArrow:
                  return ConstNumber.UP;
               case ConsoleKey.DownArrow:
                  return ConstNumber.DOWN;
               case ConsoleKey.Enter:
                  return ConstNumber.ENTER;
               case ConsoleKey.Backspace:
                  Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                  break;
               default:
                  break;
            }
         }
      }
        
      // 방향키에 따라 위 아래로만 움직이게 하고 엔터를 치면 true를 반환하는 함수
      public bool ChoiceByKey()
      {
         switch (PressDirectionKey())
         {
            case ConstNumber.UP:
               Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
               return false;
            case ConstNumber.DOWN:
               Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
               return false;
            case ConstNumber.ENTER:
               return true;
            default:
               Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
               return false;
         }
      }

      // 한글 자음, 모음만 입력되는 현상 제한
      public bool IsPerfectHangleChar(char checkChar)
      {
         string pattern = @"^[가-힣]*$";
         // 정규표현식을 이용해 자음만 들어오는 것을 방지
         if (Regex.IsMatch(checkChar + "", pattern))
            return true;
         else
            return false;
      }

      // 문자열을 읽어오면서 동시에 문자열이 조건을 충족하는지 확인하여 반환한다.
      // letterLimit : 최대 글자 수, screenHeight : 출력화면 높이
      // cursorLeft : 입력받기 시작하는 위치(x좌표), cursorTop : 입력받기 시작하는 위치(y좌표)
      // format : 포맷에 따라 문자 입력에 제한을 둠
      public string ReadAndCheckString(int letterLimit, int screenHeight, CursorPoint cursor, int format)
      {
         string input = "";
         ConsoleKeyInfo inputKey;

         // 이전 출력된 글자 지우기
         for (int iter = 0; iter < letterLimit; iter++)
               Console.Write(" ");
         Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);

         while (true)
         {
            // 문자열을 입력받으며 각각의 문자에 대해 검사함
            while (true)
            {
               inputKey = Console.ReadKey(true);
               if (inputKey.Key == ConsoleKey.Escape)
                  return null;
               else if (inputKey.Key == ConsoleKey.Enter)
                  break;
               else if (inputKey.Key == ConsoleKey.Backspace)
               {
                  int bytes;
                  if (input.Length != 0)
                  {
                     bytes = Encoding.Default.GetBytes(input.Last() + "").Length;

                     Console.SetCursorPosition(Console.CursorLeft - bytes, Console.CursorTop);
                     if (bytes == 2)
                        Console.Write("  ");
                     else
                        Console.Write(" ");
                     Console.SetCursorPosition(Console.CursorLeft - bytes, Console.CursorTop);
                     if (input.Length > 0)
                        input = input.Remove(input.Length - 1);
                     if (cursor.CursorLeft > Console.CursorLeft)
                        Console.SetCursorPosition(cursor.CursorLeft, Console.CursorTop);
                  }
               }
               else {
                  // 글자 수가 제한 수를 넘지 않았다면
                  if (input.Length < letterLimit) {
                     // 포맷 제한에 따라 문자를 입력받는다
                     if (format == ConstNumber.GENERAL_LIMIT) {
                        if (inputKey.KeyChar >= 33 && inputKey.KeyChar <= 126 || IsPerfectHangleChar(inputKey.KeyChar))
                        {
                           Console.Write(inputKey.KeyChar);
                           input += inputKey.KeyChar;
                        }
                     }
                     else if(format == ConstNumber.NUMBER_LIMIT) {
                        if (inputKey.KeyChar >= '0' && inputKey.KeyChar <= '9')
                        {
                           Console.Write(inputKey.KeyChar);
                           input += inputKey.KeyChar;
                        }
                     }
                  }
               }
            }

            // 입력되지 않은 경우 다시 입력
            if (input.Length == 0)
            {
               // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
               Console.SetCursorPosition(5, screenHeight - 2);
               Console.WriteLine("잘못된 입력 : 입력되지 않음");
               Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
               Console.Write("                 ");
               Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
               continue;
            }

            // 입력된 문자열을 반환한다.
            return input;
         }
      }

      // password 입력 받을 시 사용하기 위해 오버로딩
      public string InputPassword(int letterLimit, int screenHeight, int cursorLeft, int cursorTop)
      {
         string input = "";
         ConsoleKeyInfo passwordKey;

         while (true)
         {
            // password 입력받음 * 표시
            while (true)
            {
               passwordKey = Console.ReadKey(true);
               if (passwordKey.Key == ConsoleKey.Escape)
                  return null;
               if (passwordKey.Key == ConsoleKey.Enter)
                  break;
               if (passwordKey.KeyChar >= 33 && passwordKey.KeyChar <= 126)
               {
                  if (input.Length < letterLimit) {
                        Console.Write("*");
                        input += passwordKey.KeyChar;
                  }
               }
               if (passwordKey.Key == ConsoleKey.Backspace)
               {
                  Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                  Console.Write(" ");
                  Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                  if (input.Length > 0)
                        input = input.Remove(input.Length - 1);
                  if (cursorLeft > Console.CursorLeft)
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
               }
            }
                                
            // 입력되지 않은 경우 다시 입력
            if (input.Length == 0)
            {
               // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
               Console.SetCursorPosition(5, screenHeight - 2);
               Console.WriteLine("잘못된 입력 : 입력되지 않음");
               Console.SetCursorPosition(cursorLeft, cursorTop);
               Console.Write("                ");
               Console.SetCursorPosition(cursorLeft, cursorTop);
               continue;
            }
                
            // 입력된 문자열을 반환한다.
            return input;
         }
      }

      // 학번 형식으로 입력받아 문자열을 반환하는 함수
      public string InputStudentNoFormat(int cursorLeft)
      {
         string inputString = "";
         ConsoleKeyInfo inputKey;

         while (inputString.Length < 8)
         {
            inputKey = Console.ReadKey(true);
            // ESC가 입력되면 탈출함
            if (inputKey.Key == ConsoleKey.Escape)
               return null;
            if (inputKey.KeyChar >= '0' && inputKey.KeyChar <= '9') {
               inputString += inputKey.KeyChar;
               Console.Write(inputKey.KeyChar);
                    
            }
            else
            {
               // 백스페이스에 대한 예외처리
               if (inputKey.Key == ConsoleKey.Backspace)
               {
                  Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                  Console.Write(" ");
                  Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                  // 삭제한 문자 제거
                  if (inputString.Length > 0)
                        inputString = inputString.Remove(inputString.Length - 1);
                  if (cursorLeft > Console.CursorLeft)
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
               }
               // 한글과 다른 특수문자의 입력에 대비하여 바이트 수를 계산하여 커서를 옮긴다
               else
                  Console.SetCursorPosition(cursorLeft + inputString.Length, Console.CursorTop);
            }
         }

         return inputString;
      }    
   }
}
