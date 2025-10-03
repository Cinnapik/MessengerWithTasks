using MessengerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MessengerApp.Services
{
    public class AssistantService
    {
        public (string response, TaskItem? newTask) HandleQuery(string query, IEnumerable<string> recentMessages, IEnumerable<string> tasks)
        {
            if (string.IsNullOrWhiteSpace(query)) return ("Сформулируй запрос", null);
            var q = query.Trim();

            var m = Regex.Match(q, @"создай задачу\s+(.+)", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                var title = m.Groups[1].Value.Trim();
                return ($"Создаю задачу: {title}", new TaskItem { Title = title, IsDone = false });
            }

            if (q.ToLowerInvariant().Contains("суммируй"))
            {
                var last = recentMessages?.TakeLast(6) ?? Enumerable.Empty<string>();
                var joined = string.Join(" | ", last);
                return ($"Резюме: " + (string.IsNullOrEmpty(joined) ? "нет сообщений" : joined), null);
            }

            if (q.ToLowerInvariant().Contains("напомни"))
            {
                var next = tasks?.FirstOrDefault() ?? "нет задач";
                return ($"Ближайшая задача: " + next, null);
            }

            return ("Не понял запрос. Попробуй: 'Создай задачу <текст>' или 'Суммируй'.", null);
        }
    }
}
