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

namespace MemberManager
{
   /// <summary>
   /// MainWindow.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class MainWindow : Window
   {
      LoginScreen login;
      FindID findID;
      FindPW findPW;
      Registration register;

      public MainWindow()
      {
         InitializeComponent();
         init();
         MainGrid.Children.Add(login);
      }

      public void init()
      {
         login = new LoginScreen();
         findID = new FindID();
         findPW = new FindPW();
         register = new Registration();

         login.findID.AddHandler(MouseDownEvent, new RoutedEventHandler(Label_findID_Click));
         login.findPW.AddHandler(MouseDownEvent, new RoutedEventHandler(Label_findPW_Click));
         login.register.AddHandler(MouseDownEvent, new RoutedEventHandler(Label_register_Click));

         findID.Btn_Back_FindID.Click += Btn_Back_Click;
         findPW.Btn_Back_FindPW.Click += Btn_Back_Click;
         findPW.findID_Click.AddHandler(MouseDownEvent, new RoutedEventHandler(MoveToFindID));
      }

      private void Label_findID_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(findID);
      }

      private void Label_findPW_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(findPW);
      }

      private void Label_register_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(register);
      }

      private void Btn_Back_Click(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(login);
      }

      private void MoveToFindID(object sender, RoutedEventArgs e)
      {
         MainGrid.Children.Clear();
         MainGrid.Children.Add(findID);
      }
   }
}
