using System;
using System.Windows.Media.Imaging;

namespace PlayerCards.Model
{
    public class PlayerCardModel
    {
        public int CardId { get; set; }
        public int Edition { get; set; }
        public Uri Image { get; set; }
        public BitmapImage BitmapImage { get;set;}
    }
}
