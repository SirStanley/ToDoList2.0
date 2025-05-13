using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        // Pobiera zadania z pliku dla danego dnia
        public async Task<IEnumerable<TaskItem>> GetTasksForDateAsync(DateTime date)
        {
            string filePath = GetFilePath(date);

            // Jeśli plik nie istnieje, zwrócimy pustą kolekcję
            if (!File.Exists(filePath))
                return Enumerable.Empty<TaskItem>();

            var lines = await File.ReadAllLinesAsync(filePath);
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

        // Zapisuje zadanie do pliku
        public async Task SaveTaskAsync(TaskItem task)
        {
            var tasks = (await GetTasksForDateAsync(task.Date)).ToList();
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

            await SaveTasksToFileAsync(task.Date, tasks);
        }

        // Usuwa zadanie z pliku
        public async Task DeleteTaskAsync(Guid taskId, DateTime date)
        {
            var tasks = (await GetTasksForDateAsync(date)).Where(t => t.Id != taskId).ToList();
            await SaveTasksToFileAsync(date, tasks);
        }

        // Zapisuje zadania do pliku dla określonej daty
        public async Task SaveTasksForDateAsync(DateTime date, IEnumerable<TaskItem> tasks)
        {
            await SaveTasksToFileAsync(date, tasks);
        }

        // Zapisuje wszystkie zadania do pliku
        private async Task SaveTasksToFileAsync(DateTime date, IEnumerable<TaskItem> tasks)
        {
            string filePath = GetFilePath(date);

            // Tworzymy lub nadpisujemy plik z zadaniami
            var lines = tasks.Select(t => $"{t.Id}||{t.Text}||{t.IsDone}");
            await File.WriteAllLinesAsync(filePath, lines);
        }

        // Generuje ścieżkę do pliku dla zadanych dat
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
    }
}
