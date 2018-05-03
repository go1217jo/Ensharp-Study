using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementUsingDB.Member
{
   class MemberManagement
   {
      IOException.OutputProcessor output;
      Data.DBHandler DB;

      public MemberManagement(IOException.OutputProcessor outputProcessor, Data.DBHandler DB)
      {
         output = outputProcessor;
         this.DB = DB;
      }

      internal void ManageMember()
      {
         throw new NotImplementedException();
      }

      // public void ExecuteByChoice()
   }
}
