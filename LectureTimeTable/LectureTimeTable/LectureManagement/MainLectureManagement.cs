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
      HandleExcel.ExcelHandler studentLectureData;
      IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();
      ManagementTool managementTool;

      public MainLectureManagement(StudentManagement.Student student) {
         this.student = student;
         Console.Clear();
         Console.WriteLine("\n Loading Excel Data");
         allLectureData = new HandleExcel.ExcelHandler("LectureSchedule2018-1.xlsx", "강의시간표(schedule)");
         allLectureData.SetDataRange("A1", "L167");
         managementTool = new ManagementTool(allLectureData, student);
         studentLectureData = new HandleExcel.ExcelHandler(student.StudentNo+".xlsx", "sheet1");
         studentLectureData.SetDataRange("A1", "F25");
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
                  managementTool.LectureLookup(ConstNumber.FAKE_CHOICE);
                  break;
               // 담긴 관심과목 조회
               case ConstNumber.MENULIST_2:
                  managementTool.InterestedSubjectLookup(ConstNumber.FAKE_CHOICE);
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
                  managementTool.InterestedSubjectLookup(ConstNumber.REAL_CHOICE);
                  break;
               // 강의 조회
               case ConstNumber.MENULIST_2:
                  managementTool.LectureLookup(ConstNumber.REAL_CHOICE);
                  break;
               // 시간표 조회
               case ConstNumber.MENULIST_3:
                  outputProcessor.TimeTableScreen(allLectureData, student.timeTable);
                  break;
               // 뒤로 가기
               case ConstNumber.MENULIST_4:
                  studentLectureData.SaveTimeTable(allLectureData, student.timeTable);
                  return;
            }
         }
      }
   }
}
