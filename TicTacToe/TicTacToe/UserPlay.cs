using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class UserPlay {
        int[] map;
        string[] nickname = new string[2];  // player1,2의 닉네임을 저장한다
        int win;   // 0이면 사용자1, 1이면 사용자2 승리, -1이면 무승부
        int turn;  // 0이면 사용자1, 1이면 사용자2

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

        public UserPlay(ScoreList scores) {
            int choice;
            map = new int[9]{ -1,-1,-1,-1,-1,-1,-1,-1,-1};
            ChoicePlayer(scores);
            DecideTurn();
            while (true)
            {
                Console.Clear();
                PrintGameScreen();
                while (true)
                {
                    // 현재 입력된 키를 읽고 맞는지 체크한다.
                    char _input = Console.ReadKey().KeyChar;
                    if (_input < '1' || _input > '9')
                    {
                        Console.Clear();
                        Console.WriteLine("\n\t\t\tAlert : 1 ~ 9번 중 선택하세요");
                        PrintGameScreen();
                        continue;
                    }
                    else
                    {
                        choice = int.Parse(_input + "");
                        if (map[choice - 1] != -1)  // 이미 놓여진 자리라면
                            continue;
                        map[choice - 1] = turn;
                        if (turn == 0)
                            turn = 1;
                        else
                            turn = 0;
                        break;
                    }
                }
                win = CheckGame();
                if (win != -1)
                {
                    Console.Clear();
                    ConsoleUI.GotoLine(4);
                    ConsoleUI.DynamicPrint('#');
                    Console.WriteLine("  "+nickname[win] + "가 이겼습니다.");
                    scores.Update(nickname[win], true);
                    if (win == 0)
                        scores.Update(nickname[1], false);
                    else
                        scores.Update(nickname[0], false);
                    break;
                }
                else
                {
                    int iter;
                    for (iter = 0; iter < map.Length; iter++) {
                        if (map[iter] == -1) {
                            break;
                        }
                    }
                    if (iter == map.Length)
                    {
                        Console.Clear();
                        ConsoleUI.GotoLine(4);
                        ConsoleUI.DynamicPrint('#');
                        Console.WriteLine("  무승부입니다.");
                        for (int k = 0; k < 2; k++)  // 무승부이므로 둘 다 승이 아님
                            scores.Update(nickname[k], false); 
                        break;
                    }
                }
            }
        }
        void ChoicePlayer(ScoreList scores)
        {
            int choice=0;
            for (int i = 0; i < 2; i++)
            {
                while (true)
                {
                    ConsoleUI.GotoLine(3);
                    Console.WriteLine("\t\t\t\t\t\t\t\t<Player {0} 설정>", i+1);
                    Console.WriteLine("\t\t\t\t\t\t\t==============================");
                    Console.WriteLine("\t\t\t\t\t\t\t    1. 새로운 player 생성\n");
                    Console.WriteLine("\t\t\t\t\t\t\t    2. 기존 player 이어하기");
                    Console.WriteLine("\t\t\t\t\t\t\t==============================");
                    Console.Write("\t\t\t\t\t\t\tInput > ");

                    // 현재 입력된 키를 읽고 맞는지 체크한다.
                    char _input = Console.ReadKey().KeyChar;
                    if (_input < '1' || _input > '2')
                    {
                        Console.Clear();
                        Console.WriteLine("\n\t\t\tAlert : 1 ~ 2번 중 선택하세요");
                        continue;
                    }
                    else
                    {
                        choice = int.Parse(_input + "");
                        Console.WriteLine();
                        break;
                    }
                    
                }

                if (choice == 1)
                {
                    bool repeat = true;
                    
                    Console.WriteLine("\n\t\t\t\t\t\t\t<새 플레이어 명을 정해주세요.>");
                    while (repeat)  // 공백 문자가 포함되지 않은 닉네임이 입력될 때까지 반복
                    {
                        Console.Write("\t\t\t\t\t\t\tNickname : ");
                        this.nickname[i] = Console.ReadLine();
                        if (this.nickname[i].Length > 8)  // 닉네임 길이가 8자가 넘어간다면
                        {
                            this.nickname[i] = this.nickname[i].Substring(0, 8);  // 닉네임이 너무 길면 문자열을 자름
                            Console.WriteLine("\n\t\t\t\t\t\t\t닉네임이 너무 길어 자동으로 잘립니다.");
                        }
                        if (this.nickname[i].Length == 0) { // 공백 문자가 입력되면 다시 입력 받음
                            Console.WriteLine("\n\t\t\t\t\t\t\t공백 문자가 포함될 수 없습니다.");
                            continue;
                        }
                        else {
                            for (int j = 0; j < this.nickname[i].Length; j++) // 공백 문자가 입력되면 임의로 닉네임을 생성
                            {
                                if (this.nickname[i][j] == '\n' || this.nickname[i][j] == '\t' || this.nickname[i][j] == ' ') { // 공백 문자가 포함된다면
                                    Console.WriteLine("\n\t\t\t\t\t\t\t공백 문자가 포함될 수 없습니다.");
                                    break;
                                }
                                if (j + 1 == this.nickname[i].Length)  // 공백 문자 없음 확인
                                    repeat = false;
                            }
                        }
                        if (scores.IsThere(this.nickname[i])) {
                            Console.WriteLine("\n\t\t\t\t\t\t\t해당 닉네임이 존재합니다. 다시 입력해주세요.");
                            repeat = true;
                            continue;
                        }

                        if (i == 1 && this.nickname[1].Equals(this.nickname[0])) {
                            Console.WriteLine("\n\t\t\t\t\t\t\t플레이어의 이름이 중복됩니다. 다시 입력해주세요.");
                            repeat = true;
                            continue;
                        }
                    }
                    scores.Push(nickname[i]);
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine("\n\t\t\t\t\t\t\t불러올 플레이어 명을 입력하세요");
                        Console.Write("\t\t\t\t\t\t\tNickname : ");
                        this.nickname[i] = Console.ReadLine();

                        if (i==1 && this.nickname[1].Equals(this.nickname[0]))
                            Console.WriteLine("\n\t\t\t\t\t\t\t플레이어의 이름이 중복됩니다. 다시 입력해주세요.");
                        else
                            break;
                    }
                    if (!scores.IsThere(this.nickname[i]))  // 닉네임이 존재하는지 검사한다
                    {
                        Console.WriteLine("\n\t\t\t\t\t\t\t닉네임 검색 결과 존재하지 않으므로 새로 생성합니다.");
                        scores.Push(this.nickname[i]);  // 새로운 닉네임을 등록한다
                    }
                }
                Thread.Sleep(400);
                if(i==0)
                    Console.Clear();
            }
        }
        public void DecideTurn()
        {
            Console.WriteLine("\n\t\t\t\t\t\t\t선공후공은 50% 확률로 정해집니다...");
            Thread.Sleep(500);
            Random rand = new Random();
            turn = rand.Next(0, 2);
            Console.Write("\n\t\t\t\t\t\t\t" + nickname[turn] + "님이 선공입니다.");
            Thread.Sleep(1000);
            Console.Clear();
        }

        public int CheckGame()
        {  // 게임 종료 여부와 승리 여부를 확인한다.
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
            Console.WriteLine("\t\t\t\t\t\t\t" + nickname[turn] + " turn");

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
