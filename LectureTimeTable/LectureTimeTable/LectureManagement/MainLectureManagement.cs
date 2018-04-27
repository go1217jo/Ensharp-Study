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
        IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();

        public MainLectureManagement(StudentManagement.Student student) {
            this.student = student;
            LectureMenu();
        }
        public void LectureMenu() {
            switch (outputProcessor.MainMenuScreen()) {
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
            outputProcessor.LectureListHeader();
            
        }
    }
}
