using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Organizer.Views;
using System.Threading.Tasks;

namespace Organizer
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ShowSplashScreen(desktop);
            }

            base.OnFrameworkInitializationCompleted();
        }

        private async void ShowSplashScreen(IClassicDesktopStyleApplicationLifetime desktop)
        {
            var splash = new SplashView();
            splash.Show();

            // Loading Simulation
            await Task.Delay(6000);

            splash.Close();

            // MainView Opening
            var mainWindow = new MainView();
            desktop.MainWindow = mainWindow;
          
        }
    }
}
