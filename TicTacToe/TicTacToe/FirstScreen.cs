using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TicTacToe
{
    class FirstScreen
    {
        public void PrintScreen()  // 초기화면 출력
        {
            Console.Write("\n\t\t\t\t");
            ConsoleUI.DynamicPrint('E');
            ConsoleUI.DynamicPrint('N');
            ConsoleUI.DynamicPrint('#');
            ConsoleUI.DynamicPrint('#');
            ConsoleUI.GotoLine(5);
            Console.WriteLine("\t\t==================================================================================================================");
            Console.WriteLine("\t\t□□□□□□□  □□  □□□□□    □□□□□□□   □□     □□□□□    □□□□□□□  □□□□□  □□□□□");
            Console.WriteLine("\t\t      □        □□  □                  □        □  □    □                  □        □      □  □");
            Console.WriteLine("\t\t      □              □                  □       □    □   □                  □        □      □  □");
            Console.WriteLine("\t\t      □         □   □                  □      □      □  □                  □        □      □  □");
            Console.WriteLine("\t\t      □         □   □                  □      □□□□□  □                  □        □      □  □□□□□");
            Console.WriteLine("\t\t      □         □   □                  □      □      □  □                  □        □      □  □");
            Console.WriteLine("\t\t      □         □   □                  □      □      □  □                  □        □      □  □");
            Console.WriteLine("\t\t      □         □   □                  □      □      □  □                  □        □      □  □");
            Console.WriteLine("\t\t      □         □   □□□□□          □      □      □  □□□□□          □        □□□□□  □□□□□");
            Console.WriteLine("\t\t==================================================================================================================");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t\t\t\t- Press Any Key -");
        }
    }
}
