using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
   class Outprocessor
   {
      public string InsertComma(string number)
      {
         int commaCount = number.Length / 3;
         int initCommaIndex = number.Length - commaCount * 3;

         if (initCommaIndex == 0)
            initCommaIndex += 3;

         string returnNumber = number.Substring(0, initCommaIndex);

         if (number.Length <= 3)
            return number;

         for(int i=initCommaIndex; i<number.Length;i+=3)
         {
            returnNumber += ',';
            returnNumber += number.Substring(i, 3);
         }

         return returnNumber;
      }
   }
}
