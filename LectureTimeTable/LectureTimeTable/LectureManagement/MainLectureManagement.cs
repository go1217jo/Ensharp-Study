using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable.LectureManagement
{
   class MainLectureManagement
   {
      StudentManagement.Student student;
      HandleExcel.ExcelHandler allLectureData;
      IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();

      public MainLectureManagement(StudentManagement.Student student) {
         this.student = student;
         Console.Clear();
         Console.WriteLine("\n Loading Excel Data");
         allLectureData = new HandleExcel.ExcelHandler("LectureSchedule2018-1.xlsx", "강의시간표(schedule)");
         allLectureData.SetDataRange("A1", "L167");
         // 메뉴 루프 진입
         LectureMenu();
      }

      // 초기 메뉴 선택
      public void LectureMenu() {
         while (true)
         {
            switch (outputProcessor.MainMenuScreen())
            {
               // 관심과목 담기
               case ConstNumber.MENULIST_1:
                  ContainInterestedLecture();
                  break;
               // 수강신청
               case ConstNumber.MENULIST_2:
                  ApplyLecture();
                  break;
               // 뒤로 가기
               case ConstNumber.MENULIST_3:
                  allLectureData.Close();
                  return;
            }
         }
      }

      // 메인-관심과목 담기 메뉴 선택
      public void ContainInterestedLecture()
      {
         while (true)
         {
            switch (outputProcessor.InterestedSubjectMenuScreen())
            {
               // 강의 조회
               case ConstNumber.MENULIST_1:
                  LectureLookup(ConstNumber.FAKE_CHOICE);
                  break;
               // 담긴 관심과목 조회
               case ConstNumber.MENULIST_2:
                  InterestedSubjectLookup(ConstNumber.FAKE_CHOICE);
                  break;
               case ConstNumber.MENULIST_3:
                  return;
            }
         }
      }

      // 메인-수강신청 메뉴 선택
      public void ApplyLecture() {
         while (true)
         {
            switch (outputProcessor.SearchMenuScreen())
            {
               // 관심과목 검색
               case ConstNumber.MENULIST_1:
                  InterestedSubjectLookup(ConstNumber.REAL_CHOICE);
                  break;
               // 강의 조회
               case ConstNumber.MENULIST_2:
                  LectureLookup(ConstNumber.REAL_CHOICE);
                  break;
               // 시간표 조회
               case ConstNumber.MENULIST_3:
                  break;
               // 뒤로 가기
               case ConstNumber.MENULIST_4:
                  return;
            }
         }
      }
      
      // 수강신청-관심과목 검색
      public void InterestedSubjectLookup(int mode)
      {
         int choice = ConstNumber.ESC;
         int screenHeight = 30;
         
         if (student.containedSubjects.Count == 0)
         {
            outputProcessor.PressAnyKey("담겨진 관심과목이 없습니다.");
            return;
         }
         else
         {
            while (true)
            {
               Console.Clear();
               Console.Write("\n\n\n\n");
               Console.SetWindowSize(183, screenHeight);
               outputProcessor.PrintLectureList(allLectureData, student.containedSubjects);

               // 관심과목으로 수강신청일 때
               if (mode == ConstNumber.REAL_CHOICE)
               {
                  // 강의 선택
                  choice = outputProcessor.LectureChoice(student.containedSubjects.Count);

                  if (choice == ConstNumber.ESC)
                     return;

                  int presentCredit = allLectureData.ReturnCredit(student.containedSubjects[choice]);

                  if (outputProcessor.YesOrNo("선택한 강의를 담으시겠습니까?") == 1)
                  {
                     if ((student.appliedCredit + presentCredit) <= ConstNumber.MAX_APPLIED)
                     {
                        student.AddAppliedSubject(student.containedSubjects[choice], presentCredit);
                        // 관심과목에서 삭제
                        student.DeleteContainedSubject(student.containedSubjects[choice], presentCredit);
                     }
                     else
                        outputProcessor.PressAnyKey("최대 담을 수 있는 학점을 초과하였습니다.");
                  }
               }
               if (choice == ConstNumber.ESC)
               {
                  Console.ReadKey();
                  return;
               }
            }
         }
      }

      // 강의 조회 화면
      public void LectureLookup(int mode)
      {
         int choice = ConstNumber.ESC;
         List<int> searchedTuple;

         while (true)
         {
            // 검색 조건 입력 받음
            string[] conditions = outputProcessor.LectureListHeaderScreen();
            if (conditions == null)
               return;
            Console.SetCursorPosition(0, 4);

            // 검색 조건에 맞는 강의 목록 출력
            if(mode == ConstNumber.REAL_CHOICE)
               searchedTuple = outputProcessor.LectureListScreen(allLectureData, conditions, student.appliedSubjects, 167);
            else
               searchedTuple = outputProcessor.LectureListScreen(allLectureData, conditions, student.containedSubjects, 167);

            if (searchedTuple.Count == 0)
               outputProcessor.PressAnyKey("검색된 강의가 없습니다.");
            else 
               // 강의 선택
               choice = outputProcessor.LectureChoice(searchedTuple.Count);

            if (choice == ConstNumber.ESC)
               return;
            else
            {
               int presentCredit = allLectureData.ReturnCredit(searchedTuple[choice]);
               // 수강신청, 관심과목 담기를 달리 함
               if (mode == ConstNumber.REAL_CHOICE)
               {
                  if (outputProcessor.YesOrNo("선택한 강의를 신청하시겠습니까?") == 1)
                  {
                     if ((student.appliedCredit + presentCredit) <= ConstNumber.MAX_APPLIED)
                        student.AddAppliedSubject(searchedTuple[choice], presentCredit);
                     else
                        outputProcessor.PressAnyKey("최대 신청 가능 학점을 초과하였습니다.");
                  }
               }
               else
               {
                  if (outputProcessor.YesOrNo("선택한 강의를 담으시겠습니까?") == 1)
                  {
                     if ((student.containedCredit + presentCredit) <= ConstNumber.MAX_INTERESTED)
                        student.AddContainedSubject(searchedTuple[choice], presentCredit);
                     else
                        outputProcessor.PressAnyKey("최대 담을 수 있는 학점을 초과하였습니다.");
                  }
               }
            }
         }
      }
   }
}
