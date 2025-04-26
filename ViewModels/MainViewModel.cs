using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Organizer.Models;
using Organizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Organizer.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly TaskRepository _repo = new();

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Today;

        [ObservableProperty]
        private string newTaskText = string.Empty;

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        public Action? RefreshTasks { get; set; }  // <<--- MUSI być public

        public MainViewModel()
        {
            // Przy starcie można załadować zadania jeśli chcesz
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            LoadTasks();
        }

        [RelayCommand]
        private void AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTaskText))
                return;

            try
            {
                var task = new TaskItem
                {
                    Text = NewTaskText,
                    Date = SelectedDate
                };

                _repo.SaveTask(task);
                Tasks.Add(task);
                NewTaskText = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy dodawaniu zadania: {ex.Message}");
            }
        }

        [RelayCommand]
        private void DeleteTask(TaskItem task)
        {
            if (task == null) return;

            try
            {
                _repo.DeleteTask(task.Id, task.Date);
                Tasks.Remove(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy usuwaniu zadania: {ex.Message}");
            }
        }

        private void LoadTasks()
        {
            Tasks.Clear();
            try
            {
                var tasks = _repo.GetTasksForDate(SelectedDate);

                if (tasks != null && tasks.Any())
                {
                    foreach (var task in tasks)
                    {
                        Tasks.Add(task);
                    }
                }
                else
                {
                    if (!_repo.DoesTasksFileExist(SelectedDate))
                    {
                        _repo.SaveTasksForDate(SelectedDate, new List<TaskItem>());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy ładowaniu zadań: {ex.Message}");
            }
        }

        [RelayCommand]
        private void OpenTasks()
        {
            LoadTasks();
        }

        public void RefreshCurrentDayTasks()
        {
            LoadTasks();
        }
    }
}
