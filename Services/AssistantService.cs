// C:\Users\St\MessengerWithTasks\MessengerApp\Services\AssistantService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MessengerApp.Models;

namespace MessengerApp.Services
{
    public class AssistantService
    {
        // Возвращает текст ответа и опциональную новую задачу для добавления
        public (string response, TaskItem? newTask) HandleQuery(string query, IEnumerable<string> recentMessages, IEnumerable<string> tasks)
        {
            if (string.IsNullOrWhiteSpace(query)) return ("Сформулируй запрос", null);
            var q = query.Trim();

            var m = Regex.Match(q, @"^(создай задачу|create task|add task)\s+(.+)$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                var title = m.Groups[2].Value.Trim();
                var task = new TaskItem { Title = title, IsDone = false };
                return ($"Создаю задачу: {title}", task);
            }

            if (q.IndexOf("суммируй", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var last = recentMessages?.TakeLast(6) ?? Enumerable.Empty<string>();
                var joined = string.Join(" | ", last);
                return ($"Резюме: " + (string.IsNullOrEmpty(joined) ? "нет сообщений" : joined), null);
            }

            if (q.IndexOf("напомни", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var nextTask = tasks?.FirstOrDefault() ?? "нет задач";
                return ($"Ближайшая задача: " + nextTask, null);
            }

            return ("Не понял запрос. Попробуй: 'Суммируй' или 'Создай задачу <текст>'.", null);
        }
    }
}
