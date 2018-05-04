using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Library
{
   class BookManagement
   {
      Data.Student student;
      IOException.OutputProcessor outputProcessor;
      RentalManagement rentalManager;
      Data.DBHandler DB;
      int bookno = 1;

      public BookManagement(Data.Student student, Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.student = student;
         this.outputProcessor = outputProcessor;
         this.DB = DB;
         rentalManager = new RentalManagement(student);
         bookno = DB.GetBookCount() + 1;
      }

      public BookManagement(Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.student = null;
         rentalManager = null;
         this.outputProcessor = outputProcessor;
         this.DB = DB;
         bookno = DB.GetBookCount() + 1;
      }

      public string CreateBookNo()
      {
         string returnbookno = "";
         for (int i = 0; i < 6 - (bookno + "").Length; i++)
            returnbookno += "0";
         returnbookno += (bookno + "");
         return returnbookno;
      }

      public void UserRentalSystem()
      {
         switch(outputProcessor.MenuScreen(ConsoleUI.RENTAL_MENU))
         {
            case ConstNumber.MENULIST_1:
               SearchBook();
               break;
            case ConstNumber.MENULIST_2:
               ViewAllBook();
               break;
            case ConstNumber.MENULIST_3:
               rentalManager.ViewRentalList();
               break;
            case ConstNumber.MENULIST_4:
               return;
         }
      }

      public void AddBook()
      {
         Data.Book book;
         book = outputProcessor.BookRegistrationScreen();
         if (book == null)
            return;

         book.BookNo = CreateBookNo();
         if (DB.InsertBook(book))
            outputProcessor.PressAnyKey(book.Name + "이 등록되었습니다.");
         else
            outputProcessor.PressAnyKey("책 등록 실패");
      }

      public void AlterMember()
      {
         string modification = null;
         string attribute = null;
         string studentNo = outputProcessor.PrintMemberList(DB);

         switch (outputProcessor.MenuScreen(ConsoleUI.MEMBER_MODIFY))
         {
            // 멤버 이름 수정
            case ConstNumber.MENULIST_1:
               modification = outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_NAME);
               attribute = "membername";
               break;
            // 멤버 주소 수정
            case ConstNumber.MENULIST_2:
               outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_ADDRESS);
               attribute = "address";
               break;
            // 멤버 전화번호 수정
            case ConstNumber.MENULIST_3:
               outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_PHONENUMBER);
               attribute = "phonenumber";
               break;
            case ConstNumber.MENULIST_4:
               return;
         }
         if (modification == null)
            return;

         // DB에서 변경
         if (!DB.UpdateMemberInformation(studentNo, modification, attribute))
            outputProcessor.PressAnyKey("회원 수정 실패 : 중복 학번");
      }

      public void DeleteMember()
      {
         Console.Clear();
         Console.WriteLine("\n   삭제할 멤버의 학번을 입력하세요.");
         Console.Write("   → ");
         string studentNo = outputProcessor.inputProcessor.InputStudentNoFormat(Console.CursorLeft);

         if (outputProcessor.YesOrNo("해당 멤버를 정말 삭제하시겠습니까?") == 1)
         {
            if (!DB.DeleteMember(studentNo))
               outputProcessor.PressAnyKey("삭제할 멤버가 존재하지 않습니다.");
         }
      }

      public void SearchBook()
      {

      }

      public void ViewAllBook()
      {

      }

      public void ManageBooks()
      {
         while (true) {
            switch(outputProcessor.MenuScreen(ConsoleUI.BOOK_MENU))
            {
               // 서적 추가
               case ConstNumber.MENULIST_1:
                  AddBook();
                  break;
               case ConstNumber.MENULIST_2:
                  break;
               case ConstNumber.MENULIST_3:
                  break;
               case ConstNumber.MENULIST_4:
                  break;
               case ConstNumber.MENULIST_5:
                  return;
            }
         }
      }
   }
}
