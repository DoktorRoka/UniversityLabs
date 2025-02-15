using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifthLaboratoryIVT7
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

        public List<Participant> LoadParticipants(string filePath)
        {
            var participants = new List<Participant>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    participants.Add(Participant.FromString(line));
                }
            }
            return participants;
        }

        public void SaveParticipants(List<Participant> participants, string filePath)
        {
            File.WriteAllLines(filePath, participants.Select(p => p.ToString()));
        }
    }
}
