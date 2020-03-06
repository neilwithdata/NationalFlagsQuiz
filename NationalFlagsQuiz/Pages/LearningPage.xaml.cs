using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Threading;
using System.Collections.Generic;
using System.Windows;

namespace NationalFlagsQuiz.Pages
{
    public partial class LearningPage : PhoneApplicationPage
    {
        public LearningPage()
        {
            InitializeComponent();
            PopulateListBoxes();
        }

        private void PopulateListBoxes()
        {
            string[] continents = { "africa", "asia", "europe", "north america", "south america", "oceania" };

            ThreadPool.QueueUserWorkItem((o) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Populate the "all" list box first
                    ListBox allListBox = (ListBox)((PanoramaItem)ContinentsPanorama.Items[0]).Content;
                    foreach (Country c in App.Current.CountryList)
                        allListBox.Items.Add(c);
                });

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Now populate all the other list boxes sequentially
                    for (int i = 0; i < continents.Length; i++)
                    {
                        var continentQuery =
                            from country in App.Current.CountryList
                            where country.Continent.Equals(continents[i], StringComparison.CurrentCultureIgnoreCase)
                            select country;


                        ListBox listBox = (ListBox)((PanoramaItem)ContinentsPanorama.Items[i + 1]).Content;

                        foreach (Country c in continentQuery)
                            listBox.Items.Add(c);
                    }
                });
            });
        }
    }
}