using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Data
{
   class LogManagement
   {
      DBHandler DB;
      IOException.OutputProcessor outputProcessor;

      public LogManagement(DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.DB = DB;
         this.outputProcessor = outputProcessor;
      }

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
               // 로그 내보내기
               case ConstNumber.MENULIST_2:
                  ExportLog();
                  break;
               // 로그 초기화
               case ConstNumber.MENULIST_3:
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
               case ConstNumber.MENULIST_4:
                  return;
            }
         }
      }     

      public void ExportLog()
      {
         List<Data.Log> logs = DB.ViewAllLog();
         using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\log.txt", false))
         {
            file.WriteLine(" ============================================================================================================");
            file.WriteLine("          발생시간          실행자                          키워드                                로그 유형");
            file.WriteLine(" ============================================================================================================");
            for (int i = 0; i < logs.Count; i++)
               file.WriteLine(logs[i].PrintLogInformation());
         }
         outputProcessor.PressAnyKey("로그가 성공적으로 저장되었습니다.");
      }
   }
}
