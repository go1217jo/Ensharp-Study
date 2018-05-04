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
         rentalManager = new RentalManagement(DB, student);
         bookno = DB.GetBookCount() + 2;
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
         while (true)
         {
            switch (outputProcessor.MenuScreen(ConsoleUI.RENTAL_MENU))
            {
               // 책 검색
               case ConstNumber.MENULIST_1:
                  rentalManager.RentalBookSearch();
                  break;
               // 전체보기
               case ConstNumber.MENULIST_2:
                  rentalManager.RentalBookOfAll();
                  break;
               // 대출 목록 보기
               case ConstNumber.MENULIST_3:
                  rentalManager.ViewRentalList();
                  break;
               // 돌아가기
               case ConstNumber.MENULIST_4:
                  return;
            }
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

      public void AlterBook()
      {
         string modification = null;
         string attribute = null;
         string bookNo = outputProcessor.PrintBookList(DB);

         switch (outputProcessor.MenuScreen(ConsoleUI.BOOK_MODIFY))
         {
            // 책 이름 수정
            case ConstNumber.MENULIST_1:
               modification = outputProcessor.GetBookInformation(ConstNumber.BOOK_NAME);
               attribute = "bookname";
               break;
            // 책 출판사 수정
            case ConstNumber.MENULIST_2:
               modification = outputProcessor.GetBookInformation(ConstNumber.BOOK_COMPANY);
               attribute = "company";
               break;
            // 멤버 전화번호 수정
            case ConstNumber.MENULIST_3:
               modification = outputProcessor.GetBookInformation(ConstNumber.BOOK_WRITER);
               attribute = "writer";
               break;
            case ConstNumber.MENULIST_4:
               return;
         }
         if (modification == null)
            return;

         // DB에서 변경
         if (!DB.UpdateBookInformation(bookNo, modification, attribute))
            outputProcessor.PressAnyKey("책 정보 수정 실패");
      }

      public void DeleteBook()
      {
         Console.Clear();
         string bookno = outputProcessor.PrintBookList(DB);

         if (outputProcessor.YesOrNo("해당 책을 정말 삭제하시겠습니까?") == 1)
         {
            if (!DB.DeleteBook(bookno))
               outputProcessor.PressAnyKey("삭제할 책이 존재하지 않습니다.");
         }
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
               // 서적 삭제
               case ConstNumber.MENULIST_2:
                  DeleteBook();
                  break;
               // 서적 수정
               case ConstNumber.MENULIST_3:
                  AlterBook();
                  break;
               // 전체 보기
               case ConstNumber.MENULIST_4:
                  outputProcessor.PrintBookList(DB);
                  break;
               case ConstNumber.MENULIST_5:
                  return;
            }
         }
      }
   }
}
