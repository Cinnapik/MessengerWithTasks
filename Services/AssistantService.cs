// C:\Users\St\MessengerWithTasks\MessengerApp\Services\AssistantService.cs
// Простой контекстный помощник на ключевых словах
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApp.Services
{
    public class AssistantService
    {
        public string GetResponse(string query, IEnumerable<string> recentMessages, IEnumerable<string> tasks)
        {
            if (string.IsNullOrWhiteSpace(query)) return "Сформулируй запрос";
            var q = query.ToLowerInvariant();
            if (q.Contains("суммируй") || q.Contains("резюме"))
            {
                var last = recentMessages?.TakeLast(6) ?? Enumerable.Empty<string>();
                var joined = string.Join(" | ", last);
                return "Краткое резюме последних сообщений: " + (string.IsNullOrEmpty(joined) ? "нет сообщений" : joined);
            }
            if (q.Contains("напомни") || q.Contains("напоминание"))
            {
                var nextTask = tasks?.FirstOrDefault() ?? "нет задач";
                return "Ближайшая задача: " + nextTask;
            }
            if (q.StartsWith("создай задачу") || q.StartsWith("create task") || q.StartsWith("add task"))
            {
                var parts = query.Split(' ', 3);
                var title = parts.Length >= 3 ? parts[2] : "Новая задача";
                return $"Распознано: создать задачу '{title}'. Добавь задачу в раздел Tasks";
            }
            return "Не понял запрос. Попробуй 'Суммируй' или 'Напомни'.";
        }
    }
}
