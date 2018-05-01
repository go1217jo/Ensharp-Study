using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable.LectureManagement
{
   class ManagementTool
   {
      IOException.OutputProcessor outputProcessor;
      HandleExcel.ExcelHandler allLectureData;
      StudentManagement.Student student;

      public ManagementTool(HandleExcel.ExcelHandler lectureData, StudentManagement.Student student)
      {
         outputProcessor = new IOException.OutputProcessor();
         allLectureData = lectureData;
         this.student = student;
      }

      // 강의 조회 화면, mode 매개변수를 통해 수강 신청, 관심 과목 담기 등에서 쓰일 수 있음
      public void LectureLookup(int mode)
      {
         int choice = ConstNumber.ESC;
         // 검색된 튜플 인덱스를 담는 리스트
         List<int> searchedTuple;

         while (true)
         {
            // 검색 조건 입력 받음
            string[] conditions = outputProcessor.LectureListHeaderScreen();
            // 검색 도중 중지되었다면 나감
            if (conditions == null)
               return;
            Console.SetCursorPosition(0, 4);

            // 검색 조건에 맞는 강의 목록 출력
            // 수강 신청에서의 강의 조회
            if (mode == ConstNumber.REAL_CHOICE)
               searchedTuple = outputProcessor.LectureListScreen(allLectureData, conditions, student.appliedSubjects, 167);
            // 관심과목 담기에서의 강의 조회
            else
               searchedTuple = outputProcessor.LectureListScreen(allLectureData, conditions, student.containedSubjects, 167);

            // 검색된 강의가 없다면
            if (searchedTuple.Count == 0)
               outputProcessor.PressAnyKey("검색된 강의가 없습니다.");
            else
               // 사용자에 의한 강의 선택
               choice = outputProcessor.LectureChoice(searchedTuple.Count);

            if (choice == ConstNumber.ESC)
               return;
            else
            {
               // 현재 선택된 강의의 학점
               int presentCredit = allLectureData.ReturnCredit(searchedTuple[choice]);
               // 수강신청, 관심과목 담기를 달리 함
               // 수강 신청
               if (mode == ConstNumber.REAL_CHOICE)
               {
                  if (outputProcessor.YesOrNo("선택한 강의를 신청하시겠습니까?") == 1)
                  {  
                     // 동일과목이 이미 수강신청되어 있는지 학수번호를 확인한다
                     if (CheckLectureNo(searchedTuple[choice]))
                     {
                        // 신청학점이 최대학점을 초과하지 않는지 확인한다
                        if ((student.appliedCredit + presentCredit) <= ConstNumber.MAX_APPLIED)
                        {
                           // 신청 강의가 이미 신청된 강의들과 시간이 겹치지 않는지 확인하고 그렇지 않다면 추가한다
                           if (AddTimeTable(allLectureData, student.timeTable, searchedTuple[choice]))
                              student.AddAppliedSubject(searchedTuple[choice], presentCredit);
                           else
                              outputProcessor.PressAnyKey("다른 강의와 시간이 겹칩니다.");
                        }
                        else
                           outputProcessor.PressAnyKey("최대 신청 가능 학점을 초과하였습니다.");
                     }
                     else
                        outputProcessor.PressAnyKey("이미 동일 과목이 신청되었습니다.");
                  }
               }
               // 관심과목 담기
               else
               {
                  if (outputProcessor.YesOrNo("선택한 강의를 담으시겠습니까?") == 1)
                  {
                     // 관심과목 담기 학점이 최대 학점을 초과하지 않는지 확인한다
                     if ((student.containedCredit + presentCredit) <= ConstNumber.MAX_INTERESTED)
                        student.AddContainedSubject(searchedTuple[choice], presentCredit);
                     else
                        outputProcessor.PressAnyKey("최대 담을 수 있는 학점을 초과하였습니다.");
                  }
               }
               if (searchedTuple.Count == 0)
                  return;
            }
         }
      }

      // 수강신청-관심과목 검색
      public void InterestedSubjectLookup(int mode)
      {
         int choice = ConstNumber.ESC;
         int screenHeight = 30;
         // 담긴 관심과목이 없다면
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
               // 담긴 과목 리스트 출력
               outputProcessor.PrintLectureList(allLectureData, student.containedSubjects);

               // 강의 선택
               choice = outputProcessor.LectureChoice(student.containedSubjects.Count);

               if (choice == ConstNumber.ESC)
                  return;
               // 선택한 과목의 학점
               int presentCredit = allLectureData.ReturnCredit(student.containedSubjects[choice]);

               // 관심과목으로 수강신청일 때
               if (mode == ConstNumber.REAL_CHOICE)
               { 
                  if (outputProcessor.YesOrNo("선택한 강의를 신청하시겠습니까?") == 1)
                  {
                     // 동일과목이 이미 수강신청되어 있는지 학수번호를 확인한다 
                     if (CheckLectureNo(student.containedSubjects[choice]))
                     {
                        // 신청학점이 최대학점을 초과하지 않는지 확인한다
                        if ((student.appliedCredit + presentCredit) <= ConstNumber.MAX_APPLIED)
                        {
                           // 신청 강의가 이미 신청된 강의들과 시간이 겹치지 않는지 확인하고 그렇지 않다면 추가한다
                           if (AddTimeTable(allLectureData, student.timeTable, student.containedSubjects[choice]))
                           {
                              student.AddAppliedSubject(student.containedSubjects[choice], presentCredit);
                              // 관심과목에서 삭제
                              student.DeleteContainedSubject(student.containedSubjects[choice], presentCredit);
                           }
                           else
                              outputProcessor.PressAnyKey("다른 강의와 시간이 겹칩니다.");
                        }
                        else
                           outputProcessor.PressAnyKey("최대 신청 가능 학점을 초과하였습니다.");
                     }
                     else
                        outputProcessor.PressAnyKey("이미 동일 과목이 신청되었습니다.");
                  }
                  // 더 이상 담긴 관심과목이 없다면
                  if (student.containedSubjects.Count == 0)
                     return;
               }
               else
               {
                  if (outputProcessor.YesOrNo("관심과목에서 삭제하시겠습니까?") == 1)
                  {
                     // 선택된 관심과목을 삭제
                     student.DeleteContainedSubject(student.containedSubjects[choice], presentCredit);
                     // 더 이상 담긴 관심과목이 없다면
                     if (student.containedSubjects.Count == 0)
                        return;
                  }
               }
            }
         }
      }

      // 현재 학생이 수강신청한 과목 중 학수번호가 같은 과목이 있는지 확인한다
      public bool CheckLectureNo(int record)
      {
         string addLectureNo = allLectureData.ReturnSubjectNo(record);

         for(int i=0; i < student.appliedSubjects.Count; i++)
         {
            // 학수번호가 동일한 과목이라면
            if (addLectureNo.Equals(allLectureData.ReturnSubjectNo(student.appliedSubjects[i])))
               return false;
         }
         return true;
      }

      // 요일을 정의된 요일 상수로 반환
      public int ConvertCharToIntAtDay(char day)
      {
         switch(day)
         {
            case '월': return ConstNumber.MONDAY;
            case '화': return ConstNumber.TUESDAY;
            case '수': return ConstNumber.WENSDAY;
            case '목': return ConstNumber.THURSDAY;
            case '금': return ConstNumber.FRIDAY;
            default:
               return -1;
         }
      }

      // 문자열 시간을 지정된 시간표 인덱스로 반환
      public int ConvertTimeToIndex(string lectureTime)
      {
         string[] hourMinute = lectureTime.Split(':');
         int hour = int.Parse(hourMinute[0]);
         return (hour - 9) * 2 + (hourMinute[1].Equals("30") ? 1 : 0);
      }

      // 학생 시간표에 강의를 추가함
      public bool AddTimeTable(HandleExcel.ExcelHandler excelHandler, List<List<int>> timetable, int record)
      {
         string time = excelHandler.ReturnTime(record);
         // 실습수업이 있으면 길이가 2, 아니면 1
         string[] subTime = time.Split(',');
         int dayInt = 0, startTime = 0, endTime = 0 ;
         List<char> day = null;
         bool overlap = false;

         // 수업시간, 실습시간
         for (int i = 0; i < subTime.Length; i++)
         {
            day = new List<char>(new char[] { subTime[i][0], subTime[i][1] });
            // 요일이 하나이면
            if (day[1] >= '0' && day[1] <= '9')
               day.Remove(day[1]);
            // 시작 시간, 끝나는 시간
            string[] lectureTime = subTime[i].Substring(day.Count).Split('-');
            startTime = ConvertTimeToIndex(lectureTime[0]);
            endTime = ConvertTimeToIndex(lectureTime[1]) - 1;
            for (int j = 0; j < day.Count; j++) {
               dayInt = ConvertCharToIntAtDay(day[j]);
               
               for (int idx = startTime; idx <= endTime; idx++)
                  if (timetable[dayInt][idx] != 0) {
                     overlap = true;
                     break;
                  }
            }            
         }
         // 시간이 중복되는 신청이 아니라면
         if (!overlap)
         {
            // 시간표에 강의 시간 표시
            for (int j = 0; j < day.Count; j++)
            {
               dayInt = ConvertCharToIntAtDay(day[j]);
               for (int idx = startTime; idx <= endTime; idx++)
                  timetable[dayInt][idx] = record;
            }
            return true;
         }
         else
            return false;
      }
   }
}
