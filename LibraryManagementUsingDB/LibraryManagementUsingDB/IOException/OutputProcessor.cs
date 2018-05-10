using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
         if (student.Password == null || student.Password.Contains("\\"))
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
            Console.SetCursorPosition(0, 8);
            // 메뉴 화면 출력
            ConsoleUI.PrintMenu(menuString, choice);

            // 현재 선택 중인 항목에 커서를 위치시킨다.
            Console.SetCursorPosition(0, 9 + choice);

            // 엔터를 치면 선택된 항목의 인덱스를 반환한다
            if (inputProcessor.ChoiceByKey())
               return choice;

            // 커서의 위아래 위치를 제한한다.
            if (Console.CursorTop < 9)
               Console.SetCursorPosition(Console.CursorLeft, 9);
            if (Console.CursorTop > 9 + menuString.Length - 1 )
               Console.SetCursorPosition(Console.CursorLeft, 9 + menuString.Length - 1);

            // 커서 위치에 따른 메뉴 선택
            choice = Console.CursorTop - 9;
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
         Console.SetWindowSize(70, 10);
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

      // 회원들의 정보를 출력한다.
      public string PrintMemberList(Data.DBHandler DB)
      {
         int choice = 0;
         List<Data.Student> students = null;
         Console.SetWindowSize(79, 39);
         Console.Clear();
         
         Console.WriteLine("\n ============================================================================");
         Console.WriteLine("     학   번         이   름              주   소           핸드폰 번호");
         Console.WriteLine(" ============================================================================");
         while (true)
         {
            Console.SetCursorPosition(0, 4);
            students = DB.GetAllMember();
            if (students.Count == 0)
            {
               PressAnyKey("멤버가 없습니다.");
               return null;
            }
            for (int i = 0; i < students.Count; i++)
            {
               if (choice == i)
                  Console.ForegroundColor = ConsoleColor.Red;
               students[i].PrintInformation();
               Console.ForegroundColor = ConsoleColor.White;
               Console.WriteLine();
            }
            Console.SetCursorPosition(0, 4 + choice);

            if (inputProcessor.ChoiceByKey())
               return students[choice].StudentNo;

            if (Console.CursorTop < 4)
               Console.SetCursorPosition(Console.CursorLeft, 4);
            if (Console.CursorTop > 4 + ((students.Count == 0) ? 1 : students.Count) - 1)
               Console.SetCursorPosition(Console.CursorLeft, 4 + ((students.Count == 0) ? 1 : students.Count) - 1);

            // 커서 위치에 따른 메뉴 선택  
            choice = Console.CursorTop - 4;
         }
      }

      public Data.Book PrintBookList(List<Data.Book> books)
      {
         int choice = 0;
         Console.Clear();
         Console.SetWindowSize(167, 40);
         Console.WriteLine("\n ====================================================================================================================================================================");
         Console.WriteLine("           ISBN                                  도서명                                    출판사                           저자                가격  수량   출판일");
         Console.WriteLine(" ====================================================================================================================================================================");

         while (true)
         {
            Console.SetCursorPosition(0, 4);
            if (books.Count == 0)
            {
               PressAnyKey("도서가 없습니다.");
               return null;
            }
            for (int i = 0; i < books.Count; i++)
            {
               // 선택된 행이면 빨간색으로 표시
               if (choice == i)
                  Console.ForegroundColor = ConsoleColor.Red;
               books[i].PrintInformation();
               Console.ForegroundColor = ConsoleColor.White;
            }

            // 첫 행으로 커서를 옮긴다
            Console.SetCursorPosition(0, 4 + choice);

            // 엔터를 치면 선택된 인덱스를 반환한다.
            if (inputProcessor.ChoiceByKey())
               return books[choice];

            // 커서의 위아래 이동 구간을 제한한다.
            if (Console.CursorTop < 4)
               Console.SetCursorPosition(Console.CursorLeft, 4);
            if (Console.CursorTop > 4 + ((books.Count == 0) ? 1 : books.Count) - 1)
               Console.SetCursorPosition(Console.CursorLeft, 4 + ((books.Count == 0) ? 1 : books.Count) - 1 );

            // 커서 위치에 따른 메뉴 선택
            choice = Console.CursorTop - 4;
         }
      }

      public string PrintRentalList(Data.DBHandler DB, string studentno)
      {
         int choice = 0;
         List<Data.Book> books = null;
         Console.Clear();
         Console.SetWindowSize(143, 39);
         Console.WriteLine("\n ====================================================================================================");
         Console.WriteLine("        ISBN             도서명                 출판사                  저자            반납 기간");
         Console.WriteLine(" ====================================================================================================");

         while (true)
         {
            Console.SetCursorPosition(0, 4);
            books = DB.RentalList(studentno);

            if (books.Count == 0)
            {
               PressAnyKey("대출된 도서가 없습니다.");
               return null;
            }

            for (int i = 0; i < books.Count; i++)
            {
               // 선택된 행이면 빨간색으로 표시
               if (choice == i)
                  Console.ForegroundColor = ConsoleColor.Red;
               books[i].PrintDuetoInformation();
               Console.ForegroundColor = ConsoleColor.White;
               Console.WriteLine();
            }

            // 첫 행으로 커서를 옮긴다
            Console.SetCursorPosition(0, 4 + choice);

            // 엔터를 치면 선택된 인덱스를 반환한다.
            if (inputProcessor.ChoiceByKey())
               return books[choice].ISBN;

            // 커서의 위아래 이동 구간을 제한한다.
            if (Console.CursorTop < 4)
               Console.SetCursorPosition(Console.CursorLeft, 4);
            if (Console.CursorTop > 4 + ((books.Count == 0) ? 1 : books.Count) - 1)
               Console.SetCursorPosition(Console.CursorLeft, 4 + ((books.Count == 0) ? 1 : books.Count) - 1);

            // 커서 위치에 따른 메뉴 선택
            choice = Console.CursorTop - 4;
         }
      }
      public int InputBookCount()
      {
         Console.SetWindowSize(42, 16);
         while (true) {
            Console.Clear();
            Console.WriteLine("\n   수량을 입력하세요. (30권 이하)\n");
            Console.Write("   >> ");
            string input = Console.ReadLine();
            if (input.Length == 0)
               continue;
            if (Regex.IsMatch(input, "^[0-9]*$"))
            {
               if (int.Parse(input) <= 30)
                  return int.Parse(input);
            }
            PressAnyKey("50 이하의 숫자를 입력해주세요.\n  이 화면에서는 돌아갈 수 없습니다.");
            Console.SetWindowSize(42, 16);
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

      public Data.Student MemberRegistrationScreen()
      {
         Data.Student newStudent = new Data.Student();

         // 각각 항목들에 대해 문자열을 입력받는다.

         // 이름 입력 위치로 이동
         Console.SetCursorPosition(14, 11);
         // 이름 입력 형식 지정
         Console.ForegroundColor = ConsoleColor.DarkGray;
         Console.Write("성이름");
         Console.SetCursorPosition(14, 11);
         Console.ForegroundColor = ConsoleColor.White;
         newStudent.name = inputProcessor.NameFormatInput(14);
         if (newStudent.name == null)
            return null;

         // 학번 입력 위치로 이동
         Console.SetCursorPosition(14, 13);
         // 학번 형식 지정
         Console.ForegroundColor = ConsoleColor.DarkGray;
         Console.Write("________");
         Console.ForegroundColor = ConsoleColor.White;
         Console.SetCursorPosition(14, 13);
         newStudent.StudentNo = inputProcessor.InputStudentNoFormat(14);
         if (newStudent.StudentNo == null)
            return null;

         CursorPoint cursor = new CursorPoint(14, 15);
         // 주소 입력 위치로 이동
         Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
         Console.ForegroundColor = ConsoleColor.DarkGray;
         Console.Write("주소 선택");
         Console.ForegroundColor = ConsoleColor.White;
         Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
         // 키보드로 주소 선택
         newStudent.address = inputProcessor.AddressFormatInput(cursor);
         if (newStudent.address == null)
            return null;

         cursor = new CursorPoint(14, 17);
         // 전화번호 입력 위치로 이동
         Console.SetCursorPosition(cursor.CursorLeft, cursor.CursorTop);
         newStudent.phoneNumber = inputProcessor.PhoneNumberFormatInput(cursor);
         if (newStudent.phoneNumber == null)
            return null;

         Console.SetCursorPosition(14, 19);
         newStudent.Password = inputProcessor.InputPassword(10, 30, 14, 19);
         if (newStudent.Password == null)
            return null;

         Console.Clear();
         return newStudent;
      }

      public string AlterMemberInformation(string studentNo, int attribute)
      {
         string modification = null;
         ConsoleUI.PrintEnsharpLogo();
         Console.WriteLine("\n   수정 내용 입력");
         Console.Write("   → ");
         
         switch(attribute)
         {
            case ConstNumber.MEMBER_NAME:
               modification = inputProcessor.NameFormatInput(Console.CursorLeft);
               break;
            case ConstNumber.MEMBER_ADDRESS:
               modification = inputProcessor.AddressFormatInput(new CursorPoint(Console.CursorLeft, Console.CursorTop));
               break;
            case ConstNumber.MEMBER_PHONENUMBER:
               modification = inputProcessor.PhoneNumberFormatInput(new CursorPoint(Console.CursorLeft,Console.CursorTop));
               break;
         }
         return modification;
      }

      public string GetBookInformation(int attribute)
      {
         string modification = null;
         ConsoleUI.PrintEnsharpLogo();
         Console.WriteLine("\n   정보 입력");
         Console.Write("   → ");

         switch (attribute)
         {
            case ConstNumber.BOOK_NAME:
               modification = inputProcessor.ReadAndCheckString(25, 16, Console.CursorLeft, Console.CursorTop);
               break;
            case ConstNumber.BOOK_COMPANY:
               modification = inputProcessor.ReadAndCheckString(15, 16, Console.CursorLeft, Console.CursorTop);
               break;
            case ConstNumber.BOOK_WRITER:
               modification = inputProcessor.ReadAndCheckString(20, 16, Console.CursorLeft, Console.CursorTop);
               break;
         }

         return modification;
      }

      // API를 통해 책을 검색하는 화면
      public List<Data.Book> APISearchScreen()
      {
         string keyword;
         NaverAPI.SearchEngine engine = new NaverAPI.SearchEngine();
         ConsoleUI.PrintAPISearchBook();

         // 검색어를 입력받는다.
         Console.SetCursorPosition(14, 11);
         keyword = inputProcessor.ReadAndCheckString(25, 25, 14, 11);
         if (keyword == null)
            return null;

         return engine.SearchBooks(keyword);
      }
   }
}
