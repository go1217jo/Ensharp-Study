using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Library
{
   /// <summary>
   ///  도서 관리 클래스
   /// </summary>
   class BookManagement
   {
      Data.Student student;
      IOException.OutputProcessor outputProcessor;
      RentalManagement rentalManager;
      Data.DBHandler DB;

      public BookManagement(Data.Student student, Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.student = student;
         this.outputProcessor = outputProcessor;
         this.DB = DB;
         rentalManager = new RentalManagement(DB, student);
      }

      public BookManagement(Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.student = null;
         rentalManager = null;
         this.outputProcessor = outputProcessor;
         this.DB = DB;
      }

      // 유저로 로그인 시 이용 가능한 메뉴
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

      // 책 추가
      public void AddBook()
      {
         List<Data.Book> books;
         books = outputProcessor.APISearchScreen(DB);
         if (books == null)
            return;
         
         Data.Book book = outputProcessor.PrintBookList(books);
         if (book == null)
            return;
         book.Count = outputProcessor.InputBookCount();

         if (book != null && DB.InsertBook(book))
         {
            outputProcessor.PressAnyKey(book.Name + "이 등록되었습니다.");
            // 로그 기록
            DB.InsertLog("관리자", book.Name, "도서 추가");
         }
         else
            outputProcessor.PressAnyKey("책 등록 실패");
      }

      // 책 수량 수정
      public void AlterBookCount()
      {
         Data.Book book = outputProcessor.PrintBookList(DB.GetAllBooks());
         if (book == null)
            return;

         if (DB.ModifyBookCount(book.ISBN, outputProcessor.InputBookCount()))
            DB.InsertLog("관리자", book.Name, "수량 수정");  // 로그 기록
      }

      // 책 삭제
      public void DeleteBook()
      {
         Console.Clear();
         Data.Book book = outputProcessor.PrintBookList(DB.GetAllBooks());
         if (book == null)
            return;
         if (outputProcessor.YesOrNo("해당 책을 정말 삭제하시겠습니까?") == 1)
         {
            if (!DB.DeleteBook(book.ISBN))
               outputProcessor.PressAnyKey("삭제할 책이 존재하지 않습니다.");
            // 로그 기록
            DB.InsertLog("관리자", book.Name, "도서 삭제");
         }
      }

      // 관리자 책 관리 메뉴
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
               // 서적 수량 수정
               case ConstNumber.MENULIST_3:
                  AlterBookCount();
                  break;
               // 전체 보기
               case ConstNumber.MENULIST_4:
                  outputProcessor.PrintBookList(DB.GetAllBooks());
                  break;
               case ConstNumber.MENULIST_5:
                  return;
            }
         }
      }
   }
}
