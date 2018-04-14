using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagement.UI
{
    class KeyInput
    {
        public const int LEFT = 4;
        public const int UP = 8;
        public const int RIGHT = 6;
        public const int DOWN = 5;
        public const int ENTER = 0;

        // 방향키 이외의 입력들에 대해 예외처리를 하며 방향키가 들어오면 설정해놓은 상수를 반환한다.
        public int PressDirectionKey()
        {
            ConsoleKeyInfo inputKey;
            while (true)
            {
                inputKey = Console.ReadKey();
                switch (inputKey.Key)
                {
                    case ConsoleKey.LeftArrow:
                        return LEFT;
                    case ConsoleKey.RightArrow:
                        return RIGHT;
                    case ConsoleKey.UpArrow:
                        return UP;
                    case ConsoleKey.DownArrow:
                        return DOWN;
                    case ConsoleKey.Enter:
                        return ENTER;
                    case ConsoleKey.Backspace:
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                        Console.Write(" ");
                        break;
                    default:
                        // 한글은 글자당 2바이트이므로 입력되었을 경우 2바이트씩 진행한다.
                        // 따라서 한글과 같은 2바이트 문자에 대해 따로 처리해준다.
                        if(Encoding.Default.GetBytes(inputKey.KeyChar+"").Length == 2)
                            Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                        else
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        break;
                }
            }
        }

        // 방향키에 따라 위 아래로만 움직이게 하고 엔터를 치면 true를 반환하는 함수
        public bool ChoiceByKey() {
            switch (PressDirectionKey())
            {
                case KeyInput.UP:
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    return false;
                case KeyInput.DOWN:
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop + 1);
                    return false;
                case KeyInput.ENTER:
                    Console.Clear();
                    return true;
                default:
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    return false;
            }
        }

        // 문자열을 읽어오면서 동시에 문자열이 조건을 충족하는지 확인하여 반환한다.
        // letterLimit : 최대 글자 수, screenHeight : 출력화면 높이
        // cursorLeft : 입력받기 시작하는 위치(x좌표), cursorTop : 입력받기 시작하는 위치(y좌표)
        // blankLimit : 공백 허용을 하면 false, 제한을 두려면 true
        public string ReadAndCheckString(int letterLimit, int screenHeight, int cursorLeft, int cursorTop, bool blankLimit)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();

                // 입력된 문자열 길이가 제한 수를 넘어간다면
                if (input.Length > letterLimit)
                {
                    // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                    Console.SetCursorPosition(5, screenHeight - 2);
                    Console.WriteLine("잘못된 입력 : 글자 수 제한");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("                                ");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    continue;
                }

                // 입력되지 않은 경우 다시 입력
                if (input.Length == 0)
                {
                    // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                    Console.SetCursorPosition(5, screenHeight - 2);
                    Console.WriteLine("잘못된 입력 : 입력되지 않음");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("                                ");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    continue;
                }
                else
                {
                    bool possible = false;
                    bool noBlank = true;

                    // 공백문자만 입력된 경우
                    for (int i = 0; i < input.Length; i++) {
                        if (input[i] != '\n' && input[i] != '\t' && input[i] != ' ')  // 공백 문자가 아닌 문자가 있다면
                            possible = true;
                        else {
                            // 공백 문자를 허용하지 않는다면
                            if (blankLimit) {
                                // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                                Console.SetCursorPosition(5, screenHeight - 2);
                                Console.WriteLine("잘못된 입력 : 공백이 입력됨");
                                Console.SetCursorPosition(cursorLeft, cursorTop);
                                Console.Write("                                ");
                                Console.SetCursorPosition(cursorLeft, cursorTop);
                                noBlank = false;
                                break;
                            }
                        }
                    }
                    // 공백 문자만 입력되었을 경우 다시 입력 받음
                    if(!possible){
                        // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                        Console.SetCursorPosition(5, screenHeight - 2);
                        Console.WriteLine("잘못된 입력 : 공백만 입력됨");
                        Console.SetCursorPosition(cursorLeft, cursorTop);
                        Console.Write("                                ");
                        Console.SetCursorPosition(cursorLeft, cursorTop);
                        continue;
                    }
                    if (!noBlank)
                        continue;
                }
                // 입력된 문자열을 반환한다.
                return input;
            }
        }

        // password 입력 받을 시 사용하기 위해 오버로딩
        public string ReadAndCheckString(int letterLimit, int screenHeight, int cursorLeft, int cursorTop)
        {
            string input = "";
            ConsoleKeyInfo passwordKey;

            while (true)
            {
                // password 입력받음 * 표시
                while (true) {
                    passwordKey = Console.ReadKey(true);
                    if (passwordKey.Key == ConsoleKey.Enter)
                        break;
                    if (passwordKey.KeyChar >= 33 && passwordKey.KeyChar <= 126)
                    {
                        Console.Write("*");
                        input += passwordKey.KeyChar;
                    }
                    if(passwordKey.Key == ConsoleKey.Backspace) {
                        Console.SetCursorPosition(Console.CursorLeft-1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        if(input.Length > 0)
                            input = input.Remove(input.Length - 1);
                        if (cursorLeft > Console.CursorLeft)
                            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    }
                }

                // 입력된 문자열 길이가 제한 수를 넘어간다면
                if (input.Length > letterLimit)
                {
                    // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                    Console.SetCursorPosition(5, screenHeight - 2);
                    Console.WriteLine("잘못된 입력 : 글자 수 제한");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("                                ");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    continue;
                }

                // 입력되지 않은 경우 다시 입력
                if (input.Length == 0)
                {
                    // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                    Console.SetCursorPosition(5, screenHeight - 2);
                    Console.WriteLine("잘못된 입력 : 입력되지 않음");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("                                ");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    continue;
                }
                else
                {
                    bool possible = true;

                    // 공백문자만 입력된 경우
                    for (int i = 0; i < input.Length; i++)
                    {
                        // 공백 문자가 있다면 
                        if (input[i] == '\n' && input[i] == '\t' && input[i] == ' ')
                        { 
                            // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                            Console.SetCursorPosition(5, screenHeight - 2);
                            Console.WriteLine("잘못된 입력 : 공백이 입력됨");
                            Console.SetCursorPosition(cursorLeft, cursorTop);
                            Console.Write("                                ");
                            Console.SetCursorPosition(cursorLeft, cursorTop);
                            possible = false;
                            break;
                        }
                    }
                    // 공백 문자가 있다면 다시 진행
                    if (!possible)
                        continue;
                }
                // 입력된 문자열을 반환한다.
                return input;
            }
        }

        // 이름 형식으로 입력받아 문자열을 반환하는 함수
        public string NameFormatInput(int cursorLeft)
        {
            ConsoleKeyInfo inputKey;
            string inputString = "";

            while (inputString.Length < 3) {
                inputKey = Console.ReadKey();
                if (Encoding.Default.GetBytes(inputKey.KeyChar + "").Length == 2) {
                    inputString += inputKey.KeyChar;
                    if (inputString.Length == 1)
                        Console.Write(" ");
                }
                else  {
                    // 백스페이스에 대한 예외처리
                    if (inputKey.Key == ConsoleKey.Backspace)
                    {
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        if (inputString.Length > 0)
                            inputString = inputString.Remove(inputString.Length - 1);
                        if (cursorLeft > Console.CursorLeft)
                            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    }
                    // 한글과 다른 특수문자의 입력에 대비하여 바이트 수를 계산하여 커서를 옮긴다
                    else {
                        int bufferLength = Encoding.Default.GetBytes(inputKey.KeyChar + "").Length;
                        if (Console.CursorLeft - bufferLength < cursorLeft)
                            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        else
                            Console.SetCursorPosition(Console.CursorLeft - bufferLength, Console.CursorTop);
                    }

                }
            }

            // 이름을 전부 입력한 뒤 들어오는 키 입력에 대한 예외처리
            inputKey = Console.ReadKey();
            if(inputKey.Key != ConsoleKey.Enter)
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            else
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.Write("  ");

            return inputString;
        }

        // 학번 형식으로 입력받아 문자열을 반환하는 함수
        public string StudentNoFormatInput(int cursorLeft)
        {
            string inputString = "";
            ConsoleKeyInfo inputKey;

            while (inputString.Length < 8)
            {
                inputKey = Console.ReadKey();
                if (inputKey.KeyChar >= '0' && inputKey.KeyChar <= '9')
                    inputString += inputKey.KeyChar;
                else
                {
                    // 백스페이스에 대한 예외처리
                    if (inputKey.Key == ConsoleKey.Backspace)
                    {
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft-1, Console.CursorTop);
                        if (inputString.Length > 0)
                            inputString = inputString.Remove(inputString.Length - 1);
                        if (cursorLeft > Console.CursorLeft)
                            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    }
                    // 한글과 다른 특수문자의 입력에 대비하여 바이트 수를 계산하여 커서를 옮긴다
                    else
                        Console.SetCursorPosition(cursorLeft + inputString.Length, Console.CursorTop);
                }
            }

            // 이름을 전부 입력한 뒤 들어오는 키 입력에 대한 예외처리
            inputKey = Console.ReadKey();
            if (inputKey.Key != ConsoleKey.Enter)
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            else
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.Write("  ");

            return inputString;
        }

        public string AddressFormatInput(int cursorLeft)
        {
            int areaChoice = 0, choiceIndex = 0;
            string[] city = { "서울특별시", "인천광역시", "대전광역시", "대구광역시", "울산광역시", "부산광역시", "광주광역시" };
            string[] seoul = { "강서구", "양천구", "구로구", "마포구", "영등포구", "은평구", "서대문구", "종로구", "중구", "용산구", "동작구", "관악구", "도봉구", "강북구", "성북구", "동대문구", "성동구", "서초구", "노원구", "중랑구", "광진구", "강남구", "송파구", "강동구" };
            string[] incheon = { "중구", "동구", "남구", "연수구", "남동구", "부평구", "계양구", "서구", "강화군", "옹진군" };
            string[] daegeon = { "유성구", "서구", "중구", "대덕구", "동구" };
            string[] daegoo = { "남구", "달서구", "동구", "북구", "서구", "수성구", "중구" };
            string[] ulsan = { "남구", "동구", "북구", "중구" };
            string[] busan = { "중구", "서구", "동구", "영도구", "부산진구", "동래구", "남구", "북구", "해운대구", "사하구", "금정구", "강서구", "연제구", "수영구", "사상구", "기장군" };
            string[] gwangju = { "광산구", "남구", "동구", "북구", "서구" };
            string[][] districtSets = { seoul, incheon, daegeon, daegoo, ulsan, busan, gwangju };
            Hashtable districts = new Hashtable();
            // 지역 해시테이블 초기화
            for(int i=0;i<city.Length;i++)
                districts[city[i]] = districtSets[i];
            
            string inputString = "";

            Console.Write("서울특별시");

            // 시 선택
            while(areaChoice == 0)
            {
                switch (PressDirectionKey()) {
                    case UP:
                        // 윗 지역 선택
                        choiceIndex--;
                        // 인덱스 범위를 넘었다면
                        if (choiceIndex < 0)
                            choiceIndex = 0;
                        // 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 이전에 있던 글자 지움
                        Console.Write("                    ");
                        // 다시 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 선택된 도시 출력
                        Console.Write(city[choiceIndex]);
                        break;
                    case DOWN:
                        // 아랫 지역 선택
                        choiceIndex++;
                        // 인덱스 범위를 넘어서면
                        if (choiceIndex >= city.Length)
                            choiceIndex = city.Length - 1;
                        // 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 이전에 있던 글자 지움
                        Console.Write("                    ");
                        // 다시 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 선택된 도시 출력
                        Console.Write(city[choiceIndex]);
                        break;
                    case ENTER:
                        // 현재 선택된 도시 선택
                        inputString += city[choiceIndex] + " ";
                        // 띄어쓰기 출력
                        Console.Write(" ");
                        areaChoice = 1;
                        // 커서 시작 위치 변경
                        cursorLeft += Encoding.Default.GetBytes(inputString).Length;
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        break;
                    default:
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                        break;
                }
            }

            // 시 선택 저장
            string[] chosenCity = (string[])districts[city[choiceIndex]];
            choiceIndex = 0;
            Console.Write(chosenCity[0]);

            // 도 선택
            while(areaChoice == 1) {
                switch (PressDirectionKey())
                {
                    case UP:
                        // 윗 지역 선택
                        choiceIndex--;
                        // 인덱스 범위를 넘었다면
                        if (choiceIndex < 0)
                            choiceIndex = 0;
                        // 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 이전에 있던 글자 지움
                        Console.Write("                    ");
                        // 다시 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 선택된 도시 출력
                        Console.Write(chosenCity[choiceIndex]);
                        break;
                    case DOWN:
                        // 아랫 지역 선택
                        choiceIndex++;
                        // 인덱스 범위를 넘어서면
                        if (choiceIndex >= chosenCity.Length)
                            choiceIndex = chosenCity.Length - 1;
                        // 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 이전에 있던 글자 지움
                        Console.Write("                    ");
                        // 다시 입력 위치로 이동
                        Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        // 선택된 도시 출력
                        Console.Write(chosenCity[choiceIndex]);
                        break;
                    case ENTER:
                        // 현재 선택된 도시 선택
                        inputString += chosenCity[choiceIndex];
                        areaChoice = 2;
                        break;
                    default:
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                        break;
                }
            }
            return inputString;
        }
        
        // 전화번호 형식으로 입력받아 문자열을 반환하는 함수
        public string PhoneNumberFormatInput(int cursorLeft)
        {
            ConsoleKeyInfo inputKey;

            string inputString = "";
            while (inputString.Length < 13)
            {
                inputKey = Console.ReadKey();
                // 숫자면 문자열에 추가한다
                if (inputKey.KeyChar >= '0' && inputKey.KeyChar <= '9')
                {
                    inputString += inputKey.KeyChar;
                    if (inputString.Length == 3 || inputString.Length == 8)
                    {
                        Console.Write("-");
                        inputString += '-';
                    }
                }
                else
                {
                    // 특수키가 아닌 경우
                    if (inputKey.KeyChar >= 33 && inputKey.KeyChar <= 126)
                    {
                        // 잘못 입력된 문자의 바이트 수만큼 앞으로 이동
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                    else
                    {
                        // 백스페이스에 대한 예외처리
                        if (ConsoleKey.Backspace == inputKey.Key)
                        {
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                            if (Console.CursorLeft < cursorLeft)
                                Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                        }
                        // 한글과 다른 특수문자의 입력에 대비하여 바이트 수를 계산하여 커서를 옮긴다
                        else
                            Console.SetCursorPosition(cursorLeft + inputString.Length, Console.CursorTop);
                    }
                }
            }
            return inputString;
        }

        // 엔터 누르면 다른 화면으로 이동하게 하는 함수
        public void PressAnyKey()
        {
            while (true) {
                if (Console.ReadKey().KeyChar != 0) {  // 아무키나 누를 때까지 반복
                    Console.Clear();
                    break;
                }
            }
        }

    }
}
