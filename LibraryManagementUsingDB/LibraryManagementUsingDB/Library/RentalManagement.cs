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

      // 입력 받은 정보로 책을 검색하여 사용자에게 보여주고 선택하게 한다
      public Data.Book SearchBook(string attribute, string keyword)
      {
         bool coincide = false;
         List<Data.Book> books = DB.GetAllBooks();
         List<Data.Book> searchResults = new List<Data.Book>();

         // 전체 도서 목록에 대해 검사한다
         for(int idx=0; idx<books.Count; idx++)
         {
            coincide = false;
            if (attribute.Equals("bookname"))
               coincide = books[idx].GetName().Contains(keyword);
            else if (attribute.Equals("company"))
               coincide = books[idx].GetCompany().Contains(keyword);
            else
               coincide = books[idx].GetWriter().Contains(keyword);

            if (coincide)
               searchResults.Add(books[idx]);
         }

         if (searchResults.Count == 0)
         {
            output.PressAnyKey("검색 결과가 없습니다.");
            return null;
         }

         // 검색 결과 중 선택된 책을 반환
         return output.PrintBookList(searchResults);
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
            // 도서 저자 검색
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
         Data.Book searchResult = SearchBook(attribute, search);
         if (searchResult == null)
            return;
         // 로그 기록
         DB.InsertLog(student.StudentNo, search, "도서 검색");
         // 찾은 책 대여
         Rental(searchResult.ISBN);
      }

      // 현재 존재하는 전체 책 목록을 출력 후 대여
      public void RentalBookOfAll()
      {
         Data.Book book = output.PrintBookList(DB.GetAllBooks());
         if (book == null)
            return;

         Rental(book.ISBN);
      }

      // 대여 처리를 하는 함수
      public void Rental(string isbn)
      {
         int count = 0;
         // 해당 책의 수량을 받아온다.
         count = DB.GetBookCount(isbn);
         // 책 수량이 존재하는지 확인
         if (count != 0)
         {
            if (output.YesOrNo("대여하시겠습니까?") == 1)
            {
               // 중복되는 대여 도서가 있다면
               if (DB.IsOverlapBook(isbn, student.StudentNo)) {
                  output.PressAnyKey("이미 대여 중인 도서입니다.");
                  return;
               }

               // 책 수량 1개 감소
               DB.ModifyBookCount(isbn, count - 1);
               // 대여 처리
               DB.Rental(student.StudentNo, isbn);
               // 로그 기록
               DB.InsertLog(student.StudentNo, DB.GetBookName(isbn), "도서 대여");
               output.PressAnyKey("도서가 성공적으로 대여되었습니다.");
            }
         }
         else
            output.PressAnyKey("현재 해당 도서가 모두 대여 중 입니다.\n  (수량이 없습니다.)");            
      }

      // 전체 대출 목록을 보여주고 반납, 연장을 할 수 있다.
      public void ViewRentalList()
      {
         string dueto = null;
         string isbn = output.PrintRentalList(DB, student.StudentNo);
         if (isbn == null)
            return;
         
         if (output.YesOrNo("반납하시겠습니까?") == 1)
            ReturnBook(isbn);
         else
         {
            if (output.YesOrNo("연장하시겠습니까?") == 1)
            {
               dueto = DB.Extension(isbn);
               if (dueto != null)
               {
                  output.PressAnyKey(dueto + "까지 연장되었습니다");
                  // 로그 기록
                  DB.InsertLog(student.StudentNo, DB.GetBookName(isbn), "도서 연장");
               }
               else
                  output.PressAnyKey("연장 횟수 초과!");
            }
         }
      }

      // 도서 반납 처리
      public void ReturnBook(string isbn)
      {
         int count = DB.GetBookCount(isbn);
         // 도서 수 증가
         DB.ModifyBookCount(isbn, count + 1);
         // 대여 항목 삭제
         DB.DeleteRental(isbn);
         output.PressAnyKey("반납되었습니다.");
         // 로그 기록
         DB.InsertLog(student.StudentNo, DB.GetBookName(isbn), "도서 반납");
      }
   }
}
