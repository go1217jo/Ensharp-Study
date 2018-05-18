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
         int commaCount, initCommaIndex;
         bool negative = false;
         string returnNumber;

         // 음수가 붙어있을 경우
         if (number.Length != 0 && number[0] == '-')
         {
            negative = true;
            number = number.Substring(1);
         }

         // 숫자가 세 글자 이하면 나감
         if(number.Length <= 3)
         {
            if (negative)
               return '-' + number;
            else
               return number;
         }
         
         commaCount = number.Length / 3;

         initCommaIndex = number.Length - commaCount * 3;
         if (initCommaIndex == 0)
            initCommaIndex += 3;
         returnNumber = number.Substring(0, initCommaIndex);
         
         for (int i=initCommaIndex; i<number.Length;i+=3)
         {
            returnNumber += ',';
            returnNumber += number.Substring(i, 3);
         }
         if (negative)
            returnNumber = '-' + returnNumber;

         return returnNumber;
      }

      // 인수로 들어온 문자열에 대해 comma를 제거하여 반환함
      public string DeleteComma(string number)
      {
         string returnNumber = "";
         string[] splits = number.Split(',');
         for (int idx = 0; idx < splits.Length; idx++)
            returnNumber += splits[idx];
         return returnNumber;
      }
   }
}
