using System;
using System.Collections.Generic;
using System.IO;
using Lab7Variant7_IVT7;
using System.Diagnostics;
using System.Linq;
using Lab7Variant7_IVT1;
using System.Runtime.CompilerServices;

namespace Lab7Variant7_IVT7
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
                Console.WriteLine("---------------------");
                Console.WriteLine("7 - Отсортировать лог по времени");
                Console.WriteLine("8 - Сравнить алгоритмы сортировки");
                Console.WriteLine("9 - Выход");
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

                    //--------------------------------

                    case 7:
                        logFunctions.SortLogByTimestamp(log);
                        Console.WriteLine("Лог отсортирован по времени.");
                        break;
                    case 8:
                        OtherFunctions functions = new OtherFunctions();
                        functions.PerformSorting();
                        break;
                    case 9:
                        Console.WriteLine("Лабораторная 2");
                        Console.WriteLine("Функции с O(1): reshenie_diskriminant, okonchanice_slova, raznost_chisel, vse_delimie_chisla_na_5");
                        Console.WriteLine("Функции с O(n): fibbonachi_ryad, vichislenie_Pi, Factorial");
                        Console.WriteLine("O(k) и O(n^3) соотвественно: ryad_Taylora, combination_finder");
                        Console.WriteLine("-----------------");
                        Console.WriteLine("Лабораторная 3");
                        Console.WriteLine("O(1): MatrixCalculator, MatrixMultiplier, MatrixTask");
                        Console.WriteLine("O(n): ArrayMaker, CyclicArrayRotator, ArrayOperations, SymmetryChecker");
                        Console.WriteLine("O(n^2): ArrayRotator");
                        Console.WriteLine("O(2^n): рекурсивное вычисления чисел фиббоначи");
                        Console.WriteLine("O(n!): вычисления определителя с использованием разложения по строке");
                        Console.WriteLine("-----------------");
                        Console.WriteLine("Лабораторная 4");
                        Console.WriteLine("O(n): PrintAllSymbols_array, PrintWordOrder_array, ReverseSentence_array, Filter_Username_array, Filter_Username_regex, BreakDownMathematic, TextDeCypher, TextManager, SmileFinder ");
                        Console.WriteLine("O(m * L): FindDotCom_methods");
                        Console.WriteLine("O(m^2): CountTime");


                        return;
                    case 10:
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
