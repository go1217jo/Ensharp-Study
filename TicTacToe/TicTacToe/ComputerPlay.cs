using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class ComputerPlay : Play
    {
        // turn 0 : 사용자, turn 1 : 컴퓨터

        public ComputerPlay(ScoreList scores)  // 생성자가 메인 플로우
        {
            int difficult;
            mode = 1;  // 컴퓨터 모드로 설정
            map = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            ChoicePlayer(scores);  // 저장된 닉네임 혹은 닉네임을 새로 생성하여 닉네임을 반환한다.
            while (true) {
                Console.WriteLine("\n\n\t\t\t\t\t\t\t\t<난이도 선택>");
                Console.WriteLine("\n\t\t\t\t\t\t\t      1. 쉬움, 2. 보통");
                Console.Write("\n\t\t\t\t\t\t\tInput > ");
                
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
                    difficult = int.Parse(_input + "");
                    Console.WriteLine();
                    break;
                }
            }
            switch (difficult)  // 선택된 난이도에 따라 게임을 실행시키고 게임 결과를 SCORE에 업데이트한다
            {
                case 1:
                    win = EasyGame();  // 쉬운 난이도 실행
                    if (win == 0)  // win이 0이면 사용자 우승
                        scores.Update(nickname[0], true);  // 이긴 성적 업데이트
                    else
                        scores.Update(nickname[0], false);  // 패배 성적 업데이트
                    break;
                case 2:
                    win = NormalGame();  // 보통 난이도 실행
                    if (win == 0)  // win이 0이면 사용자 우승
                        scores.Update(nickname[0], true);  // 이긴 성적 업데이트
                    else
                        scores.Update(nickname[0], false);  // 패배 성적 업데이트
                    break;
            }
        }

        override public void ChoicePlayer(ScoreList scores)  // 플레이어를 생성하거나 기존의 닉네임을 선택한다
        {
            int choice;
            while(true)
            {
                ConsoleUI.GotoLine(3);
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
                else {
                    choice = int.Parse(_input + "");
                    Console.WriteLine();
                    break;
                }
            }
            if (choice == 1) {  // 플레이어를 새로 생성한다.
                bool repeat = true;
                Console.WriteLine("\n\t\t\t\t\t\t\t<새 플레이어 명을 정해주세요.>");
                while (repeat)  // 공백 문자가 포함되지 않은 닉네임이 입력될 때까지 반복
                {
                    Console.Write("\t\t\t\t\t\t\tNickname : ");
                    this.nickname[0] = Console.ReadLine();
                    if (this.nickname[0].Length > 8) {
                        this.nickname[0] = this.nickname[0].Substring(0, 8);  // 닉네임이 너무 길면 문자열을 자름
                        Console.WriteLine("\n\t\t\t\t\t\t\t닉네임이 너무 길어 자동으로 잘립니다.");
                    }
                    if (this.nickname[0].Length == 0) { // 공백 문자가 입력되면 임의로 닉네임을 생성
                        Console.WriteLine("\n\t\t\t\t\t\t\t공백 문자가 포함될 수 없습니다.");
                        continue;
                    }
                    else {
                        for (int i = 0; i < this.nickname[0].Length; i++) // 공백 문자가 입력되면 임의로 닉네임을 생성
                        {
                            if (this.nickname[0][i] == '\n' || this.nickname[0][i] == '\t' || this.nickname[0][i] == ' ') {
                                Console.WriteLine("\n\t\t\t\t\t\t\t공백 문자가 포함될 수 없습니다.");
                                break;
                            }
                            if (i+1 == this.nickname[0].Length)
                                repeat = false;
                        }
                    }
                }

                if (scores.IsThere(this.nickname[0]))  // 현재 입력된 닉네임이 존재하는지 확인한다.
                {
                    Console.WriteLine("\n\t\t\t\t\t\t\t해당 닉네임이 존재하여 이어서 시작합니다.");
                }
                else {
                    scores.Push(this.nickname[0]);  // 닉네임 추가
                    Console.WriteLine("\n\t\t\t\t\t\t\t\t\t\t플레이어 생성!");
                }
            }
            else
            {
                Console.WriteLine("\n\t\t\t\t\t\t\t불러올 플레이어 명을 입력하세요");
                Console.Write("\t\t\t\t\t\t\tNickname : ");
                this.nickname[0] = Console.ReadLine();
                if(!scores.IsThere(this.nickname[0]))  // 불러올 닉네임이 있는지 확인한다.
                {
                    Console.WriteLine("\n\t\t\t\t\t\t\t닉네임 검색 결과 존재하지 않으므로 새로 생성합니다.");
                    scores.Push(this.nickname[0]);  // 닉네임을 추가한다.
                }
            }
        }

        public int EasyGame()  // 쉬운 모드
        {
            int choice;
            Random rand = new Random();

            DecideTurn();  // 사용자와 컴퓨터 턴을 결정한다.

            while (true) {
                PrintGameScreen();  // 게임 화면 출력

                // 컴퓨터가 랜덤으로 아직 놓이지 않은 자리에 놓는다
                if (turn == 1) {  // 컴퓨터 차례이면
                    while (true) {
                        choice = rand.Next(1, 10);
                        if (map[choice - 1] == -1)
                            break;
                    }
                    map[choice - 1] = 1;
                    turn = 0;
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
                            map[choice - 1] = 0;
                            Console.WriteLine();
                            break;
                        }
                    }
                    turn = 1;
                }
                Console.Clear();
                win = CheckGame();  // 게임 결과 및 종료 여부 확인
                if (win != -1) {
                    ConsoleUI.GotoLine(4);
                    ConsoleUI.DynamicPrint('#');
                    if (win == 1) {
                        Console.WriteLine("  컴퓨터가 이겼습니다.");
                    }
                    else
                        Console.WriteLine("  {0}가 이겼습니다.", nickname[0]);
                    break;
                }
                else {
                    int iter;
                    for(iter=0;iter<map.Length;iter++) {  // 모든 판이 전부 채워졌는지 확인
                        if (map[iter] == -1) {
                            break;
                        }
                    }
                    if(iter == map.Length) {  // 무승부 여부를 확인
                        ConsoleUI.GotoLine(4);
                        ConsoleUI.DynamicPrint('#');
                        Console.WriteLine("  무승부입니다.");
                        return 0;
                    }
                }
            }
            return win;
        }
        
        public int NormalGame() {  // 보통 모드
            int choice;
            Random rand = new Random();

            DecideTurn();  // 사용자와 컴퓨터 턴을 결정한다.

            while (true)
            {
                PrintGameScreen();  // 게임 화면 출력

                // 컴퓨터가 아직 놓이지 않은 자리에 놓는다
                if (turn == 1)
                {  // 컴퓨터 차례이면
                    while (true)
                    {
                        choice = PositionDecision();  // 컴퓨터가 위치를 찍는다
                        if (map[choice - 1] == -1)
                            break;
                    }
                    map[choice - 1] = 1;
                    turn = 0;
                }
                else
                {  // 사용자 차례이면
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
                            if (map[choice - 1] != -1)
                                continue;
                            map[choice - 1] = 0;
                            Console.WriteLine();
                            break;
                        }
                    }
                    turn = 1;
                }
                Console.Clear();
                win = CheckGame();
                if (win != -1)
                {
                    ConsoleUI.GotoLine(4);
                    ConsoleUI.DynamicPrint('#');
                    if (win == 1)
                    {
                        Console.WriteLine("  컴퓨터가 이겼습니다.");
                    }
                    else
                        Console.WriteLine("  {0}가 이겼습니다.", nickname[0]);
                    break;
                }
                else
                {
                    int iter;
                    for (iter = 0; iter < map.Length; iter++)  // 무승부 여부를 판단한다
                    {
                        if (map[iter] == -1)
                        {
                            break;
                        }
                    }
                    if (iter == map.Length)
                    {
                        ConsoleUI.GotoLine(4);
                        ConsoleUI.DynamicPrint('#');
                        Console.WriteLine("  무승부입니다.");
                        return 0;
                    }
                }
            }
            return win;
        }

        public int PositionDecision() {  // 놓을 위치를 결정한다
            int continuous = 0;
            if (map[4] == -1)
                return 5;
            // 가로 단순 방어
            for(int i=0;i<7; i+=3) {
                for(int j=i; j<i+3; j++) {
                    if (map[j] == 0) {
                        continuous++;
                        if(continuous == 2) {
                            if(j == i+1) {
                                if (map[i + 2] == -1)
                                    return i + 2 + 1;  // 인덱스 위치에서 1 증가한 값 반환
                            }
                            if (j == i + 2) {
                                if (map[i] == -1)
                                    return i + 1;  // 인덱스 위치에서 1 증가한 값 반환
                            }
                        }
                    }
                    else
                        continuous = 0;
                }
            }
            // 세로 단순 방어
            for (int i = 0; i < 3; i++)
            {
                for (int j = i; j < i + 7; j+=3)
                {
                    if (map[j] == 0)
                    {
                        continuous++;
                        if (continuous == 2)
                        {
                            if (j == i + 3)
                            {
                                if (map[i + 6] == -1)
                                    return i + 6 + 1;  // 인덱스 위치에서 1 증가한 값 반환
                            }
                            if (j == i + 6)
                            {
                                if (map[i] == -1)
                                    return i + 1;  // 인덱스 위치에서 1 증가한 값 반환
                            }
                        }
                    }
                    else
                        continuous = 0;
                }
            }
            return new Random().Next(1, 10);  // 마땅히 놓을 곳 없으면 아무데나
        }
    }
}
