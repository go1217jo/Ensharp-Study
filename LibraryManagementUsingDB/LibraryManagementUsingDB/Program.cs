using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// 제작자 : 주영준
/// 
///  Admin ID : 12345678, Password : 1234
///  member ID : 14010994, Password : 12345678
/// </summary>

namespace LibraryManagementUsingDB
{
   class Program
   {
      static void Main(string[] args)
      {
         Data.DBHandler DB = new Data.DBHandler();
         Member.LoginManagement loginManagement = new Member.LoginManagement(DB);

         loginManagement.Login();         
      }
   }
}
