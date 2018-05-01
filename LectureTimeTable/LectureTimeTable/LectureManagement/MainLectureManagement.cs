using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable.LectureManagement
{
   /// <summary>
   ///  수강 신청 및 관심과목 담기 등의 수강 메뉴가 실행되는 클래스
   /// </summary>
   class MainLectureManagement
   {
      // 로그인된 학생 클래스
      StudentManagement.Student student;
      // 모든 강의를 담고 있는 excelhandler
      HandleExcel.ExcelHandler allLectureData;
      // 학생이 수강신청한 강의를 담고 있는 excelhandler
      HandleExcel.ExcelHandler studentLectureData;
      IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();
      // 수강 관리 도구
      ManagementTool managementTool;

      // 생성자
      public MainLectureManagement(StudentManagement.Student student) {
         // 로그인된 학생의 계정
         this.student = student;
         Console.Clear();
         Console.WriteLine("\n Loading Excel Data");
         // 모든 강의를 담고 있는 excelhandler
         allLectureData = new HandleExcel.ExcelHandler("LectureSchedule2018-1.xlsx", "강의시간표(schedule)");
         allLectureData.SetDataRange("A1", "L167");
         managementTool = new ManagementTool(allLectureData, student);
         // 학생의 수강신청 엑셀을 생성 또는 로드한다.
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
               // 돌아가기 
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
