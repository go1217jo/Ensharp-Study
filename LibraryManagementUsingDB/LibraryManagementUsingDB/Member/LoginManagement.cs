using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LibraryManagementUsingDB.Member
{
   /// <summary>
   ///   로그인 관리 클래스
   /// </summary>
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

      // 유저로 로그인하는건지 확인
      public bool CheckUserLogin(Data.Student student)
      {
         sqlQuery = "SELECT MEMBERNAME FROM MEMBER WHERE studentno = '" + student.StudentNo + "' AND password = '" + student.Password + "';";
         MySqlDataReader reader = DB.SelectQuery(sqlQuery);
         bool success =  DB.IsThereOneValue(reader, "membername");
         reader.Close();
         return success;
      }

      // 관리자로 로그인하는건지 확인      
      public bool CheckAdminLogin(Data.Student student)
      {
         sqlQuery = "SELECT ID FROM admin WHERE ID = '" + student.StudentNo + "' AND password = '" + student.Password + "';";
         MySqlDataReader reader = DB.SelectQuery(sqlQuery);
         bool success = DB.IsThereOneValue(reader, "ID");
         reader.Close();
         return success;
      }
            
      // 로그인 화면을 출력하고 로그인 정보를 입력 받는다
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
                  Menu.AdminScreen(DB, outputProcessor);
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
      
      // 로그인 성공 시 입력 받은 로그인 정보를 전달한다.
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
               student.name = "관리자";
               student.status = ConstNumber.LOGIN_ADMIN;
            }
            // 둘 다 아니면 로그인 실패
            else
               student.status = ConstNumber.LOGIN_FAIL;
         }
         
         return student;
      }
   }
}
