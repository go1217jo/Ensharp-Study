using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Data
{
   /// <summary>
   ///  로그를 관리하는 클래스
   /// </summary>
   class LogManagement
   {
      DBHandler DB;
      IOException.OutputProcessor outputProcessor;

      public LogManagement(DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.DB = DB;
         this.outputProcessor = outputProcessor;
      }

      // 로그 관리 메뉴
      public void ManageLog()
      {
         while (true)
         {
            switch (outputProcessor.MenuScreen(ConsoleUI.LOG_MENU))
            {
               // 로그 보기
               case ConstNumber.MENULIST_1:
                  outputProcessor.ViewAllLogs(DB);
                  break;
               // 로그 초기화
               case ConstNumber.MENULIST_2:
                  if (outputProcessor.YesOrNo("로그를 초기화하시겠습니까?") == 1)
                  {
                     // 로그 테이블 초기화
                     DB.ClearTable();
                     // 이전 로그 파일이 존재하면 삭제
                     if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\log.txt"))
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\log.txt");
                  }
                  break;
               // 돌아 가기
               case ConstNumber.MENULIST_3:
                  return;
            }
         }
      }     
   }
}
