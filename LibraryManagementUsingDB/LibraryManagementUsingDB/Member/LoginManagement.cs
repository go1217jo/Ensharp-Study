using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LibraryManagementUsingDB.Member
{
   class LoginManagement
   {
      Data.DBHandler DB;
      // sql 질의문
      string sqlQuery;
      IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();

      public LoginManagement(Data.DBHandler dbHandler)
      {
         DB = dbHandler;
      }
      public bool CheckUserLogin(Data.Student student)
      {
         sqlQuery = "SELECT MEMBERNAME FROM MEMBER WHERE studentno = '" + student.StudentNo + "' AND password = '" + student.Password + "';";
         MySqlDataReader reader = DB.SelectQuery(sqlQuery);
         bool success =  DB.IsThereOneValue(reader, "membername");
         reader.Close();
         return success;
      }
      
      public bool CheckAdminLogin(Data.Student student)
      {
         sqlQuery = "SELECT ID FROM admin WHERE ID = '" + student.StudentNo + "' AND password = '" + student.Password + "';";
         MySqlDataReader reader = DB.SelectQuery(sqlQuery);
         bool success = DB.IsThereOneValue(reader, "ID");
         reader.Close();
         return success;
      }

      public void AdminScreen()
      {
         MemberManagement memberManagement = new MemberManagement(DB, outputProcessor);
         Library.BookManagement bookManagement = new Library.BookManagement(DB, outputProcessor);
         while (true)
         {
            switch(outputProcessor.MenuScreen(ConsoleUI.ADMIN_MENU))
            {
               case ConstNumber.MENULIST_1:
                  memberManagement.ManageMember();
                  break;
               case ConstNumber.MENULIST_2:
                  bookManagement.ManageBooks();
                  break;
               case ConstNumber.MENULIST_3:
                  return;
            }
         }
      }
      
      public void Login()
      {
         // 유저 또는 관리자 로그인
         while (true)
         {
            Data.Student student = LoginProcess();
            if (student == null)
            {
               Console.Clear();
               return;
            }

            switch (student.status)
            {
               case ConstNumber.LOGIN_ADMIN:
                  AdminScreen();
                  break;
               case ConstNumber.LOGIN_USER:
                  new Library.BookManagement(student, DB, outputProcessor).UserRentalSystem();
                  break;
               case ConstNumber.LOGIN_FAIL:
                  outputProcessor.PressAnyKey("로그인 실패");
                  break;
            }
         }
      }
      
      // 0 반환 : 로그인 실패, 1 반환 : 유저 로그인, 2 반환 : 관리자 로그인
      public Data.Student LoginProcess()
      {
         // 로그인 화면 출력
         Data.Student student = outputProcessor.LoginScreen();
         if (student == null)
            return null;

         // 유저 로그인 확인
         if (CheckUserLogin(student))
         {
            outputProcessor.PressAnyKey(student.StudentNo + "님 환영합니다.");
            student.status = ConstNumber.LOGIN_USER;
            DB.LoadMemberInformation(student);
         }
         else
         {
            // 유저가 아니면 관리자 로그인인지 확인
            if (CheckAdminLogin(student))
            {
               outputProcessor.PressAnyKey("관리자님 환영합니다.");
               student.status = ConstNumber.LOGIN_ADMIN;
            }
            // 둘 다 아니면 로그인 실패
            else
            {
               student.status = ConstNumber.LOGIN_FAIL;
            }
         }
         
         return student;
      }
   }
}
