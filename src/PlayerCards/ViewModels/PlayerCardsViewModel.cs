using PlayerCards.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;

namespace PlayerCards.ViewModels
{
    public class PlayerCardsViewModel : BindableBase
    {
        private ObservableCollection<PlayerCardModel> _playerCards = new ObservableCollection<PlayerCardModel>();
        public DelegateCommand GetNewCardCommand { get; private set; }
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ObservableCollection<PlayerCardModel> PlayerCards
        {
            get; private set;
        } 

        public ObservableCollection<string> PlayerTexts { get; private set; }
        public PlayerCardsViewModel()
        {
            PlayerCards = new ObservableCollection<PlayerCardModel>();
            PlayerTexts = new ObservableCollection<string>();
            GetNewCardCommand = new DelegateCommand(GetNewCard, CanGetNewCard);
            Message = "View A from your Prism Module";
            string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyPath = Path.GetDirectoryName(fullPath);
        }

        public bool CanGetNewCard()
        {
            return true;
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public async void GetNewCard()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44324/");
                var request = new HttpRequestMessage(HttpMethod.Get, "api/Card");
                var responseMessage = await client.SendAsync(request);
                var byteArray = await responseMessage.Content.ReadAsByteArrayAsync();

                string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string assemblyPath = Path.GetDirectoryName(fullPath);
                PlayerCards.Add(new PlayerCardModel() { CardId = 2, Edition = 1, BitmapImage = LoadImage(byteArray) });
            }
        }
    }
}
