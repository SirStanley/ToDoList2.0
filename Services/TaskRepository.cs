using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Organizer.Models;

namespace Organizer.Services
{
    public class TaskRepository
    {
        private const string FolderPath = "Tasks";  // Folder w którym będą przechowywane pliki z zadaniami

        public TaskRepository()
        {
            // Tworzymy folder "Tasks", jeśli jeszcze nie istnieje
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
        }

        public IEnumerable<TaskItem> GetTasksForDate(DateTime date)
        {
            string filePath = GetFilePath(date);

            // Jeśli plik nie istnieje, zwrócimy pustą kolekcję
            if (!File.Exists(filePath))
                return Enumerable.Empty<TaskItem>();

            var lines = File.ReadAllLines(filePath);
            var tasks = new List<TaskItem>();

            // Czytamy plik i parsujemy linie na zadania
            foreach (var line in lines)
            {
                var parts = line.Split("||");
                if (parts.Length == 3 &&
                    Guid.TryParse(parts[0], out var id) &&
                    bool.TryParse(parts[2], out var isDone))
                {
                    tasks.Add(new TaskItem
                    {
                        Id = id,
                        Text = parts[1],
                        Date = date,
                        IsDone = isDone
                    });
                }
            }

            return tasks;
        }
        public bool DoesTasksFileExist(DateTime date)
        {
            var path = GetFilePath(date);
            return File.Exists(path);
        }

        public void SaveTask(TaskItem task)
        {
            var tasks = GetTasksForDate(task.Date).ToList();
            var existing = tasks.FirstOrDefault(t => t.Id == task.Id);

            if (existing != null)
            {
                // Jeśli zadanie już istnieje, aktualizujemy je
                existing.Text = task.Text;
                existing.IsDone = task.IsDone;
            }
            else
            {
                // Jeśli zadanie nie istnieje, dodajemy nowe
                tasks.Add(task);
            }

            SaveTasksToFile(task.Date, tasks);
        }

        public void DeleteTask(Guid taskId, DateTime date)
        {
            var tasks = GetTasksForDate(date).Where(t => t.Id != taskId).ToList();
            SaveTasksToFile(date, tasks);
        }

        private void SaveTasksToFile(DateTime date, IEnumerable<TaskItem> tasks)
        {
            string filePath = GetFilePath(date);

            // Tworzymy lub nadpisujemy plik z zadaniami
            var lines = tasks.Select(t => $"{t.Id}||{t.Text}||{t.IsDone}");
            File.WriteAllLines(filePath, lines);
        }

        private string GetFilePath(DateTime date)
        {
            // Ścieżka do folderu "Tasks" w katalogu uruchomienia aplikacji (np. bin/Debug/net8.0)
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderPath);

            // Tworzymy folder "Tasks", jeśli jeszcze nie istnieje
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Generujemy pełną ścieżkę do pliku dla danego dnia
            string fileName = date.ToString("yyyy-MM-dd") + ".txt";
            return Path.Combine(folderPath, fileName);
        }

        // 🔧 Metody dla MainView.axaml.cs i CalendarWindow, aby obsługiwać zadania dla konkretnej daty
        public IEnumerable<TaskItem> LoadTasksForDate(DateTime date)
        {
            return GetTasksForDate(date);
        }

        public void SaveTasksForDate(DateTime date, IEnumerable<TaskItem> tasks)
        {
            SaveTasksToFile(date, tasks);
        }
    }
}
