using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLabIVT7
{
    public class HelpingFunctions
    {
        public static int GetFirstFractionalDigit(double number) // найти первую цифру дробной части
        {
            int d = 0;
            double input_number = number;
            return d = (int)(Math.Round(input_number, 1) * 10) % 10;
        }

        public static (int, int) GetHoursAndMinutesFromSeconds(int totalSeconds) // найти полные часы и минуты 
        {
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            return (hours, minutes);
        }

        public static double CornerInTimeDegrees(int hours, int minutes, int seconds) // найти угол с прошедшего времени
        {
            double AngleOfHours = hours * 30;  // 30 градусов за час (т.е 12 часов это 360 градусов и мы делим на 2)
            double AngleOfMinutes = minutes * 0.5; // 0.5 градусов за одну минуту (30 / 60 = 0.5)
            double AngleOfSeconds = seconds * (0.5 / 60); // 60 секунд это минута, а минута добавляет 0.5 градуса;
            return AngleOfHours + AngleOfMinutes + AngleOfSeconds;
        }

        public static (double, double) CalculateAreaAndPerimetr(double a, double b) // подсчитать площадь и периметр
        {
            double area = 0.5 * a * b; // вычисляем площадь по формуле 1/2 * a * b
            double c = Math.Sqrt(a * a + b * b);
            double Perimetr = a + b + c; // Мы нашли сторону C по теореме Пифагора, а потом нашли периметр
            return (area, Perimetr);
        }

        public static int CalculateNumberMultiple(int number) // подсчитать цифры умноженные на друг друга
        {
            int number1 = number / 1000;
            int number2 = (number / 100) % 10;
            int number3 = (number / 10) % 10;
            int number4 = number % 10;
            return number1 * number2 * number3 * number4;
        }

        public static int ReverseThreeDigitNumber(int number) // перевернуть числа
        {
            int number1 = number / 100;
            int number2 = (number / 10) % 10;
            int number3 = number % 10;
            return number3 * 100 + number2 * 10 + number1;
        }

        public static double SolveEquation(double x)
        {
            return ((3 * x - 5) * x + 2) * x * x - x + 7;
        }

        public static (double x, double y, double z) SolveLinearSystem(
            double a1, double b1, double c1, double d1,
            double a2, double b2, double c2, double d2,
            double a3, double b3, double c3, double d3)
        {
            // находим общий определитель (использую метод Крамера для вычисления)
            double determinant = a1 * (b2 * c3 - b3 * c2) -
                                 b1 * (a2 * c3 - a3 * c2) +
                                 c1 * (a2 * b3 - a3 * b2);

            if (determinant == 0)
            {
                throw new ArgumentException("В системе нет решений, так как определитель равен 0");
            }

            double determinantX = d1 * (b2 * c3 - b3 * c2) -
                                  b1 * (d2 * c3 - d3 * c2) +
                                  c1 * (d2 * b3 - d3 * b2);

            double determinantY = a1 * (d2 * c3 - d3 * c2) -
                                  d1 * (a2 * c3 - a3 * c2) +
                                  c1 * (a2 * d3 - a3 * d2);

            double determinantZ = a1 * (b2 * d3 - b3 * d2) -
                                  b1 * (a2 * d3 - a3 * d2) +
                                  d1 * (a2 * b3 - a3 * b2);

            double x = determinantX / determinant;
            double y = determinantY / determinant;
            double z = determinantZ / determinant;

            return (x, y, z);
        }

       
    }
}
