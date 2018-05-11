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

      public void PrintLogInformation()
      {
         IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();
         Console.Write("  ");
         Console.Write(outputProcessor.PrintFixString(logTime, 22));
         Console.Write(outputProcessor.PrintFixString(membername, 12));
         Console.Write(outputProcessor.PrintFixString(keyword, 54));
         Console.WriteLine(outputProcessor.PrintFixString(type, 22));
      }

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
