using System;
using System.Collections.Generic;
using System.IO;
using Laba9_variant7_Part2;

namespace Laba9_variant7_Part2
{
    class Program
    {
        private const string ParticipantsFile = "participants.txt";
        private const string LogFile = "log.txt";

        static void Main(string[] args)
        {
            var tableFunctions = new TableFunctions();
            var logFunctions = new LogFunctions();

            List<Participant> participants = tableFunctions.LoadParticipants(ParticipantsFile);
            List<LogEntry> log = logFunctions.LoadLog(LogFile);
            DateTime lastActionTime = DateTime.Now;

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1 - Просмотр таблицы");
                Console.WriteLine("2 - Добавить запись");
                Console.WriteLine("3 - Удалить запись");
                Console.WriteLine("4 - Обновить запись");
                Console.WriteLine("5 - Поиск записей");
                Console.WriteLine("6 - Просмотреть лог");
                Console.WriteLine("7 - Выход");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        tableFunctions.ViewTable(participants);
                        break;
                    case 2:
                        tableFunctions.AddRecord(participants, log, ref lastActionTime, logFunctions);
                        tableFunctions.SaveParticipants(participants, ParticipantsFile);
                        logFunctions.SaveLog(log, LogFile);
                        break;
                    case 3:
                        tableFunctions.DeleteRecord(participants, log, ref lastActionTime, logFunctions);
                        tableFunctions.SaveParticipants(participants, ParticipantsFile);
                        logFunctions.SaveLog(log, LogFile);
                        break;
                    case 4:
                        tableFunctions.UpdateRecord(participants, log, ref lastActionTime, logFunctions);
                        tableFunctions.SaveParticipants(participants, ParticipantsFile);
                        logFunctions.SaveLog(log, LogFile);
                        break;
                    case 5:
                        tableFunctions.SearchRecords(participants);
                        break;
                    case 6:
                        logFunctions.ViewLog(log, lastActionTime);
                        break;
                    case 7:
                        Console.WriteLine("Выход из программы.");
                        return;
                    default:
                        Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
