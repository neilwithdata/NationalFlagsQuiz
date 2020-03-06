using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace NationalFlagsQuiz
{
    public class Quiz
    {
        private List<Country> countries;
        private int currCountryIndx;

        public String Continent { get; set; }
        public int Score { get; set; }
        private bool firstGuess;

        public Country NextCountry
        {
            get
            {
                if (HasNextCountry)
                {
                    firstGuess = true;
                    return countries[++currCountryIndx];
                }
                else return null;
            }
        }

        public string ScoreText
        {
            get
            {
                if (firstGuess)
                    return Score + "/" + currCountryIndx;
                else 
                    return Score + "/" + (currCountryIndx + 1);
            }
        }

        public int Progress
        {
            get { return currCountryIndx + 1; }
        }

        public bool HasNextCountry
        {
            get
            {
                return (currCountryIndx < CountryCount - 1);
            }
        }

        public bool IsAnswer(string guess)
        {
            if (guess == countries[currCountryIndx].Name)
            {
                if (firstGuess) Score++;
                return true;
            }
            else
            {
                firstGuess = false;
                return false;
            }
        }

        public int CountryCount
        {
            get { return countries.Count; }
        }

        public string[] GenerateGuesses()
        {
            // Create a new country list with the answer country removed
            List<Country> newList = new List<Country>(countries);
            newList.RemoveAt(currCountryIndx);

            // Randomize the list
            Random rnd = new Random();
            newList = newList.OrderBy<Country, int>((item) => rnd.Next()).ToList();

            // grab the first four entries
            string[] guesses = new string[] {
                newList.ElementAt(0).Name,
                newList.ElementAt(1).Name,
                newList.ElementAt(2).Name,
                newList.ElementAt(3).Name
            };

            // replace one of the four entries (randomly) with the correct answer
            guesses[new Random().Next(0, 4)] = countries[currCountryIndx].Name;

            return guesses;
        }

        public Quiz(string continent)
        {
            this.Continent = continent;
            currCountryIndx = -1;
            firstGuess = true;

            IEnumerable<Country> continentQueryResult;

            if (continent.Equals("all", StringComparison.CurrentCultureIgnoreCase))
                continentQueryResult = App.Current.CountryList;
            else
            {
                continentQueryResult =
                    from country in App.Current.CountryList
                    where country.Continent.Equals(continent, StringComparison.CurrentCultureIgnoreCase)
                    select country;
            }

            // Randomize the country list
            Random rnd = new Random();
            countries = continentQueryResult.OrderBy<Country, int>((item) => rnd.Next()).ToList();
        }
    }
}
