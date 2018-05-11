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
                  if(outputProcessor.YesOrNo("로그를 초기화하시겠습니까?") == 1)
                     DB.ClearTable();
                  break;
               // 돌아 가기
               case ConstNumber.MENULIST_4:
                  return;
            }
         }
      }     

      public void ExportLog()
      {

      }
   }
}
