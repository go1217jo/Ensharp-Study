using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace LectureTimeTable.HandleExcel
{
   /// <summary>
   ///  Excel에 대한 처리를 하는 클래스
   /// </summary>
   class ExcelHandler
   {
      // Excel Application 객체
      Excel.Application ExcelApp;
      Excel.Workbook workbook;
      // 작업하는 워크시트
      Excel.Worksheet worksheet;
      // 선택 셀 범위
      Excel.Range cellRange;
      // 현재 담고있는 데이터
      Array data;

      // Excel Load
      public ExcelHandler(string fileName, string sheetName)
      {
         //  Excel Application 객체 생성
         ExcelApp = new Excel.Application();

         if (new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + fileName).Exists)
         {
            // Workbook 객체 생성 및 파일 오픈
            workbook = ExcelApp.Workbooks.Open(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + fileName);
            //sheets에 읽어온 엑셀값을 넣기
            Excel.Sheets sheets = workbook.Sheets;

            // 특정 sheet의 값 가져오기
            worksheet = sheets[sheetName] as Excel.Worksheet;
         }
         else
         {
            // 새로운 excel file을 생성
            workbook = ExcelApp.Workbooks.Add(Type.Missing);
            worksheet = workbook.ActiveSheet;
            worksheet.Name = sheetName;
            workbook.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + fileName);
         }                
      }

      // 소멸자
      ~ExcelHandler()
      {
         Close();
      }

      public void SetDataRange(string from, string to)
      {
         // 범위 설정
         cellRange = worksheet.get_Range(from, to) as Excel.Range;
         // 설정한 범위만큼 데이터 담기
         data = cellRange.Cells.Value2;
      }

      // 열려있던 Excel 워크북과 앱을 나가기
      public void Close()
      {
         ExcelApp.Workbooks.Close();
         ExcelApp.Quit();

         Marshal.ReleaseComObject(cellRange);
         Marshal.ReleaseComObject(worksheet);
         Marshal.ReleaseComObject(workbook);
         Marshal.ReleaseComObject(ExcelApp);

      }

      // 현재 데이터를 문자열로 반환
      public string ToStringPresentData(int index1, int index2)
      {
         // 지정 인덱스의 데이터 반환
         // 학점은 double형이므로 따로 처리하여 준다
         if (index1 != 1 && ( index2 == ConstNumber.NO)) {
               object obj = data.GetValue(index1, index2);
               return (double)obj + "";
         }    
         else
               return (string)data.GetValue(index1, index2);
      }

      // 해당 레코드의 학점 반환
      public int ReturnCredit(int record)
      {
         string obj = (string)data.GetValue(record, ConstNumber.CREDIT);
         return int.Parse(obj[0]+"");
      }

      // 해당 레코드의 시간 문자열 반환
      public string ReturnTime(int record)
      {
         return (string)data.GetValue(record, ConstNumber.TIME);
      }

      // 해당 레코드의 과목명 반환
      public string ReturnSubject(int record)
      {
         return (string)data.GetValue(record, ConstNumber.SUBJECT_NAME);
      }

      // 해당 레코드의 학수번호 반환
      public string ReturnSubjectNo(int record)
      {
         return (string)data.GetValue(record, ConstNumber.SUBJECT_NO);
      }

      // conditions 조건에 맞게 검색한 결과를 레코드 리스트 형태로 반환
      public List<int> ReturnSearchResult(string[] conditions, List<int> appliedCredit, int dataCount)
      {
         List<int> chosenTuple = new List<int>();
         int attribute = 0;  // 속성 인덱스

         for(int idx = 2; idx <= dataCount; idx++) {
            bool contained = true;
            if (appliedCredit.Contains(idx))
            {
               contained = false;
               continue;
            }
            // 검색 조건을 만족하지 않는 튜플은 저장하지 않는다
            for (attribute = 0; attribute < 5; attribute++) {
               if (!conditions[attribute].Equals("전체")) {
                  switch(attribute)
                  {
                     case 0:
                        if (!ToStringPresentData(idx, ConstNumber.MAJOR).Contains(conditions[attribute]))
                           contained = false;
                        break;
                     case 1:
                        if (!ToStringPresentData(idx, ConstNumber.SUBJECT_NAME).Contains(conditions[attribute]))
                           contained = false;
                        break;
                     case 2:
                        if (!ToStringPresentData(idx, ConstNumber.SUBJECT_NO).Contains(conditions[attribute]))
                           contained = false;
                        break;
                     case 3:
                        if (!ToStringPresentData(idx, ConstNumber.GRADE).Contains(conditions[attribute]))
                           contained = false;
                        break;
                     case 4:
                        if (!ToStringPresentData(idx, ConstNumber.PROFESSOR_NAME).Contains(conditions[attribute]))
                           contained = false;
                        break;
                  }
               }
            }
            // 검색 결과에 부합하는 튜플 인덱스 저장
            if (contained)
               chosenTuple.Add(idx);
         }

         return chosenTuple;
      }

      // 엑셀로 시간표 저장
      public void SaveTimeTable(HandleExcel.ExcelHandler excelHandler, List<List<int>> timetable)
      {
         worksheet.Cells[1, 2 + ConstNumber.MONDAY] = "월요일";
         worksheet.Cells[1, 2 + ConstNumber.TUESDAY] = "화요일";
         worksheet.Cells[1, 2 + ConstNumber.WENSDAY] = "수요일";
         worksheet.Cells[1, 2 + ConstNumber.THURSDAY] = "목요일";
         worksheet.Cells[1, 2 + ConstNumber.FRIDAY] = "금요일";
         
         for (int i = 0; i < 12; i++)
         {
            worksheet.Cells[2+i*2, 1] = (9+i) + ":00-"+ (9+i) +":30";
            worksheet.Cells[3+i*2, 1] = (9 + i) + ":30-" + (10 + i) + ":00";
         }

         // 시간표 내용 저장
         for(int day=0;day<timetable.Count; day++)
         {
            for(int time=0; time<timetable[0].Count; time++)
            {
               if(timetable[day][time]!= 0)
                  worksheet.Cells[time + 2, day + 2] = excelHandler.ReturnSubject(timetable[day][time]);
            }
         }

         workbook.Save();
      }
   }
}
