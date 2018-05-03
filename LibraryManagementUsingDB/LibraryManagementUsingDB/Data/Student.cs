using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Data
{
   class Student
   {
      private string studentNo;
      private string password;
      public string name;
      public string address;
      public string phoneNumber;

      // 계정 상태
      public int status;

      public string StudentNo
      {
         set
         {
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
         set
         {
            if (value == null)
               password = null;
            else if (value.Length <= 12)
               password = value;
            else
               Console.WriteLine("Erro : 올바르지 않은 비밀번호");
         }
         get { return password; }
      }
   }
}
