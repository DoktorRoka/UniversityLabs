using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondLabIVT7
{
    internal class second_lab_functions
    {
        public static void reshenie_diskriminant(double a, double b, double c)
        {
            double diskriminant = b * b - 4 * a * c;

            if (diskriminant > 0) 
            {
                Console.WriteLine($"D={diskriminant}");
                double x1 = (-b + Math.Sqrt(diskriminant)) / (2 * a);
                double x2 = (-b - Math.Sqrt(diskriminant)) / (2 * a);
                Console.WriteLine($"x1 = {x1}, x2 = {x2}");
            }

            if (diskriminant == 0) 
            {
                Console.WriteLine($"D={diskriminant}");
                double x1 = -b / (2 * a);
                Console.WriteLine($"x1 = {x1}");
            }

            if (diskriminant < 0) 
            {
                Console.WriteLine($"D={diskriminant}");
                Console.WriteLine("Действительных корней нет. Ответ в виде комплексных чисел: ");
                diskriminant = Math.Abs(diskriminant);

                double x1 = -b / (2 * a);

                Console.WriteLine($"x1={x1}-{Math.Sqrt(diskriminant)}i, x2={x1}+{Math.Sqrt(diskriminant)}i");

            }
        }

        public static void vichislenie_Pi(int number_of_operations)
        {
            if (number_of_operations < 0) {
                Console.WriteLine("Нельзя сделать отрицательное кол-во операций");
                return; 
            }
            Console.WriteLine($"Количество слагаемых для вычисления: {number_of_operations}");

            double pi_4 = 0.0;
            int current_symbol = 1;

            for (int i = 0; i < number_of_operations; i++)
            {
                int denominator = 2 * i + 1;  
                double term = current_symbol / (double)denominator;  
                pi_4 += term;  
                string sign = current_symbol == 1 ? "+" : "-";  
                Console.WriteLine($"Шаг {i + 1}: текущее слагаемое = {sign} 1/{denominator}, сумма для pi/4 = {pi_4}");
                current_symbol *= -1;  // меняем знак
            }
            double pi = pi_4 * 4;
            Console.WriteLine($"Приближенное значение pi={pi}");

        }

        public static void fibbonachi_ryad(int number_of_operations)
        {
            double[] f = new double[number_of_operations];
            int number_of_4_digit_number = 0;

            for (int i = 0; i < number_of_operations;i++)
            {
                if (i < 2)
                    f[i] = 1;
                else
                    f[i] = f[i - 1] + f[i - 2];
                if (f[i] >= 1000 && f[i] <= 9999)
                    number_of_4_digit_number++;

                Console.WriteLine($"Шаг {i} f = {f[i]}");

            
            }
            Console.WriteLine($"Всего 4-х значных элементов: {number_of_4_digit_number}");
        }

        private static double Factorial(int n)
        {
            double result = 1;
            for (int i = 2; i<=n; i++)
            {
                result *= i;
            }
            return result;
        }

        public static void ryad_Taylora()
        {
            double q;
            int x;
            Console.WriteLine("Введите значение x: ");
            x = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите Q: ");
            q = double.Parse(Console.ReadLine());

            double tmp = 0;
            int step = 2;
            do
            {
                if (step == 2)
                    tmp = 1;
                else
                {
                    tmp = step % 4 != 0 ?
                        tmp - (Math.Pow(x, step) / Factorial(step)) :
                        tmp + (Math.Pow(x, step) / Factorial(step));
                }
                Console.WriteLine($"Шаг = {step} вычисление = {tmp} < {q}");
                step += 2;

            } while (tmp >= q);
        }

        public static void combination_finder()
        {
            int n;

            Console.WriteLine("Введите N>0");
            n = int.Parse(Console.ReadLine());

            double result = 0;

            bool find = false;

            int x = 0, y = 0, z = 0;
            for (x = 0; x< n; x++)
                for (y = 0; y< n; y++)
                    for (z = 0; z< n; z++)
                    {
                        result = Math.Pow(x, 3) + Math.Pow(y, 3) + Math.Pow(z, 3);

                        if (result == n)
                        {
                            Console.WriteLine(find ?
                                "В зоде перебора нашли значение x^3 + y^3 + z^3 = N" :
                                "В ходе перебора не нашли значения x^3 + y^3 + z^3 = N");
                            Console.WriteLine($"{x}^3 + {y}^3 + {z}^3 = {n}");
                            find = true;
                        }
                    }
            if (!find)
                Console.WriteLine("Ничего не найдено!");
        }

        public static void okonchanie_slova(int year)
        {
            int lastDigit = year % 10;
            int lastTwoDigits = year % 100;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            {
                Console.WriteLine($"{year} лет");
            }
            else if (lastDigit == 1)
            {
                Console.WriteLine($"{year} год");
            }

            else if (lastDigit >= 2 && lastDigit <= 4) 
            {
                Console.WriteLine($"{year} года");
            }
            else
            {
                Console.WriteLine($"{year} лет");
            }
        }

        public static void raznost_chisel(double number1, double number2, double number3)
        {
            double maxNum = Math.Max(number1, Math.Max(number2, number3));
            double minNum = Math.Min(number1, Math.Min(number2, number3));
            double difference = maxNum - minNum;
            Console.WriteLine($"Разность максимального и минимального числа: {difference}");
        }

        public static void vse_delimie_chisla_na_5()
        {
            for (int i = 1; i < 100; i++)
            {
                int SumOfDigits = 0;
                int number = i;
                while (number > 0)
                {
                    SumOfDigits += number % 10;
                    number /= 10;
                }

                if (SumOfDigits % 5 == 0)
                {
                    Console.WriteLine($"Сумма цифр числа {i} делится на 5: {SumOfDigits}");
                }
            }
        }
    }
}
