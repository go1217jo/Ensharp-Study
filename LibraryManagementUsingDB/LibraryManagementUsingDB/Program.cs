using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
