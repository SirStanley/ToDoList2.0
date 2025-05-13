using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Organizer.Views;
using System;
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
            Console.WriteLine("Splash screen is displayed");

            // Czekamy 3 sekundy (symulacja ³adowania)
            await Task.Delay(5000);
            Console.WriteLine("Splash screen closing");

            splash.Close();

            // Utworzenie i wyœwietlenie MainView tylko raz
            var mainWindow = new MainView();
            desktop.MainWindow = mainWindow;
          

            Console.WriteLine("MainView is shown");
        }
    }
}
