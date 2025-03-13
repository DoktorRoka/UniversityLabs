using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6Var7_IVT1_2semestr
{
    public class LogFunctions
    {
        public void ViewLog(List<LogEntry> log, DateTime lastActionTime)
        {
            foreach (var entry in log)
            {
                Console.WriteLine(entry);
            }

            Console.WriteLine("\nСамый долгий период бездействия пользователя:");
            Console.WriteLine((DateTime.Now - lastActionTime).ToString("hh\\:mm\\:ss"));
        }

        public void LogAction(List<LogEntry> log, string action, string recordInfo, ref DateTime lastActionTime)
        {
            log.Add(new LogEntry
            {
                Timestamp = DateTime.Now,
                Action = action,
                RecordInfo = recordInfo
            });

            if (log.Count > 50) log.RemoveAt(0);
            lastActionTime = DateTime.Now;
        }

        public List<LogEntry> LoadLog(string filePath)
        {
            var log = new List<LogEntry>();
            if (!File.Exists(filePath))
                return log;

            string currentSection = "";
            foreach (var line in File.ReadAllLines(filePath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    if (trimmed.Equals("[Log]", StringComparison.OrdinalIgnoreCase))
                        currentSection = "log";
                    else
                        currentSection = "";
                    continue;
                }
                if (string.IsNullOrEmpty(trimmed))
                    continue;

                if (currentSection == "log")
                {
                    var parts = trimmed.Split(" - ", StringSplitOptions.None);
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Неверный формат строки: " + trimmed);
                        continue;
                    }
                    if (!DateTime.TryParse(parts[0], out DateTime timestamp))
                    {
                        Console.WriteLine("Неверный формат даты: " + parts[0]);
                        continue;
                    }
                    var subParts = parts[1].Split(": ", StringSplitOptions.None);
                    if (subParts.Length < 2)
                    {
                        Console.WriteLine("Неверный формат действия и данных: " + parts[1]);
                        continue;
                    }
                    string action = subParts[0];
                    string recordInfo = string.Join(": ", subParts.Skip(1));

                    log.Add(new LogEntry
                    {
                        Timestamp = timestamp,
                        Action = action,
                        RecordInfo = recordInfo
                    });
                }
            }
            return log;
        }

        public void SaveLog(List<LogEntry> log, string filePath)
        {
            List<string> allLines = new List<string>();

            if (File.Exists(filePath))
            {
                allLines = File.ReadAllLines(filePath).ToList();
            }

            int logSectionIndex = allLines.FindIndex(line => line.Trim().Equals("[Log]", StringComparison.OrdinalIgnoreCase));
            if (logSectionIndex >= 0)
            {
                allLines = allLines.Take(logSectionIndex).ToList();
            }

            allLines.Add("[Log]");
            foreach (var entry in log)
            {
                allLines.Add(entry.ToString());
            }

            File.WriteAllLines(filePath, allLines);
        }
    }
}
