using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Threading;
using Organizer.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

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

            this.Opened += async (_, __) => await FadeInIconsAsync();
        }

        private async Task FadeInIconsAsync()
        {
            await FadeInIcon(Icon,  300);
            await FadeInIcon(Icon0, 300);
            await FadeInIcon(Icon1, 300);
            await FadeInIcon(Icon2, 300);
            await FadeInIcon(Icon3, 300);
            
            

        }

        private async Task FadeInIcon(Visual icon, int delayMs)
        {
            await Task.Delay(delayMs);

            var animation = new Animation
            {
                Duration = TimeSpan.FromMilliseconds(300),
                Easing = new CubicEaseOut(),
                FillMode = FillMode.Forward,
                Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0d),
                Setters = { new Setter(Visual.OpacityProperty, 0d) }
            },
            new KeyFrame
            {
                Cue = new Cue(1d),
                Setters = { new Setter(Visual.OpacityProperty, 1d) }
            }
        }
            };

            await animation.RunAsync(icon, CancellationToken.None);
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
