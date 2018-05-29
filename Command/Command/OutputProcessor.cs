using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Command
{
    /// <summary>
    ///  출력에 대한 처리를 하는 클래스
    /// </summary>
    class OutputProcessor
    {
        public void PrintCurrentPath(string currentPath)
        {
            Console.Write("\n"+currentPath + ">");
        }

        // 고정된 자리수의 문자열 출력
        public string PrintFixString(string toPrint, int limit, int align)
        {
            if (toPrint == null)
                toPrint = "";
            // 바이트 수를 받아옴
            int presentLength = Encoding.Default.GetBytes(toPrint).Length;

            // 공백 붙이기
            if(align == Constant.MIDDLE)
            {
                if ((limit - presentLength) % 2 == 1)
                    toPrint = ' ' + toPrint;

                for (int i = 0; i < (limit - presentLength) / 2; i++)
                    toPrint = ' ' + toPrint + ' ';
            }
            else
            {
                for (int i = 0; i < (limit - presentLength); i++)
                {
                    switch (align)
                    {
                        case Constant.LEFT:
                            toPrint = toPrint + ' ';
                            break;
                        case Constant.RIGHT:
                            toPrint = ' ' + toPrint;
                            break;
                    }
                }
            }

            return toPrint;
        }

        // 숫자 세 자리 수마다 콤마를 찍는다
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
            if (number.Length <= 3)
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

            for (int i = initCommaIndex; i < number.Length; i += 3)
            {
                returnNumber += ',';
                returnNumber += number.Substring(i, 3);
            }
            if (negative)
                returnNumber = '-' + returnNumber;

            return returnNumber;
        }
    }
}
