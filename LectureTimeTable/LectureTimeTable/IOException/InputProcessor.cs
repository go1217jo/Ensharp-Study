using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureTimeTable.IOException
{
    class InputProcessor
    {
        public const int LEFT = 4;
        public const int UP = 8;
        public const int RIGHT = 6;
        public const int DOWN = 5;
        public const int ENTER = 0;
        public const int ESC = -1;

        // 방향키 이외의 입력들에 대해 예외처리를 하며 방향키가 들어오면 설정해놓은 상수를 반환한다.
        public int PressDirectionKey()
        {
            ConsoleKeyInfo inputKey;
            while (true)
            {
                inputKey = Console.ReadKey(true);
                switch (inputKey.Key)
                {
                    case ConsoleKey.Escape:
                        return ESC;
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
                        break;
                }
            }
        }
        
        // 방향키에 따라 위 아래로만 움직이게 하고 엔터를 치면 true를 반환하는 함수
        public bool ChoiceByKey()
        {
            switch (PressDirectionKey())
            {
                case InputProcessor.UP:
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    return false;
                case InputProcessor.DOWN:
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
                    return false;
                case InputProcessor.ENTER:
                    Console.Clear();
                    return true;
                default:
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
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
                    Console.SetCursorPosition(cursorLeft, cursorTop - 1);
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
                    Console.SetCursorPosition(cursorLeft, cursorTop - 1);
                    Console.Write("                                ");
                    Console.SetCursorPosition(cursorLeft, cursorTop - 1);
                    continue;
                }
                else
                {
                    bool possible = false;
                    bool noBlank = true;

                    // 공백문자만 입력된 경우
                    for (int i = 0; i < input.Length; i++)
                    {
                        if (input[i] != '\n' && input[i] != '\t' && input[i] != ' ')  // 공백 문자가 아닌 문자가 있다면
                            possible = true;
                        else
                        {
                            // 공백 문자를 허용하지 않는다면
                            if (blankLimit)
                            {
                                // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                                Console.SetCursorPosition(5, screenHeight - 2);
                                Console.WriteLine("잘못된 입력 : 공백이 입력됨");
                                Console.SetCursorPosition(cursorLeft, cursorTop - 1);
                                Console.Write("                                ");
                                Console.SetCursorPosition(cursorLeft, cursorTop - 1);
                                noBlank = false;
                                break;
                            }
                        }
                    }
                    // 공백 문자만 입력되었을 경우 다시 입력 받음
                    if (!possible)
                    {
                        // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                        Console.SetCursorPosition(5, screenHeight - 2);
                        Console.WriteLine("잘못된 입력 : 공백만 입력됨");
                        Console.SetCursorPosition(cursorLeft, cursorTop - 1);
                        Console.Write("                                ");
                        Console.SetCursorPosition(cursorLeft, cursorTop - 1);
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
        public string InputPassword(int letterLimit, int screenHeight, int cursorLeft, int cursorTop)
        {
            string input = "";
            ConsoleKeyInfo passwordKey;

            while (true)
            {
                // password 입력받음 * 표시
                while (true)
                {
                    passwordKey = Console.ReadKey(true);
                    if (passwordKey.Key == ConsoleKey.Escape)
                        return null;
                    if (passwordKey.Key == ConsoleKey.Enter)
                        break;
                    if (passwordKey.KeyChar >= 33 && passwordKey.KeyChar <= 126)
                    {
                        if (input.Length < letterLimit) {
                            Console.Write("*");
                            input += passwordKey.KeyChar;
                        }
                    }
                    if (passwordKey.Key == ConsoleKey.Backspace)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        if (input.Length > 0)
                            input = input.Remove(input.Length - 1);
                        if (cursorLeft > Console.CursorLeft)
                            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    }
                }
                                
                // 입력되지 않은 경우 다시 입력
                if (input.Length == 0)
                {
                    // 입력되면서 화면에 표시되었던 문자열을 지우고 커서를 다시 위치시킨다.
                    Console.SetCursorPosition(5, screenHeight - 2);
                    Console.WriteLine("잘못된 입력 : 입력되지 않음");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("                 ");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    continue;
                }
                
                // 입력된 문자열을 반환한다.
                return input;
            }
        }

        // 학번 형식으로 입력받아 문자열을 반환하는 함수
        public string InputStudentNoFormat(int cursorLeft)
        {
            string inputString = "";
            ConsoleKeyInfo inputKey;

            while (inputString.Length < 8)
            {
                inputKey = Console.ReadKey(true);
                // ESC가 입력되면 탈출함
                if (inputKey.Key == ConsoleKey.Escape)
                    return null;
                if (inputKey.KeyChar >= '0' && inputKey.KeyChar <= '9') {
                    inputString += inputKey.KeyChar;
                    Console.Write(inputKey.KeyChar);
                    
                }
                else
                {
                    // 백스페이스에 대한 예외처리
                    if (inputKey.Key == ConsoleKey.Backspace)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        // 삭제한 문자 제거
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

            return inputString;
        }
    }
}
