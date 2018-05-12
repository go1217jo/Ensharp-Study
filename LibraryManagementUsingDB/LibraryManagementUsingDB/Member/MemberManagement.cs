using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Member
{
   /// <summary>
   ///  멤버 관리 클래스
   /// </summary>
   class MemberManagement
   {
      IOException.OutputProcessor outputProcessor;
      Data.DBHandler DB;

      public MemberManagement(Data.DBHandler DB, IOException.OutputProcessor outputProcessor)
      {
         this.outputProcessor = outputProcessor;
         this.DB = DB;
      }

      // 멤버 추가
      public void AddMember()
      {
         Data.Student student;
         ConsoleUI.PrintRegistration();
         student = outputProcessor.MemberRegistrationScreen();
         if (student == null)
            return;
         if (DB.InsertMember(student))
         {
            outputProcessor.PressAnyKey(student.name + " 회원님이 등록되었습니다.");
            // 로그 기록
            DB.InsertLog("관리자", student.StudentNo, "회원 추가");
         }
         else
            outputProcessor.PressAnyKey("회원 등록 실패");
      }

      // 멤버 수정
      public void AlterMember()
      {
         string modification = null;
         string attribute = null;
         string studentNo = outputProcessor.PrintMemberList(DB);
         if (studentNo == null)
            return;

         switch (outputProcessor.MenuScreen(ConsoleUI.MEMBER_MODIFY))
         {
            // 멤버 이름 수정
            case ConstNumber.MENULIST_1:
               modification = outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_NAME);
               attribute = "membername";
               break;
            // 멤버 주소 수정
            case ConstNumber.MENULIST_2:
               modification = outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_ADDRESS);
               attribute = "address";
               break;
            // 멤버 전화번호 수정
            case ConstNumber.MENULIST_3:
               modification = outputProcessor.AlterMemberInformation(studentNo, ConstNumber.MEMBER_PHONENUMBER);
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
         else
            // 로그 기록
            DB.InsertLog("관리자", studentNo, "회원 수정");

      }

      // 멤버 삭제
      public void DeleteMember()
      {
         Console.Clear();
         Console.WriteLine("\n   삭제할 멤버의 학번을 입력하세요.");
         Console.Write("   → ");
         string studentNo = outputProcessor.inputProcessor.InputStudentNoFormat(Console.CursorLeft);
         if (studentNo == null)
            return;
         if (outputProcessor.YesOrNo("해당 멤버를 정말 삭제하시겠습니까?") == 1)
         {
            if (!DB.DeleteMember(studentNo))
               outputProcessor.PressAnyKey("삭제할 멤버가 존재하지 않습니다.");
            else
               // 로그 기록
               DB.InsertLog("관리자", studentNo, "회원 삭제");
         }
      }

      // 관리자 멤버 관리 메뉴
      public void ManageMember()
      {
         while (true)
         {
            switch (outputProcessor.MenuScreen(ConsoleUI.MEMBER_MENU))
            {
               // 회원 추가
               case ConstNumber.MENULIST_1:
                  AddMember();
                  break;
               // 회원 정보 수정
               case ConstNumber.MENULIST_2:
                  AlterMember();
                  break;
               // 회원 삭제
               case ConstNumber.MENULIST_3:
                  DeleteMember();
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

   }
}
