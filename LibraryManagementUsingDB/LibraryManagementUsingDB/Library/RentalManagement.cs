using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Library
{
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

      public void RentalBookOfAll()
      {
         string bookno = output.PrintBookList(DB);
         if (bookno == null)
            return;

         Rental(bookno);
      }

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

      public void ViewRentalList()
      {
         string bookno = output.PrintRentalList(DB, student.StudentNo);
         if (bookno == null)
            return;

         Rental(bookno);
      }
   }
}
