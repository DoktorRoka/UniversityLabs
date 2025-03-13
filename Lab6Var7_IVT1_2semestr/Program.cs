using System;
using System.Collections.Generic;
using Lab6Var7_IVT1_2semestr;
using static Lab6Var7_IVT1_2semestr.OtherFunctions;

namespace Lab6Var7_IVT1_2semestr
{
    class Program
    {
        private const string DataFile = "lab.dat";

        static void Main(string[] args)
        {
            var tableFunctions = new TableFunctions();
            var logFunctions = new LogFunctions();

            List<Participant> participants;
            List<LogEntry> log;
            tableFunctions.LoadData(DataFile, out participants, out log);
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
                // ---------------------------------
                Console.WriteLine("-------------------------");
                Console.WriteLine("8 - Работа с бинарным файлом (7 вариант)");
                Console.WriteLine("9 - Работа с текстовым файлом (7 вариант)");
                Console.WriteLine("10 - Создать бэкап lab.dat");
                Console.WriteLine("11 - Анализатор .bmp");





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
                        tableFunctions.SaveData(DataFile, participants, log);
                        break;
                    case 3:
                        tableFunctions.DeleteRecord(participants, log, ref lastActionTime, logFunctions);
                        tableFunctions.SaveData(DataFile, participants, log);
                        break;
                    case 4:
                        tableFunctions.UpdateRecord(participants, log, ref lastActionTime, logFunctions);
                        tableFunctions.SaveData(DataFile, participants, log);
                        break;
                    case 5:
                        tableFunctions.SearchRecords(participants);
                        break;
                    case 6:
                        logFunctions.ViewLog(log, lastActionTime);
                        break;
                    case 7:
                        Console.WriteLine("Выход из программы.");
                        tableFunctions.SaveData(DataFile, participants, log);
                        return;
                    case 8:
                        BinaryFileHandler handler = new BinaryFileHandler();
                        handler.ProcessBinaryFiles("file1.bin", "file2.bin");
                        Console.WriteLine("Бинарные файлы успешно обработаны.");
                        handler.ViewFileContents("file1.bin", "file2.bin");
                        break;
                    case 9:
                        TextFileProcessor processor = new TextFileProcessor();
                        processor.ProcessTextFile("input.txt", "output.txt");

                        break;
                    case 10:
                        LabFileProcessor copyer = new LabFileProcessor();
                        copyer.ProcessLabFile();
                        Console.WriteLine("Операция завершена. Нажмите любую клавишу для выхода.");
                        Console.ReadKey();
                        break;
                    case 11:

                        Console.Write("Введите имя bmp-файла: ");
                        string fileName = Console.ReadLine();

                        BmpFileInfo bmpInfo = new BmpFileInfo();
                        bmpInfo.ReadBmpHeader(fileName);

                        Console.WriteLine("\nНажмите любую клавишу для выхода...");
                        Console.ReadKey();

                        break;
                    default:
                        Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
