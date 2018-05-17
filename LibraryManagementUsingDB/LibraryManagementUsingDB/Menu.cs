using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB
{
   /// <summary>
   ///  메뉴 관리 클래스
   /// </summary>
   class Menu
   {
      // 관리자로 로그인 시 보이는 메뉴
      public static void AdminScreen(Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         Member.MemberManagement memberManagement = new Member.MemberManagement(DB, outputProcessor);
         Library.BookManagement bookManagement = new Library.BookManagement(DB, outputProcessor);
         Data.LogManagement logManagement = new Data.LogManagement(DB, outputProcessor);
         while (true)
         {
            switch (outputProcessor.MenuScreen(ConsoleUI.ADMIN_MENU))
            {
               // 멤버 관리
               case ConstNumber.MENULIST_1:
                  ManageMember(memberManagement, DB, outputProcessor);
                  break;
               // 도서 관리
               case ConstNumber.MENULIST_2:
                  Menu.ManageBooks(bookManagement, DB, outputProcessor);
                  break;
               // 로그 관리
               case ConstNumber.MENULIST_3:
                  logManagement.ManageLog();
                  break;
               case ConstNumber.MENULIST_4:
                  return;
            }
         }
      }

      // 관리자 멤버 관리 메뉴
      public static void ManageMember(Member.MemberManagement memberManagement, Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         while (true)
         {
            switch (outputProcessor.MenuScreen(ConsoleUI.MEMBER_MENU))
            {
               // 회원 추가
               case ConstNumber.MENULIST_1:
                  memberManagement.AddMember();
                  break;
               // 회원 정보 수정
               case ConstNumber.MENULIST_2:
                  memberManagement.AlterMember();
                  break;
               // 회원 삭제
               case ConstNumber.MENULIST_3:
                  memberManagement.DeleteMember();
                  break;
               // 회원 목록(전체)
               case ConstNumber.MENULIST_4:
                  outputProcessor.PrintMemberList(DB);
                  break;
               case ConstNumber.MENULIST_5:
                  return;
            }
         }
      }

      // 관리자 책 관리 메뉴
      public static void ManageBooks(Library.BookManagement bookManagement, Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      { 
         while (true)
         {
            switch (outputProcessor.MenuScreen(ConsoleUI.BOOK_MENU))
            {
               // 서적 추가
               case ConstNumber.MENULIST_1:
                  bookManagement.AddBook();
                  break;
               // 서적 삭제
               case ConstNumber.MENULIST_2:
                  bookManagement.DeleteBook();
                  break;
               // 서적 수량 수정
               case ConstNumber.MENULIST_3:
                  bookManagement.AlterBookCount();
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
