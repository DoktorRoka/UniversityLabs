using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Laba9_variant7_Part1
{
    public class TableFunctions
    {
        public void ViewTable(DoublyLinkedList<Participant> participants)
        {
            Console.WriteLine("\nСостав спортклуба:");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine($"| {"№",2} | {"ФИО",-20} | {"Тип",-3} | {"Год рождения",-12} | {"Опыт (лет)",-10} |");
            Console.WriteLine("---------------------------------------------------------------");

            int idx = 1;
            foreach (var participant in participants)
            {
                Console.WriteLine(
                    $"| {idx,2} | {participant.FullName,-20} | {participant.Type,-3} | {participant.BirthYear,-12} | {participant.Experience,-10} |"
                );
                idx++;
            }

            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Перечисляемый тип: Т - тренер, С - спортсмен");
        }

        public void AddRecord(
            DoublyLinkedList<Participant> participants,
            List<LogEntry> log,
            ref DateTime lastActionTime,
            LogFunctions logFunctions)
        {
            var participant = new Participant();

            Console.Write("Введите ФИО: ");
            participant.FullName = Console.ReadLine();

            Console.Write("Введите тип (Т - тренер, С - спортсмен): ");
            participant.Type = char.ToUpper(Console.ReadLine()[0]);

            Console.Write("Введите год рождения: ");
            participant.BirthYear = int.Parse(Console.ReadLine());

            Console.Write("Введите опыт (лет): ");
            participant.Experience = int.Parse(Console.ReadLine());

            participants.AddLast(participant);
            logFunctions.LogAction(log, "Добавлена запись", participant.FullName, ref lastActionTime);
        }

        public void DeleteRecord(
            DoublyLinkedList<Participant> participants,
            List<LogEntry> log,
            ref DateTime lastActionTime,
            LogFunctions logFunctions)
        {
            Console.Write("Введите номер записи для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int index)
                && index >= 1
                && index <= participants.Count)
            {
                var removed = participants.GetAt(index - 1);
                participants.RemoveAt(index - 1);
                logFunctions.LogAction(log, "Удалена запись", removed.FullName, ref lastActionTime);
            }
            else
            {
                Console.WriteLine("Некорректный номер записи.");
            }
        }

        public void UpdateRecord(
            DoublyLinkedList<Participant> participants,
            List<LogEntry> log,
            ref DateTime lastActionTime,
            LogFunctions logFunctions)
        {
            Console.Write("Введите номер записи для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int index)
                && index >= 1
                && index <= participants.Count)
            {
                // Получаем текущий участник
                var participant = participants.GetAt(index - 1);

                Console.Write("Введите новое ФИО: ");
                participant.FullName = Console.ReadLine();

                Console.Write("Введите новый тип (Т - тренер, С - спортсмен): ");
                participant.Type = char.ToUpper(Console.ReadLine()[0]);

                Console.Write("Введите новый год рождения: ");
                participant.BirthYear = int.Parse(Console.ReadLine());

                Console.Write("Введите новый опыт (лет): ");
                participant.Experience = int.Parse(Console.ReadLine());

                participants.SetAt(index - 1, participant);
                logFunctions.LogAction(log, "Обновлена запись", participant.FullName, ref lastActionTime);
            }
            else
            {
                Console.WriteLine("Некорректный номер записи.");
            }
        }

        public void SearchRecords(DoublyLinkedList<Participant> participants)
        {
            Console.Write("Введите минимальный год рождения для поиска: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Некорректный ввод года.");
                return;
            }

            var results = new DoublyLinkedList<Participant>();
            foreach (var p in participants)
            {
                if (p.BirthYear > year)
                    results.AddLast(p);
            }

            if (results.Count > 0)
            {
                ViewTable(results);
            }
            else
            {
                Console.WriteLine("Нет записей, соответствующих критерию.");
            }
        }

        public DoublyLinkedList<Participant> LoadParticipants(string filePath)
        {
            var participants = new DoublyLinkedList<Participant>();
            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    participants.AddLast(Participant.FromString(line));
                }
            }
            return participants;
        }

        public void SaveParticipants(DoublyLinkedList<Participant> participants, string filePath)
        {
            var lines = new List<string>(participants.Count);
            foreach (var p in participants)
            {
                lines.Add(p.ToString());
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
