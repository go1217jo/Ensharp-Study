using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TicTacToe
{
    class ConsoleUI
    {
        public static void GotoLine(int n)  // 여러 줄 개행
        {
            for (int i = 0; i < n; i++)
                Console.WriteLine();
        }
        public static void DynamicPrint(char ch) {
            for(int i=0;i < 20;i++) {
                Console.Write(ch+"");
                Thread.Sleep(30);
            }
        }
    }
}
