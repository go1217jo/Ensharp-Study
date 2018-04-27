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

        public void PrintPresentData(int index1, int index2)
        {
            // 데이터 출력
            Console.WriteLine(data.GetValue(index1, index2));
        }
    }
}
