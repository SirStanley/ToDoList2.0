using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Organizer.ViewModels
{
    public class SplashScreenViewModel : INotifyPropertyChanged
    {
        private string _startupMessage = "Starting application...";
        public string StartupMessage
        {
            get => _startupMessage;
            set
            {
                _startupMessage = value;
                OnPropertyChanged(nameof(StartupMessage));
            }
        }

        private double _progressValue;
        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public Action OnFinished { get; set; } = () => { };
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isLoading;

        public SplashScreenViewModel()
        {
            _ = StartLoading();
        }

        public async Task StartLoading()
        {
            if (_isLoading) return;
            _isLoading = true;

            try
            {
                for (int i = 0; i <= 100; i++)
                {
                    if (_cts.Token.IsCancellationRequested)
                        return;

                    ProgressValue = i;
                    StartupMessage = $"Loading... {i}%";
                    await Task.Delay(30, _cts.Token);
                }

                OnFinished?.Invoke();
            }
            catch (TaskCanceledException)
            {
                StartupMessage = "Loading cancelled.";
            }
            finally
            {
                _isLoading = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
