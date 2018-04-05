using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Score
    {
        private string nickname;  // 유저 닉네임
        private int score;  // 게임 이긴 횟수
        private int times;  // 게임 플레이 횟수
       
        public Score(string nickname, int score)  // 초기화
        {
            this.nickname = nickname;
            this.score = score;
            times = 0;
        }
        
        public void PrintScore()  // 플레이어 성적 출력
        {
            if (times != 0)
            {
                float rate = (float)score / times;  // 승률 계산
                Console.WriteLine("Nickname : {0}    Win times : {1}    Winning Rate : {2}%", nickname, score, rate*100);
            }
            else
                Console.WriteLine("Nickname : {0}    Win times : {1}", nickname, score);
        }

        public void AlertScore(bool win)  // 이기면 게임 수와 승 수를 변경한다.
        {
            times++;
            if (win)
                score++;
        }

        public string GetNickName()  // 닉네임 반환
        {
            return nickname;
        }
        public int GetScore()  // 승 수 반환
        {
            return score;
        }
    }
    
    class ScoreList
    {
        ArrayList scores;

        public ScoreList()  // ArrayList 초기화
        {
            scores = new ArrayList();
        }

        public bool IsThere(string nickname)  // 매개변수로 입력된 닉네임이 현재 존재하는지 확인하여 존재여부를 반환함
        {
            bool yes = false;
            Score temp;
            foreach (object obj in scores)
            {
                temp = (Score)obj;
                if(temp.GetNickName().Equals(nickname))
                {
                    yes = true;
                    break;
                }
            }
            return yes;
        }
        public void Push(string nickname)  // 신규 플레이어를 추가한다.
        {
            scores.Add(new Score(nickname, 0));  // 맨 뒤에 추가, 초기 점수는 0
        }

        public void Update(string nickname, bool win)  // 기존 플레이어의 전적을 업데이트한다.
        {
            foreach(object obj in scores)  // 이 반복에서 플레이어를 못 찾는 경우는 없다, 플레이 후 갱신되기 때문
            {
                Score player = (Score)obj;
                if (player.GetNickName().Equals(nickname))  // 갱신할 player 탐색
                    player.AlertScore(win);  // 점수 갱신
            }
        }

        public void MergeSort(int left, int right)  // 승 수를 기준으로 내림차순 정렬한다.
        {
            int middle = (left + right) / 2;
            Score temp1, temp2;
            int l = left, r = middle + 1, idx=0;
            Score[] tempArr = new Score[scores.Count];
            if (left >= right)
                return;
            
            MergeSort(left, middle);
            MergeSort(middle + 1, right);

            // Merge 부분
            while(l <= middle && r <= right) // 분할된 것을 merge한다. 
            {
                temp1 = (Score)scores[l];
                temp2 = (Score)scores[r];
                if (temp1.GetScore() > temp2.GetScore()) {
                    tempArr[idx++] = temp1;
                    l++;
                }
                else {
                    tempArr[idx++] = temp2;
                    r++;
                }
            }
            while (l <= middle)  
                tempArr[idx++] = (Score)scores[l++];
            while (r <= right)
                tempArr[idx++] = (Score)scores[r++];

            for (idx = left; idx <= right; idx++)  // 정렬된 부분을 원래 배열 부분으로 바꾸어준다.
                scores[idx] = tempArr[idx];
        }
        
        public void RankPrint()  // 플레이어들 성적 출력
        {
            Score temp;
            MergeSort(0, scores.Count-1);
            Console.Clear();
            ConsoleUI.GotoLine(4);
            Console.WriteLine("\t\t\t\t\t===============================================================");
            for (int i=0;i<scores.Count; i++)
            {
                temp = (Score)scores[i]; 
                Console.Write("\t\t\t\t\t {0} ",i+1);
                temp.PrintScore();
            }
            Console.WriteLine("\t\t\t\t\t===============================================================");
            Console.WriteLine("\t\t\t\t\t\t\t\tPress Any Key...");
            while (true)
            {
                if (Console.ReadKey().KeyChar != 0)
                {
                    Console.Clear();
                    break;
                }
            }
        }
    }    
}
