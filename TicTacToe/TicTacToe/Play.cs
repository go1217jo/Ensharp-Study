using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TicTacToe
{
    abstract class Play
    {
        protected int[] map;
        protected string[] nickname = new string[2];  // player1,2의 닉네임을 저장한다
        protected int win;   // 0이면 사용자1, 1이면 사용자2 승리, -1이면 무승부 / 0이면 사용자, 1이면 컴퓨터 승리
        protected int turn;  // 0이면 사용자1, 1이면 사용자2
        public int mode;  // User mode = 0, Computer mode = 1

        // 8 x 18 image
        public string[] blackBall = {"    ■■■■■    ",
                              "  ■■■■■■■  ",
                              "■■    ■■■■■",
                              "■■    ■■■■■",
                              "■■■■■■■■■",
                              "■■■■■■■■■",
                              "  ■■■■■■■  ",
                              "    ■■■■■    "};

        public string[] whiteBall = {"    ■■■■■    ",
                              "  ■          ■  ",
                              "■  ■■        ■",
                              "■  ■■        ■",
                              "■              ■",
                              "■              ■",
                              "  ■          ■  ",
                              "    ■■■■■    "};

        abstract public void ChoicePlayer(ScoreList scores);  // 플레이어 닉네임을 설정한다
        
        public void DecideTurn()  // 선공후공을 결정한다.
        {
            Console.WriteLine("\n\t\t\t\t\t\t\t선공후공은 50% 확률로 정해집니다...");
            Thread.Sleep(500);
            Random rand = new Random();
            turn = rand.Next(0, 2);
            if(mode == 0)  // 모드에 따라 출력 메시지가 다름
                Console.Write("\n\t\t\t\t\t\t\t" + nickname[turn] + "님이 선공입니다.");
            else {
                if (turn == 1)
                    Console.Write("\n\t\t\t\t\t\t\tComputer가 선공입니다.");
                else
                    Console.Write("\n\t\t\t\t\t\t\t" + nickname[turn] + "님이 선공입니다.");
            }
            Thread.Sleep(1000);
            Console.Clear();
        }

        public int CheckGame()  // 게임 종료 여부와 승리 여부를 확인한다.
        {  
            for (int stone = 0; stone < 2; stone++)
            {
                // 가로 검사
                for (int i = 0; i < 3; i++)
                {
                    if (map[i * 3] == stone && map[i * 3 + 1] == stone && map[i * 3 + 2] == stone)
                        return stone;
                }
                // 세로 검사
                for (int i = 0; i < 3; i++)
                {
                    if (map[i] == stone && map[i + 3] == stone && map[i + 6] == stone)
                        return stone;
                }
                // 대각선 검사
                if (map[0] == stone && map[4] == stone && map[8] == stone)
                    return stone;
                if (map[2] == stone && map[4] == stone && map[6] == stone)
                    return stone;
            }
            return -1;
        }

        public void PrintGameScreen()  // 게임 화면을 출력한다
        {
            ConsoleUI.GotoLine(3);
            
            if (mode == 0)  // 모드에 따라 출력 메시지가 다름
                Console.WriteLine("\t\t\t\t\t\t\t" + nickname[turn] + " turn");
            else
            {
                if (turn == 1)
                    Console.WriteLine("\t\t\t\t\t\t\t" + "Computer" + " turn");
                else
                    Console.WriteLine("\t\t\t\t\t\t\t" + nickname[turn] + " turn");
            }

            for (int k = 0; k < 9; k += 3)  // 격자를 생성하고 map에 맞게 돌을 출력한다
            {
                if (k == 0)
                    Console.WriteLine("\t\t\t\t\t  --------------------------------------------------------------");
                for (int i = 0; i < 8; i++)
                {
                    for (int j = k; j < k + 3; j++)
                    {
                        if (j == k)
                            Console.Write("\t\t\t\t\t | ");
                        if (map[j] == -1)
                            Console.Write("                  ");
                        else if (map[j] == 0)
                            Console.Write(whiteBall[i]);
                        else
                            Console.Write(blackBall[i]);
                        Console.Write(" | ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\t\t\t\t\t  --------------------------------------------------------------");
            }
            Console.WriteLine("\t\t\t\t\t\t\t\t\t1 2 3\n\t\t\t\t\t\t\t\t\t4 5 6\n\t\t\t\t\t\t\t\t\t7 8 9");
        }

    }
}
