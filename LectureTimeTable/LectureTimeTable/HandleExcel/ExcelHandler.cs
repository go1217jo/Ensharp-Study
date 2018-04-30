using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace LectureTimeTable.HandleExcel
{
   class ExcelHandler
   {
      // Excel Application 객체
      Excel.Application ExcelApp;
      // 작업하는 워크시트
      Excel.Worksheet worksheet;
      // 현재 담고있는 데이터
      Array data;

      // Excel Load
      public ExcelHandler(string fileName, string sheetName)
      {
         try
         {
            //  Excel Application 객체 생성
            ExcelApp = new Excel.Application();

            // Workbook 객체 생성 및 파일 오픈
            Excel.Workbook workbook = ExcelApp.Workbooks.Open(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\"+ fileName);

            //sheets에 읽어온 엑셀값을 넣기
            Excel.Sheets sheets = workbook.Sheets;

            // 특정 sheet의 값 가져오기
            worksheet = sheets[sheetName] as Excel.Worksheet;
         }
         catch (SystemException e)
         {
            Console.WriteLine(e.Message);
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
         Excel.Range cellRange = worksheet.get_Range(from, to) as Excel.Range;
         // 설정한 범위만큼 데이터 담기
         data = cellRange.Cells.Value2;
      }

      // 열려있던 Excel 워크북과 앱을 나가기
      public void Close()
      {
         ExcelApp.Workbooks.Close();
         ExcelApp.Quit();
      }

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

      public int ReturnCredit(int record)
      {
         string obj = (string)data.GetValue(record, ConstNumber.CREDIT);
         return int.Parse(obj[0]+"");
      }

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
   }
}
