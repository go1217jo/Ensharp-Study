using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace PageTransition
{
    /// <summary>
    /// Interaction logic for PageTransitor.xaml
    /// </summary>
    public partial class PageTransitor : UserControl
    {
        ContentControl currentPresenter;
        ContentControl newPresenter;
        Storyboard slideDown;
        Storyboard slideLeft;

        public PageTransitor()
        {
            InitializeComponent();

            slideDown = Resources["SlideDown"] as Storyboard;
            slideLeft = Resources["SlideLeft"] as Storyboard;

            currentPresenter = Presenter2;
        }


        public void SlideDown(object newPage)
        {
            if (currentPresenter.Content == null)
            {
                //On start of application, currentPresenter content will be null, so no need to slide.
                currentPresenter.Content = newPage;
                return;
            }

            if (currentPresenter == Presenter1)
            {
                newPresenter = Presenter2;
            }
            else
            {
                newPresenter = Presenter1;
            }

            Storyboard.SetTarget(slideDown.Children[0], newPresenter);
            Storyboard.SetTarget(slideDown.Children[1], currentPresenter);


            newPresenter.Visibility = System.Windows.Visibility.Visible;
            newPresenter.Content = newPage;


            slideDown.Begin();
        }

        public void SlideLeft(object newPage)
        {
            if (currentPresenter.Content == null)
            {
                //On start of application, currentPresenter content will be null, so no need to slide.
                currentPresenter.Content = newPage;
                return;
            }

            if (currentPresenter == Presenter1)
            {
                newPresenter = Presenter2;
            }
            else
            {
                newPresenter = Presenter1;
            }

            Storyboard.SetTarget(slideLeft.Children[0], currentPresenter);
            Storyboard.SetTarget(slideLeft.Children[1], newPresenter);
            newPresenter.Visibility = System.Windows.Visibility.Visible;
            newPresenter.Content = newPage;


            slideLeft.Begin();
        }

        private void SlideDown_Completed(object sender, EventArgs e)
        {

            if (currentPresenter == Presenter2)
                Panel.SetZIndex(Presenter1, 1);
            else
                Panel.SetZIndex(Presenter1, 0);

            (currentPresenter.RenderTransform as TranslateTransform).Y = 0;

            currentPresenter.Visibility = System.Windows.Visibility.Collapsed;
            currentPresenter = newPresenter;
        }

        private void SlideLeft_Completed(object sender, EventArgs e)
        {
            if (currentPresenter == Presenter2)
                Panel.SetZIndex(Presenter1, 1);
            else
                Panel.SetZIndex(Presenter1, 0);


            (currentPresenter.RenderTransform as TranslateTransform).X = 0;
            currentPresenter.Visibility = System.Windows.Visibility.Collapsed;

            //Change the current presenter to new page which can be used in next transit operation.
            currentPresenter = newPresenter;
        } 
    }
}
