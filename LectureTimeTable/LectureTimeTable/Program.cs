using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Student = LectureTimeTable.StudentManagement.Student;

namespace LectureTimeTable
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice = 0;
            StudentManagement.Student student;

            // 학생 목록
            List<StudentManagement.Student> students = new List<StudentManagement.Student>();
            // 학생 추가
            students.Add(new Student("14010994", "12345678"));
            students.Add(new Student("15011987", "12345678"));

            // 출력 예외처리 객체 생성
            IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();

            while (true) {
                // 학생 로그인
                student = outputProcessor.LoginScreen();
                if (student.CheckLoginSuccess(students)) {
                    new LectureManagement.MainLectureManagement(student);
                }
                else
                    outputProcessor.PressAnyKey("로그인에 실패하셨습니다.");
                    
            }

            
        }
    }
}
