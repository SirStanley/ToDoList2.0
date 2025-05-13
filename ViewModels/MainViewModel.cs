using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Organizer.Models;
using Organizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly TaskRepository _repo = new();

        [ObservableProperty]
        private DateTimeOffset? selectedDate = DateTimeOffset.Now;

        [ObservableProperty]
        private string newTaskText = string.Empty;

        [ObservableProperty]
        private string loadingMessage = string.Empty;

        [ObservableProperty]
        private int progress = 0;

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        public MainViewModel()
        {
            LoadTasksAsync().ConfigureAwait(false);
        }

        partial void OnSelectedDateChanged(DateTimeOffset? value)
        {
            if (value != null)
            {
                LoadTasksAsync().ConfigureAwait(false);
            }
        }

        [RelayCommand]
        public async Task AddTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(NewTaskText)) return;

            if (SelectedDate.HasValue)
            {
                var task = new TaskItem
                {
                    Text = NewTaskText,
                    Date = SelectedDate.Value.DateTime
                };

                await _repo.SaveTaskAsync(task);
                Tasks.Add(task);
                NewTaskText = string.Empty;
            }
        }

        [RelayCommand]
        public async Task DeleteTaskAsync(TaskItem task)
        {
            if (task == null) return;

            if (SelectedDate.HasValue)
            {
                await _repo.DeleteTaskAsync(task.Id, SelectedDate.Value.DateTime);
                Tasks.Remove(task);
            }
        }

        [RelayCommand]
        public async Task LoadTasksAsync()
        {
            Tasks.Clear();
            if (SelectedDate.HasValue)
            {
                var tasks = await _repo.GetTasksForDateAsync(SelectedDate.Value.DateTime);
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
        }

        [RelayCommand]
        public async Task SaveTasksAsync()
        {
            if (SelectedDate.HasValue)
            {
                await _repo.SaveTasksForDateAsync(SelectedDate.Value.DateTime, Tasks.ToList());
            }
        }

        [RelayCommand]
        public void ResetToToday()
        {
            SelectedDate = DateTimeOffset.Now;
        }
    }
}
