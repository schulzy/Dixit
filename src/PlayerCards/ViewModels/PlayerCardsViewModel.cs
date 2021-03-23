using PlayerCards.Model;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace PlayerCards.ViewModels
{
    public class PlayerCardsViewModel : BindableBase
    {
        private ObservableCollection<PlayerCardModel> _playerCards = new ObservableCollection<PlayerCardModel>();

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ObservableCollection<PlayerCardModel> PlayerCards
        {
            get { return _playerCards; }
            set { SetProperty(ref _playerCards, value); }
        }

        public PlayerCardsViewModel()
        {
            Message = "View A from your Prism Module";
            string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyPath = Path.GetDirectoryName(fullPath);
            _playerCards.Add(new PlayerCardModel() { CardId = 1, Edition = 1, Image = new System.Uri(Path.Combine(assemblyPath, "Assets/dixit_0034.jpg")) });
            _playerCards.Add(new PlayerCardModel() { CardId = 2, Edition = 1, Image = new System.Uri(Path.Combine(assemblyPath, "Assets/hatlap.jpg")) });
        }
    }
}
