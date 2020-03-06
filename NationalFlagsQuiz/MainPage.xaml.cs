using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace NationalFlagsQuiz
{
    public partial class MainPage : PhoneApplicationPage
    {
        Storyboard aboutInStoryboard;
        Storyboard aboutOutStoryboard;
        Popup aboutPopup;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            initDisplay();
            initAboutPopup();
        }

        private void initAboutPopup()
        {
            aboutPopup = new Popup()
            {
                Child = new AboutDialog(),
                HorizontalOffset = 10,
                IsOpen = false
            };

            aboutInStoryboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 800,
                To = 560,
                Duration = new System.Windows.Duration(TimeSpan.FromMilliseconds(250))
            };

            Storyboard.SetTarget(animation, aboutPopup);
            Storyboard.SetTargetProperty(animation, new PropertyPath("UIElement.VerticalOffset"));

            aboutInStoryboard.Children.Add(animation);

            aboutOutStoryboard = new Storyboard();
            DoubleAnimation outAnimation = new DoubleAnimation()
            {
                From = 560,
                To = 800,
                Duration = new System.Windows.Duration(TimeSpan.FromMilliseconds(250))
            };

            Storyboard.SetTarget(outAnimation, aboutPopup);
            Storyboard.SetTargetProperty(outAnimation, new PropertyPath("UIElement.VerticalOffset"));

            aboutOutStoryboard.Children.Add(outAnimation);
        }

        private void initDisplay()
        {
            // Kick off start animations
            BackgroundStoryboard.Begin();
            UserSelectionStoryboard.Begin();
            AboutInStoryboard.Begin();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (aboutPopup.IsOpen)
            {
                aboutOutStoryboard.Begin();
                aboutOutStoryboard.Completed += (s, args) => aboutPopup.IsOpen = false;
                e.Cancel = true;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Return the "Learn" text back to it's original position in case it has changed due to animation effect
            if (LearnSlideOut.X >= 480)
                LearnSlideOut.X -= 480;

            if (TakeQuizSlideOut.X >= 480)
                TakeQuizSlideOut.X -= 480;

            base.OnNavigatedFrom(e);
        }

        private void About_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AboutScaleStoryboard.Begin();
        }

        private void TakeQuiz_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TakeQuizSlideOutStoryboard.Begin();
            TakeQuizSlideOutStoryboard.Completed += (s, args) => NavigationService.Navigate(new Uri("/Pages/SelectContinentPage.xaml", UriKind.Relative));
        }

        private void Learn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LearnSlideOutStoryboard.Begin();
            LearnSlideOutStoryboard.Completed += (s, args) => NavigationService.Navigate(new Uri("/Pages/LearningPage.xaml", UriKind.Relative));
        }

        private void About_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            aboutPopup.IsOpen = true;
            aboutInStoryboard.Begin();
        }
    }
}