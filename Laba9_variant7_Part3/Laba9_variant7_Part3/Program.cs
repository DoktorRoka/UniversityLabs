using System;

namespace Laba9_variant7_Part3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("1) Проверка мат. выражения через стек");
            Console.WriteLine("2) Игра Считалка");
            Console.WriteLine("3) Переборка чисел у которых комбинаций суммы кубов ≥ 3");
            Console.WriteLine("4) Часто встречаемые слова");
            Console.WriteLine("5) Плей-офф команд");
            Console.WriteLine("--------------------");
            Console.WriteLine("6) Добавить в список элементы другого в обратном порядке");
            Console.WriteLine("7) Код для обработки разреженных матриц");

            Console.WriteLine();
            Console.Write("Выберите пункт меню (1–7): ");

            int myChoice = int.Parse(Console.ReadLine());

            Console.WriteLine(); // пустая строка перед выводом результата

            switch (myChoice)
            {
                case 1:
                    var lab = new LabFunc();
                    lab.MatVirazh();
                    break;

                case 2:
                    var game = new CountingGame();
                    game.Run();
                    break;

                case 3:
                    var finder = new CombinationFinder();
                    finder.Run();
                    break;

                case 4:
                    var analyzer = new WordFrequencyAnalyzer();
                    analyzer.Run();
                    break;

                case 5:
                    var bracket = new PlayoffBracket();
                    bracket.Run();
                    break;

                case 6:
                    Console.WriteLine("1) Использовать DoublyLinkedList");
                    Console.WriteLine("2) Использовать List<T>");
                    Console.Write("Выберите вариант: ");
                    if (int.TryParse(Console.ReadLine(), out int opt) && opt == 1)
                    {
                        var custom = new CustomListReverser();
                        custom.Run();
                    }
                    else
                    {
                        var dotnet = new DotNetListReverser();
                        dotnet.Run();
                    }
                    break;


                case 7:
                    Console.WriteLine("1) Использовать двумерный массив");
                    Console.WriteLine("2) Использовать связный список");
                    Console.Write("Выберите вариант: ");
                    if (!int.TryParse(Console.ReadLine(), out int opt7))
                    {
                        Console.WriteLine("Неверный ввод."); break;
                    }

                    if (opt7 == 1)
                    {
                        var m1 = new DenseMatrix();
                        var m2 = new DenseMatrix();
                        Console.WriteLine("Заполняем первую матрицу:");
                        m1.FillFromConsole();
                        Console.WriteLine("Заполняем вторую матрицу:");
                        m2.FillFromConsole();

                        Console.WriteLine("\nМатрица 1:");
                        m1.Print();
                        Console.WriteLine("\nМатрица 2:");
                        m2.Print();

                        Console.WriteLine("\nТранспонированная первая:");
                        m1.Transpose().Print();

                        Console.WriteLine("\nСумма:");
                        m1.Add(m2).Print();

                        Console.WriteLine("\nПроизведение:");
                        m1.Multiply(m2).Print();
                    }
                    else
                    {
                        var s1 = new SparseMatrix();
                        var s2 = new SparseMatrix();
                        Console.WriteLine("Заполняем первую разреженную матрицу:");
                        s1.FillFromConsole();
                        Console.WriteLine("Заполняем вторую разреженную матрицу:");
                        s2.FillFromConsole();

                        Console.WriteLine("\nМатрица 1:");
                        s1.Print();
                        Console.WriteLine("\nМатрица 2:");
                        s2.Print();

                        Console.WriteLine("\nТранспонированная первая:");
                        s1.Transpose().Print();

                        Console.WriteLine("\nСумма:");
                        s1.Add(s2).Print();

                        Console.WriteLine("\nПроизведение:");
                        s1.Multiply(s2).Print();
                    }
                    break;

                default:
                    Console.WriteLine("Пункт меню вне диапазона 1–6.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
