using System;
using System.Windows.Media.Imaging;

namespace NationalFlagsQuiz
{
    public class Country
    {
        private const String FlagFolder = "/Data/flags/";

        public String Name { get; set; }
        public String CountryCode { get; set; }
        public String Continent { get; set; }
        public String FlagImgName { get; set; }

        private BitmapImage flagImg;
        public BitmapImage FlagImg
        {
            get { return flagImg; }
        }

        public void LoadFlagImg()
        {
            flagImg = new BitmapImage(new Uri(FlagFolder + FlagImgName, UriKind.Relative));
        }
    }
}
