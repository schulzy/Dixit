using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace PlayerCards.Model
{
    public class PlayerCardModel
    {
        public int CardId { get; set; }
        public int Edition { get; set; }
        public Uri Image { get; set; }
        public BitmapFrame BitmapFrame {get;set;}
    }
}
