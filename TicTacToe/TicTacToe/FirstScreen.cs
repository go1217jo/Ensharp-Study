using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class FirstScreen
    {
        public void PrintScreen()  // 초기화면 출력
        {
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
