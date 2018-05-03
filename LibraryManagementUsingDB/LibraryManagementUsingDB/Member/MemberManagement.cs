using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Member
{
   class MemberManagement
   {
      IOException.OutputProcessor outputProcessor;
      Data.DBHandler DB;

      public MemberManagement(Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.outputProcessor = outputProcessor;
         this.DB = DB;
      }

      public void AddMember()
      {
         Data.Student student;
         ConsoleUI.PrintRegistration();
         student = outputProcessor.RegistrationScreen();
         if (student == null)
            return;
         if (DB.InsertMember(student))
            outputProcessor.PressAnyKey(student.name + " 회원님이 등록되었습니다.");
         else
            outputProcessor.PressAnyKey("회원 등록 실패");
      }

      public void ManageMember()
      {
         while (true)
         {
            switch (outputProcessor.MenuScreen(ConsoleUI.MEMBER_MENU))
            {
               case ConstNumber.MENULIST_1:
                  AddMember();
                  break;
               case ConstNumber.MENULIST_2:
                  break;
               case ConstNumber.MENULIST_3:
                  break;
               case ConstNumber.MENULIST_4:
                  break;
               case ConstNumber.MENULIST_5:
                  return;
            }
         }
      }

   }
}
