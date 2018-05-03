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

      public void AlterMember()
      {
         string modification = null;
         string attribute = null;
         string studentNo = "14010994";

         switch (outputProcessor.MenuScreen(ConsoleUI.MEMBER_MODIFY))
         {
            // 멤버 이름 수정
            case ConstNumber.MENULIST_1:
               modification = outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_NAME);
               attribute = "membername";
               break;
            // 멤버 주소 수정
            case ConstNumber.MENULIST_2:
               outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_ADDRESS);
               attribute = "address";
               break;
            // 멤버 전화번호 수정
            case ConstNumber.MENULIST_3:
               outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_PHONENUMBER);
               attribute = "phonenumber";
               break;
            case ConstNumber.MENULIST_4:
               return;
         }
         if (modification == null)
            return;

         // DB에서 변경
         if (!DB.UpdateMemberInformation(studentNo, modification, attribute))
            outputProcessor.PressAnyKey("회원 수정 실패 : 중복 학번");
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
                  AlterMember();
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
