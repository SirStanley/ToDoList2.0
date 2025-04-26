using Avalonia.Controls;
using Avalonia.Interactivity;
using Organizer.Models;
using Organizer.Services;
using Organizer.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.Views
{
    public partial class MainView : Window
    {
        private readonly TaskRepository _repo;
        private MainViewModel ViewModel => (MainViewModel)DataContext!;

        public MainView()
        {
            InitializeComponent();
            _repo = new TaskRepository();

            // £adowanie zadañ dla wybranej daty (domyœlnie dzisiaj)
            LoadTasksForSelectedDate(DateTime.Today);

            // Przypisanie odœwie¿enia zadañ do delegata
            ViewModel.RefreshTasks = ViewModel.RefreshCurrentDayTasks;
        }

        private void LoadTasksForSelectedDate(DateTime date)
        {
            var tasks = _repo.LoadTasksForDate(date);
            ViewModel.Tasks.Clear();
            foreach (var t in tasks)
                ViewModel.Tasks.Add(t);

            ViewModel.SelectedDate = date;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var text = NewTaskTextBox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                ViewModel.Tasks.Add(new TaskItem { Text = text, Date = ViewModel.SelectedDate });
                ViewModel.NewTaskText = string.Empty;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is TaskItem task)
            {
                ViewModel.Tasks.Remove(task);
            }
        }

        private async void SaveList_Click(object sender, RoutedEventArgs e)
        {
            var calendar = new CalendarWindow(ViewModel.Tasks.ToList(), _repo, ViewModel.RefreshCurrentDayTasks);
            await calendar.ShowDialog(this);
        }
    }
}
