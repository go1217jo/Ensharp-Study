﻿using System;
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

      public string PrintLogInformation()
      {
         IOException.OutputProcessor outputProcessor = new IOException.OutputProcessor();
         return "  " + outputProcessor.PrintFixString(logTime, 22)
         + outputProcessor.PrintFixString(membername, 12)
         + outputProcessor.PrintFixString(keyword, 54)
         + outputProcessor.PrintFixString(type, 22);
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
