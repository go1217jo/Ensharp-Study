using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Command
{
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
                        case Constant.MIDDLE:
                            toPrint = ' ' + toPrint;
                            break;
                    }
                }
            }

            return toPrint;
        }
    }
}
