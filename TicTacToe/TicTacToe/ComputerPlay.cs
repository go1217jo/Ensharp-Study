using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class ComputerPlay
    {
        int[] map;
        string nickname;
        int win;
        int turn;  // 0이면 컴퓨터 턴, 1이면 사용자 턴
        // 8 x 18 image
        string[] blackBall = {"    ■■■■■    ",
                              "  ■■■■■■■  ",
                              "■■    ■■■■■",
                              "■■    ■■■■■",
                              "■■■■■■■■■",
                              "■■■■■■■■■",
                              "  ■■■■■■■  ",
                              "    ■■■■■    "};

        string[] whiteBall = {"    ■■■■■    ",
                              "  ■          ■  ",
                              "■  ■■        ■",
                              "■  ■■        ■",
                              "■              ■",
                              "■              ■",
                              "  ■          ■  ",
                              "    ■■■■■    "};

        public void PrintGameScreen()
        {
            ConsoleUI.GotoLine(2);
            if (turn == 0)
                Console.WriteLine("\t\tComputer turn\n");
            else
                Console.WriteLine("\t\t{0} turn\n", nickname);
            
            for (int k = 0; k < 9; k += 3)
            {
                if(k==0)
                    Console.WriteLine("  --------------------------------------------------------------");
                for (int i = 0; i < 8; i++)
                {
                    for (int j = k; j < k+3; j++)
                    {
                        if (j == k)
                            Console.Write(" | ");
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
                Console.WriteLine("  --------------------------------------------------------------");
            }
        }

        public void DecideTurn()
        {
            Console.WriteLine("선공후공은 50% 확률로 정해집니다...");
            Thread.Sleep(500);
            Random rand = new Random();
            if (rand.Next() % 2 == 0) {
                turn = 1;
                Console.WriteLine("선공입니다!");
            }
            else {
                turn = 0;
                Console.WriteLine("후공입니다!");
            }
            Thread.Sleep(1000);
            Console.Clear();
        }

        public ComputerPlay(ScoreList scores)  // 생성자가 메인 플로우
        {
            int difficult;
            map = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            ChoicePlayer(scores);  // 저장된 닉네임 혹은 닉네임을 새로 생성하여 닉네임을 반환한다.
            while (true) {
                Console.WriteLine("난이도 선택");
                Console.WriteLine("1. 쉬움, 2. 보통, 3. 어려움");
                Console.Write("Input > ");
                
                // 현재 입력된 키를 읽고 맞는지 체크한다.
                char _input = Console.ReadKey().KeyChar;
                if (_input < '1' || _input > '3')
                {
                    Console.Clear();
                    Console.WriteLine("\n\t\t\tAlert : 1 ~ 3번 중 선택하세요");
                    continue;
                }
                else
                {
                    difficult = int.Parse(_input + "");
                    Console.WriteLine();
                    break;
                }
            }
            switch (difficult)
            {
                case 1:
                    win = EasyGame();
                    if (win == 1)
                        scores.Update(nickname, true);
                    else
                        scores.Update(nickname, false);
                    break;
                case 2:
                    win = NormalGame();
                    break;
                case 3:
                    win = DifficultGame();
                    break;
            }
        }

        void ChoicePlayer(ScoreList scores)
        {
            int choice;
            while(true)
            {
                Console.WriteLine("1. 새로운 player 생성");
                Console.WriteLine("2. 기존 player 이어하기");
                Console.Write("Input > ");

                // 현재 입력된 키를 읽고 맞는지 체크한다.
                char _input = Console.ReadKey().KeyChar;
                if (_input < '0' || _input > '2')
                {
                    Console.Clear();
                    Console.WriteLine("\n\t\t\tAlert : 1 ~ 2번 중 선택하세요");
                    continue;
                }
                else {
                    choice = int.Parse(_input + "");
                    Console.WriteLine();
                    break;
                }
            }
            if (choice == 1) {
                Console.WriteLine("새 플레이어 명을 정해주세요.");
                Console.Write("Nickname : ");
                this.nickname = Console.ReadLine();
                if(scores.IsThere(this.nickname))
                {
                    Console.WriteLine("해당 닉네임이 존재하여 이어서 시작합니다.");
                }
                else
                    scores.Push(this.nickname);
            }
            else
            {
                Console.WriteLine("불러올 플레이어 명을 입력하세요");
                Console.Write("Nickname : ");
                this.nickname = Console.ReadLine();
                if(!scores.IsThere(this.nickname))
                {
                    Console.WriteLine("닉네임 검색 결과 존재하지 않으므로 새로 생성합니다.");
                    scores.Push(this.nickname);
                }
            }
        }

        public int CheckGame() {  // 게임 종료 여부와 승리 여부를 확인한다.
            for (int stone = 0; stone < 2; stone++) {
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

        public int EasyGame()
        {
            int choice;
            Random rand = new Random();

            DecideTurn();  // 사용자와 컴퓨터 턴을 결정한다.

            while (true) {
                PrintGameScreen();  // 게임 화면 출력

                // 컴퓨터가 랜덤으로 아직 놓이지 않은 자리에 놓는다
                if (turn == 0) {  // 컴퓨터 차례이면
                    while (true) {
                        choice = rand.Next(1, 10);
                        if (map[choice - 1] == -1)
                            break;
                    }
                    map[choice - 1] = 0;
                    turn = 1;
                }
                else {  // 사용자 차례이면
                    while (true) {
                        // 현재 입력된 키를 읽고 맞는지 체크한다.
                        char _input = Console.ReadKey().KeyChar;
                        if (_input < '1' || _input > '9')
                        {
                            Console.Clear();
                            Console.WriteLine("\n\t\t\tAlert : 1 ~ 9번 중 선택하세요");
                            PrintGameScreen();
                            continue;
                        }
                        else {

                            choice = int.Parse(_input + "");
                            if (map[choice - 1] != -1)
                                continue;
                            map[choice - 1] = 1;
                            Console.WriteLine();
                            break;
                        }
                    }
                    turn = 0;
                }
                Console.Clear();
                win = CheckGame();
                if (win != -1) {
                    ConsoleUI.GotoLine(4);
                    ConsoleUI.DynamicPrint('#');
                    if (win == 0) {
                        Console.WriteLine("  컴퓨터가 이겼습니다.");
                    }
                    else
                        Console.WriteLine("  {0}가 이겼습니다.", nickname);
                    break;
                }
                else {
                    int iter;
                    for(iter=0;iter<map.Length;iter++) {
                        if (map[iter] == -1) {
                            break;
                        }
                    }
                    if(iter == map.Length) {
                        ConsoleUI.GotoLine(4);
                        ConsoleUI.DynamicPrint('#');
                        Console.WriteLine("  무승부입니다.");
                        return 0;
                    }
                }
            }
            return win;
        }
        
        public int NormalGame() {
            return -1;
        }

        public int DifficultGame() {
            return -1;
        }
    }
}
