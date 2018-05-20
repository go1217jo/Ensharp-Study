using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
   class Calculation
   {
      // 연산자에 따라 연산한다
      public double Operate(char oper, double operand1, double operand2)
      {
         switch(oper)
         {
            case '+':
               return operand1 + operand2;
            case '-':
               return operand1 - operand2;
            case 'x':
               return operand1 * operand2;
            case '÷':
               return operand1 / operand2;
            default:
               return 0.0;
         }
      }
      
      public double ReturnResult(string equation)
      {
         Stack<double> numberStack = new Stack<double>();
         Stack<char> operStack = new Stack<char>();

         double operand1, operand2;
         string[] splitBySpace = equation.Split(' ');

         for(int idx = 0; idx < splitBySpace.Length; idx++)
         {
            // 연산자
            if ((idx % 2) == 1)
            {
               // 연산자 스택이 비어있지 않으면
               if (operStack.Count != 0)
               {
                  // 스택의 Top에 있는 연산자가 우선순위가 높다면 스택에서 연산자를 빼내고 연산함
                  if (CompareOperationPriority(new char[] { operStack.Peek(), splitBySpace[idx][0] }) == 1)
                  {
                     operand2 = numberStack.Pop();
                     operand1 = numberStack.Pop();
                     numberStack.Push(Operate(operStack.Pop(), operand1, operand2));
                     operStack.Push(splitBySpace[idx][0]);
                  }
                  // 그렇지 않으면 스택에 넣는다
                  else
                     operStack.Push(splitBySpace[idx][0]);
               }
               else
                  operStack.Push(splitBySpace[idx][0]);
            }
            // 피연산자면
            else
            {
               if (splitBySpace[idx].Contains("negate"))
                  numberStack.Push(ProcessNegate(splitBySpace[idx]));
               else
                  numberStack.Push(double.Parse(splitBySpace[idx]));
            }
         }

         // 남아있는 연산자에 대해 연산을 진행한다.
         while(operStack.Count != 0)
         {
            operand2 = numberStack.Pop();
            operand1 = numberStack.Pop();
            numberStack.Push(Operate(operStack.Pop(), operand1, operand2));
         }

         return numberStack.Pop();
      }

      // 두 연산자의 우선순위를 비교한다.
      // 왼쪽 연산자가 우선순위가 더 높다면 1, 같다면 0, 낮다면 -1을 반환한다.
      public int CompareOperationPriority(char[] oper)
      {
         int[] priority = new int[2];

         // *, / 의 우선순위를 1, +, - 의 우선순위를 2라 한다
         for (int idx = 0; idx < 2; idx++) {
            if (oper[idx] == 'x' || oper[idx] == '÷')
            priority[idx] = 1;
            else
               priority[idx] = 2;
         }

         // 두 연산자 중 왼쪽 연산자가 우선순위가 더 높다면
         if (priority[1] > priority[0])
            return 1;
         else if (priority[1] == priority[0])
            return 0;
         else
            return -1;
      }

      // 피연산자 문자열을 숫자로 변환한다
      public double ConvertToOperand(string operand)
      {
         double number = 1.0;

         // negative 함수가 쓰여진 피연산자인 경우
         if (operand.Contains("negate"))
            number = ProcessNegate(operand);
         else
         {
            if (operand.Contains("."))
               number = double.Parse(operand);
            else
               number = (double)int.Parse(operand);
         }
         return number;
      }

      // negate가 포함된 문자열을 숫자로 변환
      public double ProcessNegate(string operand)
      {
         int negateCount = 0, sign = 1;
         int numberStartIndex = operand.LastIndexOf('(') + 1;
         int numberEndIndex = operand.IndexOf(')') - 1;
         // negate 함수 인수 값을 구함
         string number = operand.Substring(numberStartIndex, numberEndIndex - numberStartIndex + 1);
         
         // negate 개수를 구함
         while(operand.Contains("negate"))
         {
            // negate( 를 없앰
            operand = operand.Substring(7);
            negateCount++;
         }

         if ((negateCount % 2) == 1)
            sign = -1;

         if (number.Contains("."))
            return double.Parse(number) * sign;
         else
            return (double)int.Parse(number) * sign;
      }
   }
}
