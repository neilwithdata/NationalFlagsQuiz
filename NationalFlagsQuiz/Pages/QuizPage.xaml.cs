using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Threading;

namespace NationalFlagsQuiz.Pages
{
    public partial class QuizPage : PhoneApplicationPage
    {
        private Quiz quiz;

        public QuizPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string continent = NavigationContext.QueryString["continent"];

            if (continent == "N.America")
                continent = "North America";
            else if (continent == "S.America")
                continent = "South America";

            quiz = new Quiz(continent);

            QuizProgressBar.Minimum = 0.0;
            QuizProgressBar.Maximum = quiz.CountryCount;

            updateQuestion();
        }

        private void updateQuestion()
        {
            Country c = quiz.NextCountry;

            if (c == null)
            {
                MessageBox.Show("Congratulations, your score was " + quiz.Score + "/" + quiz.CountryCount, "Quiz: " + quiz.Continent, MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            else
            {
                QuizFlagImg.Source = c.FlagImg;

                // Update the progress display
                QuizProgressBar.Value = quiz.Progress;

                // re-enable all the buttons
                Option1Button.IsEnabled = true;
                Option2Button.IsEnabled = true;
                Option3Button.IsEnabled = true;
                Option4Button.IsEnabled = true;

                // Hide all the guess indicators

                string[] guesses = quiz.GenerateGuesses();

                Option1Button.Content = guesses[0];
                Option2Button.Content = guesses[1];
                Option3Button.Content = guesses[2];
                Option4Button.Content = guesses[3];
            }
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            string selection = (string)((Button)sender).Content;

            if (quiz.IsAnswer(selection))
            {
                // Visual indicator user selected the right answer
                AnswerTextBlock.Text = selection;
                AnswerPanel.Opacity = 1.0;
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Thread.Sleep(1000);
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        AnswerPanel.Opacity = 0.0;
                        
                        // Swap to the next question
                        updateQuestion();

                        // Update the score display
                        ScoreTextBlock.Text = "score: " + quiz.ScoreText;

                    });
                });
            }
            else
            {
                // Update the score display
                ScoreTextBlock.Text = "score: " + quiz.ScoreText;

                // Disable the button
                (sender as Button).IsEnabled = false;
            }
        }
    }
}