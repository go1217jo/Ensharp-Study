using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.IOException
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

      // 이름 형식으로 입력받아 문자열을 반환하는 함수
      public string NameFormatInput(int cursorLeft)
      {
         ConsoleKeyInfo inputKey;
         string inputString = "";

         while (inputString.Length < 3)
         {
            inputKey = Console.ReadKey(true);

            // ESC가 입력되면 탈출함
            if (inputKey.Key == ConsoleKey.Escape)
               return null;
            // 한글이면
            if (Encoding.Default.GetBytes(inputKey.KeyChar + "").Length == 2)
            {
               // 정규표현식을 이용해 자음만 들어오는 것을 방지
               if (IsPerfectHangleChar(inputKey.KeyChar))
               {
                  inputString += inputKey.KeyChar;
                  Console.Write(inputKey.KeyChar + "");
                  if (inputString.Length == 3)
                     break;
               }
               else
                  // 자음이나 모음만 들어왔을 경우
                  Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
            }
            else
            {
               // 백스페이스에 대한 예외처리
               if (inputKey.Key == ConsoleKey.Backspace)
               {
                  Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                  Console.Write("  ");
                  Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                  if (inputString.Length > 0)
                     inputString = inputString.Remove(inputString.Length - 1);
                  if (cursorLeft > Console.CursorLeft)
                     Console.SetCursorPosition(cursorLeft, Console.CursorTop);
               }
            }
         }

         // 이름을 전부 입력한 뒤 들어오는 키 입력에 대한 예외처리
         inputKey = Console.ReadKey();
         // ESC가 입력되면 탈출함
         if (inputKey.Key == ConsoleKey.Escape)
            return null;
         if (inputKey.Key != ConsoleKey.Enter)
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
         else
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
         Console.Write("  ");

         return inputString;
      }
      
      // password 입력 받을 시 사용
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
                  if (input.Length < letterLimit)
                  {
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
            if (inputKey.KeyChar >= '0' && inputKey.KeyChar <= '9')
            {
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
            }
         }
         return inputString;
      }

      public string AddressFormatInput(CursorPoint cursor)
      {
         int choice = 0;
         string[] city = { "서울특별시", "인천광역시", "대전광역시", "대구광역시", "울산광역시", "부산광역시", "광주광역시", "경기도", "강원도" };
         string[] seoul = { "강서구", "양천구", "구로구", "마포구", "영등포구", "은평구", "서대문구", "종로구", "중구", "용산구", "동작구", "관악구", "도봉구", "강북구", "성북구", "동대문구", "성동구", "서초구", "노원구", "중랑구", "광진구", "강남구", "송파구", "강동구" };
         string[] incheon = { "중구", "동구", "남구", "연수구", "남동구", "부평구", "계양구", "서구", "강화군", "옹진군" };
         string[] daegeon = { "유성구", "서구", "중구", "대덕구", "동구" };
         string[] daegoo = { "남구", "달서구", "동구", "북구", "서구", "수성구", "중구" };
         string[] ulsan = { "남구", "동구", "북구", "중구" };
         string[] busan = { "중구", "서구", "동구", "영도구", "부산진구", "동래구", "남구", "북구", "해운대구", "사하구", "금정구", "강서구", "연제구", "수영구", "사상구", "기장군" };
         string[] gwangju = { "광산구", "남구", "동구", "북구", "서구" };
         string[] gyunggi = { "수원시", "고양시", "성남시", "부천시", "안양시", "광명시", "평택시", "안산시", "과천시", "오산시", "시흥시", "군포시", "의왕시", "하남시", "용인시", "이천시", "안성시", "김포시", "화성시", "광주시", "의정부시", "동두천시", "구리시", "남양주시", "파주시", "양주시", "포천시", "여주시", "연천군", "가평군", "양평군" };
         string[] gangwon = { "원주시", "춘천시", "강릉시", "동해시", "속초시", "삼척시", "태백시" };
         string[][] districtSets = { seoul, incheon, daegeon, daegoo, ulsan, busan, gwangju, gyunggi, gangwon };
         Hashtable districts = new Hashtable();

         // 지역 해시테이블 초기화
         for (int i = 0; i < city.Length; i++)
            districts[city[i]] = districtSets[i];

         string inputString = "";

         // 시 선택
         choice = ComboBox(new List<string>(city), cursor, "          ");
         if (choice == ConstNumber.ESC)
            return null;

         // 시 선택 저장
         inputString += city[choice];
         string[] chosenCity = (string[])districts[inputString];
         inputString += " ";

         // 도 선택
         choice = ComboBox(new List<string>(chosenCity), new CursorPoint(cursor.CursorLeft+inputString.Length*2 - 1, cursor.CursorTop), "        ");
         if (choice == ConstNumber.ESC)
            return null;

         // 도 선택 저장
         inputString += chosenCity[choice];

         return inputString;
      }

      // 전화번호 형식으로 입력받아 문자열을 반환하는 함수
      public string PhoneNumberFormatInput(CursorPoint cursor)
      {
         ConsoleKeyInfo inputKey;

         string inputString = "01";
                  
         // 전화번호 형식 지정
         Console.ForegroundColor = ConsoleColor.DarkGray;
         Console.Write("01_-____-____");
         Console.ForegroundColor = ConsoleColor.White;
         Console.SetCursorPosition(cursor.CursorLeft+2, cursor.CursorTop);

         while (inputString.Length < 13)
         {
            inputKey = Console.ReadKey(true);
            // ESC가 입력되면 탈출함
            if (inputKey.Key == ConsoleKey.Escape)
               return null;
            // 숫자면 문자열에 추가한다
            if (inputKey.KeyChar >= '0' && inputKey.KeyChar <= '9')
            {
               inputString += inputKey.KeyChar;
               Console.Write(inputKey.KeyChar + "");
               if (inputString.Length == 3 || inputString.Length == 8)
               {
                  Console.Write("-");
                  inputString += '-';
               }
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
                  if (cursor.CursorLeft > Console.CursorLeft)
                     Console.SetCursorPosition(cursor.CursorLeft, Console.CursorTop);
               }
            }
         }
         return inputString;
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

            inputKey = PressDirectionKey();
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
   }
}
