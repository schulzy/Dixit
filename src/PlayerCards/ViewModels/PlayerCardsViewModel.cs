using Dixit.PlayerCards.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Dixit.PlayerCards.ViewModels
{
    public class PlayerCardsViewModel : BindableBase
    {
        private ObservableCollection<PlayerCardModel> _playerCards = new ObservableCollection<PlayerCardModel>();
        private string _message;
        private bool _canGetNewCard = true;
        public DelegateCommand GetNewCardCommand { get; private set; }
        public DelegateCommand<PlayerCardModel> DeleteCardCommand { get; private set; }

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
            DeleteCardCommand = new DelegateCommand<PlayerCardModel>(DeleteCard, CanDeleteCard);
            Message = "View A from your Prism Module";
            string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyPath = Path.GetDirectoryName(fullPath);
        }

        private bool CanDeleteCard(PlayerCardModel parameter) => true;
        

        private void DeleteCard(PlayerCardModel parameter)
        {
            PlayerCards.Remove(parameter);
        }

        public bool CanGetNewCard()
        {
            if (PlayerCards.Count >= 6)
                return false;

            return _canGetNewCard;
        }

        public async void GetNewCard()
        {
            _canGetNewCard = false;
            GetNewCardCommand.RaiseCanExecuteChanged();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44324/");
                var request = new HttpRequestMessage(HttpMethod.Get, "api/Card");
                var responseMessage = await client.SendAsync(request);
                var byteArray = await responseMessage.Content.ReadAsByteArrayAsync();
                var image = LoadImage(byteArray);
                PlayerCards.Add(new PlayerCardModel() { BitmapImage = image, HashCode = GetHashSHA1(byteArray) });
            }
            _canGetNewCard = true;
            GetNewCardCommand.RaiseCanExecuteChanged();
        }

        private string GetHashSHA1(byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
            }
        }

        /// <summary>
        /// Create <see cref="BitmapImage"/> from <see cref="byte[]"/>
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
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
    }
}
