using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Data
{
   class Log
   {
      private string logTime;
      private string membername;
      private string keyword;
      private string type;

      public string LogTime
      {
         get { return logTime; }
         set { logTime = value; }
      }

      public string Membername
      {
         get { return membername; }
         set { membername = value; }
      }

      public string Keyword
      {
         get { return keyword; }
         set { keyword = value; }
      }

      public string Type
      {
         get { return type; }
         set { type = value; }
      }

   }
}
