using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{    
    class MainScreen
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(145, 40);
            FirstScreen First = new FirstScreen();
            while (true) {
                First.PrintScreen();
                if (Console.ReadKey().KeyChar != 0) {
                    Console.Clear();
                    break;
                }
            }
            new Menu();
        }
    }
}