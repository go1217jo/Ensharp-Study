using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable.StudentManagement
{
   class Student
   {
      private string studentNo;
      private string password;
      public int appliedCredit = 0;
      public int containedCredit = 0;

      public List<int> appliedSubjects;
      public List<int> containedSubjects;

      public List<List<int>> timeTable = new List<List<int>>();

      public Student() {
         appliedSubjects = new List<int>();
         containedSubjects = new List<int>();

         // timetable 초기화
         for (int day = ConstNumber.MONDAY; day <= ConstNumber.FRIDAY; day++)
            timeTable.Add(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      }

      public Student(string studentNo, string password)
      {
         this.studentNo = studentNo;
         this.password = password;

         appliedSubjects = new List<int>();
         containedSubjects = new List<int>();

         // timetable 초기화
         for (int day = ConstNumber.MONDAY; day <= ConstNumber.FRIDAY; day++)
            timeTable.Add(new List<int>() {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 });
      }

      public void AddAppliedSubject(int record, int credit) {
         appliedSubjects.Add(record);
         appliedCredit += credit;
      }

      public void AddContainedSubject(int record, int credit) {
         containedSubjects.Add(record);
         containedCredit += credit;
      }

      public void DeleteAppliedSubject(int record, int credit)
      {
         appliedSubjects.Remove(record);
         appliedCredit -= credit;
      }

      public void DeleteContainedSubject(int record, int credit)
      {
         containedSubjects.Remove(record);
         containedCredit -= credit;
      }

      public string StudentNo
      {
         set {
            if (value == null)
               studentNo = null;
            else if (value.Length == 8)
               studentNo = value;
            else
               Console.WriteLine("Error : 올바르지 않은 학번");
         }
            
         get { return studentNo; }
      }

      public string Password
      {
         set {
            if (value == null)
               password = null;
            else if (value.Length <= 12)
               password = value;
            else
               Console.WriteLine("Erro : 올바르지 않은 비밀번호");
         }
         get { return password; }
      }

      // 인수로 주어진 학생목록에서 일치하는 학생이 있으면 true를 반환한다.
      public bool CheckLoginSuccess(List<Student> students)
      {
         for(int iter = 0; iter < students.Count; iter++) {
            // 학번 확인
            if(students[iter].StudentNo.Equals(studentNo))
            {
               // 패스워드 확인
               if (students[iter].Password.Equals(password))
                  return true;
            }
         }
         return false;
      }
   }
}
