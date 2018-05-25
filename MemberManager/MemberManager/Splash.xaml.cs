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
using System.Windows.Shapes;
using System.Timers;

namespace MemberManager
{
    /// <summary>
    /// Splash.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
            this.AddHandler(MouseUpEvent, new RoutedEventHandler(SplashMove));
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        
        private void SplashMove(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
