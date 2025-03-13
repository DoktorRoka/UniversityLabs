using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6Var7_IVT1_2semestr
{
    public class TableFunctions
    {
        public void ViewTable(List<Participant> participants)
        {
            Console.WriteLine("\nСостав спортклуба:");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("| ФИО            | Тип | Год рождения | Опыт (лет) |");
            Console.WriteLine("-------------------------------------------------");

            foreach (var participant in participants)
            {
                Console.WriteLine($"| {participant.FullName,-14} | {participant.Type,-3} | {participant.BirthYear,-12} | {participant.Experience,-10} |");
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Перечисляемый тип: Т - тренер, С - спортсмен");
        }

        public void AddRecord(List<Participant> participants, List<LogEntry> log, ref DateTime lastActionTime, LogFunctions logFunctions)
        {
            Participant participant = new Participant();

            Console.Write("Введите ФИО: ");
            participant.FullName = Console.ReadLine();

            Console.Write("Введите тип (Т - тренер, С - спортсмен): ");
            participant.Type = char.ToUpper(Console.ReadLine()[0]);

            Console.Write("Введите год рождения: ");
            participant.BirthYear = int.Parse(Console.ReadLine());

            Console.Write("Введите опыт (лет): ");
            participant.Experience = int.Parse(Console.ReadLine());

            participants.Add(participant);
            logFunctions.LogAction(log, "Добавлена запись", participant.FullName, ref lastActionTime);
        }

        public void DeleteRecord(List<Participant> participants, List<LogEntry> log, ref DateTime lastActionTime, LogFunctions logFunctions)
        {
            Console.Write("Введите номер записи для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= participants.Count)
            {
                var removed = participants[index - 1];
                participants.RemoveAt(index - 1);
                logFunctions.LogAction(log, "Удалена запись", removed.FullName, ref lastActionTime);
            }
            else
            {
                Console.WriteLine("Некорректный номер записи.");
            }
        }

        public void UpdateRecord(List<Participant> participants, List<LogEntry> log, ref DateTime lastActionTime, LogFunctions logFunctions)
        {
            Console.Write("Введите номер записи для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= participants.Count)
            {
                Participant participant = participants[index - 1];

                Console.Write("Введите новое ФИО: ");
                participant.FullName = Console.ReadLine();

                Console.Write("Введите новый тип (Т - тренер, С - спортсмен): ");
                participant.Type = char.ToUpper(Console.ReadLine()[0]);

                Console.Write("Введите новый год рождения: ");
                participant.BirthYear = int.Parse(Console.ReadLine());

                Console.Write("Введите новый опыт (лет): ");
                participant.Experience = int.Parse(Console.ReadLine());

                participants[index - 1] = participant;
                logFunctions.LogAction(log, "Обновлена запись", participant.FullName, ref lastActionTime);
            }
            else
            {
                Console.WriteLine("Некорректный номер записи.");
            }
        }

        public void SearchRecords(List<Participant> participants)
        {
            Console.Write("Введите минимальный год рождения для поиска: ");
            int year = int.Parse(Console.ReadLine());

            var results = participants.Where(p => p.BirthYear > year).ToList();

            if (results.Any())
            {
                ViewTable(results);
            }
            else
            {
                Console.WriteLine("Нет записей, соответствующих критерию.");
            }
        }

        public void LoadData(string filePath, out List<Participant> participants, out List<LogEntry> log)
        {
            participants = new List<Participant>();
            log = new List<LogEntry>();

            if (!File.Exists(filePath))
                return;

            string currentSection = "";
            foreach (var line in File.ReadAllLines(filePath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    if (trimmed.Equals("[Participants]", StringComparison.OrdinalIgnoreCase))
                        currentSection = "participants";
                    else if (trimmed.Equals("[Log]", StringComparison.OrdinalIgnoreCase))
                        currentSection = "log";
                    else
                        currentSection = "";
                    continue;
                }
                if (string.IsNullOrEmpty(trimmed))
                    continue;

                if (currentSection == "participants")
                {
                    try
                    {
                        participants.Add(Participant.FromString(trimmed));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при загрузке участника: " + ex.Message);
                    }
                }
                else if (currentSection == "log")
                {
                    // Ожидается формат: "2025-03-10 22:56:06 - Добавлена запись: Максим"
                    var parts = trimmed.Split(" - ", StringSplitOptions.None);
                    if (parts.Length != 2 || !DateTime.TryParse(parts[0], out DateTime timestamp))
                    {
                        Console.WriteLine("Неверный формат лога: " + trimmed);
                        continue;
                    }
                    var subParts = parts[1].Split(": ", StringSplitOptions.None);
                    if (subParts.Length < 2)
                    {
                        Console.WriteLine("Неверный формат лога: " + trimmed);
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
        }

        public void SaveData(string filePath, List<Participant> participants, List<LogEntry> log)
        {
            List<string> lines = new List<string>();

            lines.Add("[Participants]");
            foreach (var participant in participants)
            {
                lines.Add(participant.ToString());
            }
            lines.Add(""); 

            lines.Add("[Log]");
            foreach (var entry in log)
            {
                lines.Add(entry.ToString());
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
