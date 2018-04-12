using System;
using System.Collections.Generic;
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

        public int PressDirectionKey()
        {
            while (true)
            {
                switch (Console.ReadKey().Key)
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
                        break;
                    default:
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        break;
                }
            }
        }

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

        public string ReadAndCheckString(int letterLimit, int screenHeight, int cursorLeft, int cursorTop, bool blankLimit)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();

                // 입력된 문자열 길이가 제한 수를 넘어간다면
                if (input.Length > letterLimit)
                {
                    Console.SetCursorPosition(5, screenHeight - 2);
                    Console.WriteLine("잘못된 입력 : 글자 수 제한");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("                        ");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    continue;
                }

                // 입력되지 않은 경우 다시 입력
                if (input.Length == 0)
                {
                    Console.SetCursorPosition(5, screenHeight - 2);
                    Console.WriteLine("잘못된 입력 : 입력되지 않음");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write("                        ");
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    continue;
                }
                else
                {
                    // 공백 제한이 있다면 공백 체크를 한다.
                    if (blankLimit)
                    {
                        bool possible = true;
                        // 문자열 내에 공백이 포함되어 있는지 확인
                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i] == '\n' || input[i] == '\t' || input[i] == ' ')
                            { // 공백 문자가 포함된다면
                                Console.SetCursorPosition(5, screenHeight - 2);
                                Console.WriteLine("잘못된 입력 : 공백이 입력됨");
                                Console.SetCursorPosition(cursorLeft, cursorTop);
                                Console.Write("                        ");
                                Console.SetCursorPosition(cursorLeft, cursorTop);
                                possible = false;
                                break;
                            }
                        }
                        // 문자열 중간에 공백문자가 포함되어 있으면 다시 입력 받음
                        if (!possible)
                            continue;
                    }
                }
                return input;
            }
        }

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
