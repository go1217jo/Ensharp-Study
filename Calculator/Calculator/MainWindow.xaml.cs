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

      Outprocessor outprocessor = new Outprocessor();
      public MainWindow()
      {
         InitializeComponent();
        
      }

      private void Btn_percentage_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void Btn_root_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_square_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_one_divide_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_cancel_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_cancel_all_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_erase_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_divide_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_seven_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '7';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_eight_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '8';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_nine_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '9';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_multiply_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_four_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '4';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_five_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '5';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_six_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '6';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_minus_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_one_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '1';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_two_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '2';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_three_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '3';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_plus_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_plusMinus_Click(object sender, RoutedEventArgs e)
      {

      }

      private void Btn_zero_Click(object sender, RoutedEventArgs e)
      {
         currentNumber += '0';
         calculationScreen.Text = outprocessor.InsertComma(currentNumber);
      }

      private void Btn_dot_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void Btn_enter_Click(object sender, RoutedEventArgs e)
      {

      }
   }
}
