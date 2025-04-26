using Avalonia.Controls;
using Avalonia.Interactivity;
using Organizer.Models;
using Organizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Organizer.Views
{
    public partial class CalendarWindow : Window
    {
        private IEnumerable<TaskItem> _tasks = Array.Empty<TaskItem>();
        private TaskRepository _repo = new TaskRepository();
        private Action? _onTasksSaved;  // Delegat powiadamiający o zapisaniu zadań

        // Konstruktor bezparametrowy wymagany przez Avalonię
        public CalendarWindow()
        {
            InitializeComponent();
            DatePicker.SelectedDate = new DateTimeOffset(DateTime.Today); // Domyślna data
        }

        // Konstruktor z delegatem i parametrami
        public CalendarWindow(IEnumerable<TaskItem> tasks, TaskRepository repo, Action? onTasksSaved)
            : this() // Wywołanie domyślnego konstruktora
        {
            _tasks = tasks;
            _repo = repo;
            _onTasksSaved = onTasksSaved; // Przechowywanie delegata
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var dateOffset = DatePicker.SelectedDate ?? new DateTimeOffset(DateTime.Today);
            var date = dateOffset.Date; // DateTime
            _repo.SaveTasksForDate(date, _tasks);

            // Wywołanie delegata po zapisaniu
            _onTasksSaved?.Invoke();

            Close();
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dateOffset = DatePicker.SelectedDate ?? new DateTimeOffset(DateTime.Today);
            var date = dateOffset.Date;

            // Ładowanie zadań dla wybranego dnia
            _tasks = _repo.GetTasksForDate(date).ToList();

            // Jeśli brak zadań, tworzymy nowy plik (jeśli nie istnieje)
            if (!_tasks.Any())
            {
                _repo.SaveTasksForDate(date, _tasks);
            }

            // Wywołanie delegata (np. MainViewModel odświeży listę)
            _onTasksSaved?.Invoke();

            
            Close();
        }





        private void RefreshTaskView()
        {
            // Zaktualizowanie widoku zadań w kalendarzu (jeśli używasz odpowiednich kontrolek do wyświetlania)
            // Możesz dodać logikę, aby odświeżyć UI, np. pobierając dane z _tasks i pokazując je w odpowiednich kontrolkach.
            // Może to być np. lista kontrolek "TaskItem" w UI.
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
