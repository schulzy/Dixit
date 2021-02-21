using Prism.Mvvm;

namespace PlayerCards.ViewModels
{
    public class PlayerCardsViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public PlayerCardsViewModel()
        {
            Message = "View A from your Prism Module";
        }
    }
}
