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

        public MainLectureManagement(StudentManagement.Student student) {
            this.student = student;
            Console.Clear();
            Console.WriteLine("\n Loading Excel Data");
            allLectureData = new HandleExcel.ExcelHandler("LectureSchedule2018-1.xlsx", "강의시간표(schedule)");
            allLectureData.SetDataRange("A1", "L167");
            // 메뉴 루프 진입
            LectureMenu();
        }

        public void LoadSubmittedLecture()
        {

        }
        public void LectureMenu() {
            while (true)
            {
                switch (outputProcessor.MainMenuScreen())
                {
                    case ConstNumber.MENULIST_1:
                        ContainInterestedLecture();
                        break;
                    case ConstNumber.MENULIST_2:
                        ApplyLecture();
                        break;
                    case ConstNumber.MENULIST_3:
                        return;
                    default:
                        break;
                }
            }
        }

        public void ContainInterestedLecture() { }

        public void ApplyLecture() {
            switch(outputProcessor.SearchMenuScreen())
            {
                case ConstNumber.MENULIST_1:
                    break;
                case ConstNumber.MENULIST_2:
                    LectureLookup();
                    break;
                case ConstNumber.MENULIST_3:
                    break;
                case ConstNumber.MENULIST_4:
                    return;
            }
        }

        public void LectureLookup()
        {
            // 검색 조건 입력 받음
            string[] conditions = outputProcessor.LectureListHeaderScreen();
            Console.SetCursorPosition(0, 4);
            // 검색 조건에 맞는 강의 목록 출력
            outputProcessor.LectureListScreen(allLectureData, conditions);
        }
    }
}
