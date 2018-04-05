using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Menu
    {
        int choiceNumber;
        ScoreList players = new ScoreList();

        public void PrintMenu()
        {
            ConsoleUI.GotoLine(6);
            Console.WriteLine("\t\t\t\t\t\t\t=========== Menu ===========\n");
            Console.WriteLine("\t\t\t\t\t\t\t   1. Play With Computer\n");
            Console.WriteLine("\t\t\t\t\t\t\t   2. User vs User\n");
            Console.WriteLine("\t\t\t\t\t\t\t   3. Score Board\n");
            Console.WriteLine("\t\t\t\t\t\t\t   0. Exit\n");
            Console.WriteLine("\t\t\t\t\t\t\t============================\n");
            Console.Write("\t\t\t\t\t\t\t Menu Choice > ");
        }

        public Menu()
        {
            MenuChoice();
        }

        public void MenuChoice()
        {
            while (true)
            {
                PrintMenu();  // 메뉴를 출력한다.
                // 현재 입력된 키를 읽고 맞는지 체크한다.
                char _input = Console.ReadKey().KeyChar;
                if (_input < '0' || _input > '3') {
                    Console.Clear();
                    Console.WriteLine("\n\t\t\tAlert : 1 ~ 3번 중 선택하세요");
                    continue;
                }
                choiceNumber = int.Parse(_input+"");
                Console.Clear();

                switch (choiceNumber)
                {
                    case 0:
                        return;
                    case 1:
                        new ComputerPlay(players);
                        break;
                    //break;
                    case 2:
                        Console.WriteLine("call User vs User");
                        return;
                    //break;
                    case 3:
                        players.RankPrint();
                        break;
                    default:
                        Console.WriteLine("\n\t\t\tAlert : 1 ~ 3번 중 선택하세요");
                        break;
                }
            }
        }
    }
}
