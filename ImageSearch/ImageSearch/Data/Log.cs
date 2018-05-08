using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Data
{
   public class Log
   {
      string logTime;
      string keyword;

      public string LogTime
      {
         get { return logTime; }
         set { logTime = value; }
      }

      public string Keyword
      {
         get { return keyword; }
         set { keyword = value; }
      }
   }
}
