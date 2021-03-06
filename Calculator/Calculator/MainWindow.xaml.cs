﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
   /// <summary>
   /// MainWindow.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class MainWindow : Window
   {
      string currentNumber = "";
      string currentEquation = "";
      // 가수부
      string manatissa = "";
      // 현재 입력되고 있는 숫자가 소수일지 아닐지의 상태
      bool floatState = false;
      // negate를 위한 문자열, 인덱스
      string negateNumber = "";
      int negateStartIndex = 0;
      string priorOperation = "";

      Outprocessor outprocessor = new Outprocessor();
      List<Button> numberButtons;
      List<Button> operButtons;
      Calculation calculation = new Calculation();

      public MainWindow()
      {
         InitializeComponent();
         // 버튼 객체들에 대한 이벤트 초기화
         numberButtons = new List<Button>(new Button[] { Btn_zero, Btn_one, Btn_two, Btn_three, Btn_four, Btn_five, Btn_six, Btn_seven, Btn_eight, Btn_nine });
         operButtons = new List<Button>(new Button[] { Btn_percentage, Btn_root, Btn_square, Btn_one_divide, Btn_divide, Btn_multiply, Btn_minus, Btn_plus });
         for (int idx = 0; idx < numberButtons.Count; idx++)
            numberButtons[idx].Click += new RoutedEventHandler(Btn_number_Click);
         for (int idx = 0; idx < operButtons.Count; idx++)
            operButtons[idx].Click += new RoutedEventHandler(Btn_operation_Click);
         this.ResizeMode = ResizeMode.NoResize;
      }

      // 0으로 나누는 수식이 있는지 확인한다.
      public bool IsDivideByZero()
      {
         if (currentEquation.Contains("÷ 0") && !floatState && !priorOperation.Contains("."))
            return true;
         else
            return false;
      }

      // 숫자 키패드를 누를 수 있도록 전환한다.
      public void EnableNumberButtons()
      {
         for (int idx = 0; idx < numberButtons.Count; idx++)
            numberButtons[idx].IsHitTestVisible = true;         
      }

      // 연산 키패드를 누를 수 있도록 전환한다.
      public void EnableOperationButtons()
      {
         for (int idx = 0; idx < operButtons.Count; idx++)
            operButtons[idx].IsHitTestVisible = true;
         Btn_enter.IsHitTestVisible = true;
         Btn_plusMinus.IsHitTestVisible = true;
         Btn_dot.IsHitTestVisible = true;
      }

      // 연산 키패드를 누를 수 없도록 한다.
      public void DisableOperationButtons()
      {
         for (int idx = 0; idx < operButtons.Count; idx++)
            operButtons[idx].IsHitTestVisible = false;
         Btn_enter.IsHitTestVisible = false;
         Btn_plusMinus.IsHitTestVisible = false;
         Btn_dot.IsHitTestVisible = false;
      }

      // 화면에 숫자가 다 출력되도록 텍스트 크기를 조절한다
      public void AdjustTextSize()
      {
         string manatissaStr = manatissa;
         string number = currentNumber;

         if (currentNumber.Length == 0)
         {
            string[] splits = calculationScreen.Text.Split('.');
            number = splits[0];
            if(splits.Length == 2)
               manatissaStr = '.' + splits[1];
         }

         int floatPartLength = (manatissaStr.Length - 1 >= 0) ? manatissaStr.Length - 1 : 0;
         int increaseRate = number.Length + floatPartLength - 12;

         if (increaseRate >= 1 && increaseRate < 4)
            calculationScreen.FontSize = Constant.BASIC_FONT_SIZE - (4.3 - increaseRate * 0.08) * increaseRate;
         // 입력 제한
         else if (increaseRate >= 4)
         {
            for (int idx = 0; idx < numberButtons.Count; idx++)
               numberButtons[idx].IsHitTestVisible = false;
            if(increaseRate != 4)
               calculationScreen.FontSize = Constant.BASIC_FONT_SIZE - increaseRate*2.2;
         }
         // 최대 출력 글자 수보다 작을 때
         else {
            calculationScreen.FontSize = Constant.BASIC_FONT_SIZE;
            if (!numberButtons[0].IsHitTestVisible)
               EnableNumberButtons();
         }
      }
      
      // 숫자 키패트 버튼 중 하나가 눌릴 경우
      private void Btn_number_Click(object sender, RoutedEventArgs e)
      {
         EnableOperationButtons();
         // 소수이면
         if (floatState)
            manatissa += ((Button)sender).Content;
         // 현재 소수가 아니면
         else
         {
            // 00으로 입력되는 것을 방지
            if(!(currentNumber.Length == 0 && ((Button)sender).Content.Equals("0")))
               currentNumber += ((Button)sender).Content;
         }
         calculationScreen.Text = outprocessor.InsertComma(currentNumber) + manatissa;
         if (calculationScreen.Text.Length == 0)
            calculationScreen.Text = "0";
         
         AdjustTextSize();
      }

      // 연산자 버튼 중 하나가 눌릴 경우
      private void Btn_operation_Click(object sender, RoutedEventArgs e)
      {
         // 연산자 누른 뒤 바로 또 연산자를 누를 경우를 배제
         if(currentNumber.Length != 0)
            Btn_enter_Click(sender, e);
                           
         // 아무것도 입력된 수가 없으면 화면의 값에 대한 연산
         if (currentNumber.Length == 0 && negateNumber.Length == 0 && resultScreen.Text.Length == 0)
            currentNumber = calculationScreen.Text;
         
         // 현재 입력된 버튼의 연산 문자를 가져옴
         char oper = ((Button)sender).Content.ToString()[0];
         if (currentNumber.Length == 0 && negateNumber.Length == 0)
            currentEquation = currentEquation.Remove(currentEquation.Length - 3);

         // 소수점만 남아있을 경우
         if (manatissa.Length == 1)
            manatissa = "";
                
         switch (oper)
         {
            case '+':
            case '-':
            case '÷':
               currentEquation += (currentNumber + manatissa + ' ' + oper + ' ');
               break;
            case 'X':
               currentEquation += (currentNumber + manatissa + " x ");
               break;
         }

         floatState = false;
         if (currentEquation.Length >= 28)
            resultScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
         resultScreen.Text = currentEquation;

         // 0으로 나누는 수식이 있을 경우
         if(IsDivideByZero())
         {
            Btn_cancel_all_Click(sender, e);
            calculationScreen.Text = "0으로 나눌 수 없습니다.";
         }

         currentNumber = "";
         manatissa = "";
         negateNumber = "";
         EnableNumberButtons();
      }
           
      // CE버튼
      private void Btn_cancel_Click(object sender, RoutedEventArgs e)
      {
         currentNumber = "";
         manatissa = "";
         negateNumber = "";
         floatState = false;
         EnableOperationButtons();
         calculationScreen.Text = "0";
         AdjustTextSize();
      }

      // C 버튼
      private void Btn_cancel_all_Click(object sender, RoutedEventArgs e)
      {
         currentNumber = "";
         currentEquation = "";
         manatissa = "";
         negateNumber = "";
         floatState = false;
         resultScreen.Text = "";
         calculationScreen.Text = "0";
         priorOperation = "";
         EnableOperationButtons();
         AdjustTextSize();
         resultScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
      }

      // 플러스마이너스 버튼을 누르면 발생하는 이벤트, negate에 대한 처리
      private void Btn_plusMinus_Click(object sender, RoutedEventArgs e)
      {
         string origin = "";

         if(currentNumber.Length != 0)
         {
            if (currentNumber[0] == '-')
               currentNumber = currentNumber.Substring(1);
            else
               currentNumber = '-' + currentNumber;
            calculationScreen.Text = outprocessor.InsertComma(currentNumber) + manatissa;
         }
         else
         {
            origin = calculationScreen.Text;
            if (!calculationScreen.Text.Equals("0"))
            {
               if (calculationScreen.Text[0] == '-')
                  calculationScreen.Text = calculationScreen.Text.Substring(1);
               else
                  calculationScreen.Text = '-' + calculationScreen.Text;
            }
            if (negateNumber.Length == 0)
            {
               negateNumber = "negate(" + origin + ")";
               negateStartIndex = currentEquation.Length;
               currentEquation += negateNumber;
            }
            else
            {
               negateNumber = "negate(" + negateNumber + ")";
               currentEquation = currentEquation.Remove(negateStartIndex) + negateNumber;
            }
               
            resultScreen.Text = currentEquation;
            
            if (currentEquation.Length >= 28)
               resultScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
         }
      }

      // 백스페이스 버튼
      private void Btn_erase_Click(object sender, RoutedEventArgs e)
      {
         if (!numberButtons[0].IsHitTestVisible)
            EnableNumberButtons();
         // 현재 소수면
         if (floatState)
         {
            manatissa = manatissa.Remove(manatissa.Length - 1);
            // 소수점이 지워지면 정수상태
            if (manatissa.Length == 0)
               floatState = false;
            calculationScreen.Text = outprocessor.InsertComma(currentNumber) + manatissa;
         }
         else
         {
            if (currentNumber.Length != 0)
            {
               currentNumber = currentNumber.Remove(currentNumber.Length - 1);
               if (currentNumber.Length == 0)
                  calculationScreen.Text = "0";
               else
                  calculationScreen.Text = outprocessor.InsertComma(currentNumber);
            }
         }
         AdjustTextSize();
      }

      // 점 버튼
      private void Btn_dot_Click(object sender, RoutedEventArgs e)
      {
         if (floatState)
            return;
         // 지수부가 없으면 0으로 결정
         if (currentNumber.Length == 0)
            currentNumber += '0';
         manatissa += '.';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber) + manatissa;
         floatState = true;
      }

      private void Btn_enter_Click(object sender, RoutedEventArgs e)
      {
         string originEquation = resultScreen.Text;
         string[] splits = currentEquation.Split(' ');

         // 일반적인 피연산자1 + 연산자 + 피연산자2 수식
         if (currentNumber.Length != 0)
            currentEquation += currentNumber + manatissa;
         // =만 계속 입력하는 경우
         else if (currentNumber.Length == 0 && currentEquation.Length == 0)
            currentEquation = calculationScreen.Text + priorOperation;
         // 마지막 피연산자가 negate인 경우
         else if (negateNumber.Length != 0)
            priorOperation = ' ' + splits[splits.Length - 2] + ' ' + negateNumber;
         // 피연산자1 + 연산자 수식인 경우
         else
            currentEquation += calculationScreen.Text;

         //  연산자 + 피연산자2 부분을 저장해둠
         if (splits.Length - 2 >= 0 && negateNumber.Length == 0 && !priorOperation.Contains("negate"))
            priorOperation = ' ' + splits[splits.Length - 2] + ' ' + calculationScreen.Text;

         // 0으로 나누는 수식이 있을 경우
         if (IsDivideByZero())
         {
            Btn_cancel_all_Click(sender, e);
            DisableOperationButtons();
            calculationScreen.Text = "0으로 못 나눔";
         }
         else
         {
            // 연산 결과 반환
            double result = calculation.ReturnResult(currentEquation);
            
            calculationScreen.Text = result.ToString();

            // 연산 결과 길이가 길면 텍스트 크기를 조절한다.
            int increaseRate = outprocessor.DeleteComma(calculationScreen.Text).Length - 12;
            if (increaseRate >= 1)
               calculationScreen.FontSize = Constant.BASIC_FONT_SIZE - (4.3 - increaseRate * 0.3) * increaseRate;

            // 중간 연산일 경우에는 초기화하지 않음
            if (((Button)sender).Content.Equals("="))
            {
               currentNumber = "";
               resultScreen.Text = "";
               currentEquation = "";
               manatissa = "";
               floatState = false;
               negateNumber = "";
            }
            else 
               currentEquation = originEquation;

            
         }
      }
   }
}
