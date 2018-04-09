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
            Console.SetWindowSize(145, 40);  // 콘솔 화면 크기 설정
            FirstScreen First = new FirstScreen();
            while (true) {
                First.PrintScreen(); // 첫 화면 출력
                if (Console.ReadKey().KeyChar != 0) {  // 아무키나 누를 때까지 반복
                    Console.Clear();
                    break;
                }
            }
            new Menu();  // 메뉴 선택
        }
    }
}