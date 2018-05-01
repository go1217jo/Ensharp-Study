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
            if (mode == ConstNumber.REAL_CHOICE)
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
                     if (CheckLectureNo(searchedTuple[choice]))
                     {
                        if ((student.appliedCredit + presentCredit) <= ConstNumber.MAX_APPLIED)
                        {
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

                  if (outputProcessor.YesOrNo("선택한 강의를 신청하시겠습니까?") == 1)
                  {
                     if (CheckLectureNo(student.containedSubjects[choice]))
                     {
                        if ((student.appliedCredit + presentCredit) <= ConstNumber.MAX_APPLIED)
                        {
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
                  if (student.containedSubjects.Count == 0)
                     return;
               }
               if (choice == ConstNumber.ESC)
               {
                  Console.ReadKey();
                  return;
               }
            }
         }
      }

      public bool CheckLectureNo(int record)
      {
         string addLectureNo = allLectureData.ReturnSubjectNo(record);

         for(int i=0; i < student.appliedSubjects.Count; i++)
         {
            if (addLectureNo.Equals(allLectureData.ReturnSubjectNo(student.appliedSubjects[i])))
               return false;
         }
         return true;
      }

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

      public int ConvertTimeToIndex(string lectureTime)
      {
         string[] hourMinute = lectureTime.Split(':');
         int hour = int.Parse(hourMinute[0]);
         return (hour - 9) * 2 + (hourMinute[1].Equals("30") ? 1 : 0);
      }

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
