using SecondLabIVT7;
using System.Windows.Markup;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("1) Решить квадратное уравнение (a, b, c)");
        Console.WriteLine("2) Приближённое вычисление числа pi (ввод слагаемых от пользователя)");
        Console.WriteLine("3) Вывести на экран кол-во четырехзначных чисел в ряде Фибоначчи");
        Console.WriteLine("4) Вычислить приблеженное cos(x) по формуле тейлора (ввод x и q)");
        Console.WriteLine("5) Найти комбинации натурального числа N");
        Console.WriteLine("6) Правильное окончанеие слово (лет/год/года)");
        Console.WriteLine("7) Разность 3 чисел (3 числа)");
        Console.WriteLine("8) Все двузначные числа которые делятся на 5");


        int user_choice = int.Parse(Console.ReadLine());


        switch (user_choice)
        {
            case 1:
                Console.WriteLine("Решаем уравнение: ax^2 + bx + c = 0");
                Console.Write("Введите а: ");
                int letter_a = int.Parse(Console.ReadLine());
                Console.Write("Введите b: ");
                int letter_b = int.Parse(Console.ReadLine());
                Console.Write("Введите c: ");
                int letter_c = int.Parse(Console.ReadLine());
                second_lab_functions.reshenie_diskriminant(letter_a, letter_b, letter_c);

                break;
            case 2:
                Console.WriteLine("Введите количество операций: ");
                int number_of_operations = int.Parse(Console.ReadLine());
                second_lab_functions.vichislenie_Pi(number_of_operations);
                break;

            case 3:
                Console.Write("Введите кол-во операций: ");
                int number_of_oper = int.Parse(Console.ReadLine());
                second_lab_functions.fibbonachi_ryad(number_of_oper);
                break;

            case 4:
                second_lab_functions.ryad_Taylora();
                break;

            case 5:
                second_lab_functions.combination_finder();
                break;

            case 6:
                Console.WriteLine("Введите число от 1 до 100: ");
                int year = int.Parse(Console.ReadLine());

                if (year < 1 || year > 100)
                {
                    Console.WriteLine("Число должно быть в диапазоне от 1 до 100");
                    return;
                }
                second_lab_functions.okonchanie_slova(year);

                break;

            case 7:
                Console.Write("Число 1 = ");
                double number_1 = double.Parse(Console.ReadLine());
                Console.Write("Число 2 = ");
                double number_2 = double.Parse(Console.ReadLine());
                Console.Write("Число 3 = ");
                double number_3 = double.Parse(Console.ReadLine());

                second_lab_functions.raznost_chisel(number_1, number_2, number_3);
                break;

            case 8:
                second_lab_functions.vse_delimie_chisla_na_5();
                break;
        }
    }
}