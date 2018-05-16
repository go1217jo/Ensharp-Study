using System;
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

      Outprocessor outprocessor = new Outprocessor();
      List<Button> numberButtons;
      List<Button> operButtons;

      public MainWindow()
      {
         InitializeComponent();
         numberButtons = new List<Button>(new Button[] { Btn_zero, Btn_one, Btn_two, Btn_three, Btn_four, Btn_five, Btn_six, Btn_seven, Btn_eight, Btn_nine });
         operButtons = new List<Button>(new Button[] { Btn_percentage, Btn_root, Btn_square, Btn_one_divide, Btn_divide, Btn_multiply, Btn_minus, Btn_plus, Btn_enter });
         for (int idx = 0; idx < numberButtons.Count; idx++)
            numberButtons[idx].Click += new RoutedEventHandler(Btn_number_Click);
         for (int idx = 0; idx < operButtons.Count; idx++)
            operButtons[idx].Click += new RoutedEventHandler(Btn_operation_Click);
      }

      // 화면에 숫자가 다 출력되도록 텍스트 크기를 조절한다
      public void AdjustTextSize()
      {
         int increaseRate = currentNumber.Length - 12;
         if (increaseRate >= 1 && increaseRate < 4)
            calculationScreen.FontSize = Constant.BASIC_FONT_SIZE - (4.3 - increaseRate * 0.08) * increaseRate;
         // 입력 제한
         else if (increaseRate >= 4)
         {
            for (int idx = 0; idx < numberButtons.Count; idx++)
               numberButtons[idx].IsHitTestVisible = false;
         }
         else {
            calculationScreen.FontSize = Constant.BASIC_FONT_SIZE;
            if (!numberButtons[0].IsHitTestVisible) {
               for (int idx = 0; idx < numberButtons.Count; idx++)
                  numberButtons[idx].IsHitTestVisible = true;
            }
         }
      }

      private void Btn_number_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += ((Button)sender).Content;
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
         AdjustTextSize();
      }

      private void Btn_operation_Click(object sender, RoutedEventArgs e)
      {
         // 현재 입력된 버튼의 연산 문자를 가져옴
         char oper = ((Button)sender).Content.ToString()[0];
         if (currentNumber.Length == 0)
            currentEquation = currentEquation.Remove(currentEquation.Length - 1);
         switch (oper)
         {
            case '+':
            case '-':
               currentEquation += (currentNumber + oper);
               break;
            case 'X':
               currentEquation += (currentNumber + 'x');
               break;
            case '÷':
               currentEquation += (currentNumber + '/');
               break;
            case '√':
               currentEquation += ("√(" + currentNumber + ')');
               break;
         }

         resultScreen.Text = currentEquation;
         currentNumber = "";
      }
           
      private void Btn_cancel_Click(object sender, RoutedEventArgs e)
      {
         currentNumber = "";
         calculationScreen.Text = "0";
         AdjustTextSize();
      }

      private void Btn_cancel_all_Click(object sender, RoutedEventArgs e)
      {
         currentNumber = "";
         currentEquation = "";
         resultScreen.Text = "";
         calculationScreen.Text = "0";
         AdjustTextSize();
      }

      private void Btn_plusMinus_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_erase_Click(object sender, RoutedEventArgs e)
      {
         if (currentNumber.Length != 0) {
            currentNumber = currentNumber.Remove(currentNumber.Length - 1);
            if (currentNumber.Length == 0)
               calculationScreen.Text = "0";
            else
               calculationScreen.Text = outprocessor.InsertComma(currentNumber);
         }
      }

      private void Btn_dot_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void Btn_enter_Click(object sender, RoutedEventArgs e)
      {

      }
   }
}
