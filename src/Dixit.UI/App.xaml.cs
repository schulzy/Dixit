using Dixit.UI.Views;
using PlayerCards.Views;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace Dixit.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("PlayerCards", typeof(PlayerCardsView));
        }
    }
}
