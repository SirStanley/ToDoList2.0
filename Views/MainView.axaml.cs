using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;
using Organizer.Models;
using Organizer.ViewModels;


namespace Organizer.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }


        private async void SaveList_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                await vm.SaveTasksAsync();
        }

        private async void Add_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                await vm.AddTaskAsync();
        }

        private void ResetDate_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                vm.ResetToToday();
        }

        private async void Delete_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem task)
            {
                if (DataContext is MainViewModel vm)
                    await vm.DeleteTaskAsync(task);
            }
        }
    }
}
