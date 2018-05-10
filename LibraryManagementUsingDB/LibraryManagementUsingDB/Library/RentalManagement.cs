using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Library
{
   /// <summary>
   ///  대여 관리 클래스
   /// </summary>
   class RentalManagement
   {
      Data.Student student;
      Data.DBHandler DB;
      IOException.OutputProcessor output = new IOException.OutputProcessor();

      public RentalManagement(Data.DBHandler DB, Data.Student student)
      {
         this.student = student;
         this.DB = DB;
      }

      // 책을 직접 검색한 뒤 대여
      public void RentalBookSearch()
      {
         string search = "";
         string attribute = "";
         switch (output.MenuScreen(ConsoleUI.BOOK_SEARCH))
         {
            // 책 이름 검색
            case ConstNumber.MENULIST_1:
               search = output.GetBookInformation(ConstNumber.BOOK_NAME);
               attribute = "bookname";
               break;
            // 책 출판사 검색
            case ConstNumber.MENULIST_2:
               search = output.GetBookInformation(ConstNumber.BOOK_COMPANY);
               attribute = "company";
               break;
            // 멤버 전화번호 검색
            case ConstNumber.MENULIST_3:
               search = output.GetBookInformation(ConstNumber.BOOK_WRITER);
               attribute = "writer";
               break;
            case ConstNumber.MENULIST_4:
               return;
         }
         if (search == null)
            return;

         // 책 검색
         string bookno = DB.SearchBook(search, attribute);
         if (bookno == null) {
            output.PressAnyKey("검색 실패");
            return;
         }

         // 찾은 책 대여
         Rental(bookno);
      }

      // 현재 존재하는 전체 책 목록을 출력 후 대여
      public void RentalBookOfAll()
      {
         Data.Book book = output.PrintBookList(DB.GetAllBooks());
         if (book == null)
            return;

         Rental(book.ISBN);
      }

      // 대여, 연장, 반납을 하는 함수
      public void Rental(string bookno)
      {
         // 대출중인지 확인
         if (DB.CheckRental(bookno))
         {
            if (DB.IsBorrowSamePerson(student.StudentNo, bookno))
            {
               if (output.YesOrNo("연장하시겠습니까?") == 1)
               {
                  if (DB.Extension(bookno))
                     output.PressAnyKey("반납 기한은 " + DB.GetDueTo(bookno));
                  else
                     output.PressAnyKey("연장 실패");
               }
               else if (output.YesOrNo("반납하시겠습니까?") == 1)
                  DB.DeleteRental(bookno);
            }
            else
               output.PressAnyKey("대출 중 입니다.");
         }
         else
         {
            if (output.YesOrNo("대여하시겠습니까?") == 1)
            {
               if (DB.Rental(student.StudentNo, bookno))
                  output.PressAnyKey("반납 기한은 " + DB.GetDueTo(bookno));
            }
            else
               return;
         }
      }

      // 전체 대출 목록을 보여주고 반납, 연장을 할 수 있다.
      public void ViewRentalList()
      {
         string bookno = output.PrintRentalList(DB, student.StudentNo);
         if (bookno == null)
            return;

         Rental(bookno);
      }
   }
}
