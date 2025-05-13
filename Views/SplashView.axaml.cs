using Avalonia.Controls;
using Avalonia.Threading;
using Organizer.ViewModels;

namespace Organizer.Views
{
    public partial class SplashView : Window
    {
        public SplashView()
        {
            InitializeComponent();

            if (DataContext is not SplashScreenViewModel viewModel)
            {
                viewModel = new SplashScreenViewModel();
                DataContext = viewModel;
            }

            viewModel.OnFinished = ShowMainView;
        }

        private void ShowMainView()
        {
            Dispatcher.UIThread.Post(() =>
            {
                var mainView = new MainView();
                mainView.Show();
                this.Close();
            });
        }
    }
}
