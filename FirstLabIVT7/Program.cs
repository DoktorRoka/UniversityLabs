using FirstLabIVT7;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Выберите программу:");
        Console.WriteLine("1. Найти первую цифру дробной части числа. (использовать запятую для разделения)");
        Console.WriteLine("2. Определить количество полных часов и минут, прошедших с начала суток.");
        Console.WriteLine("3. Определить угол в градусах между часовой в начале и между введеным временем.");
        Console.WriteLine("4. Поменять значения местами.");
        Console.WriteLine("5. Вычислить площадь и периметр треугольника.");
        Console.WriteLine("6. Найти произведение чисел введеного числа.");
        Console.WriteLine("7. Перевернуть число.");
        Console.WriteLine("8. Решить пример: 3x^4 - 5x^3 + 2x^2 - x + 7");
        Console.WriteLine("9. Решить систему уравнений с введенными значенями.");
        Console.WriteLine("10. Создать таблицу состава спортклуба.");
        Console.WriteLine("11. Вычесть значение по формуле.");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Введите вещественное число: ");
                double x_chislo = Convert.ToDouble(Console.ReadLine());
                int firstFractionalDigit = HelpingFunctions.GetFirstFractionalDigit(x_chislo);
                Console.WriteLine("Первая цифра дробной части числа: {0}", firstFractionalDigit);
                break;

            case "2":
                Console.Write("Введите количество секунд с начала суток: ");
                int totalSeconds = Convert.ToInt32(Console.ReadLine());
                (int hours, int minutes) = HelpingFunctions.GetHoursAndMinutesFromSeconds(totalSeconds);
                Console.WriteLine("Прошло {0} часов и {1} минут ", hours, minutes);
                
                break;

            case "3":
                Console.Write("Введите текущее время\n");
                Console.Write("Часы (от 0 до 11): ");
                int hour = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите минуты(от 0 до 59): ");
                int minute = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите секунды(от 0 до 59): ");
                int seconds = Convert.ToInt32(Console.ReadLine());
                double count_angle = HelpingFunctions.CornerInTimeDegrees(hour, minute, seconds);
                Console.WriteLine("{0} градусов", count_angle);
                break;

            case "4":
                Console.Write("Введите число а: ");
                int a = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите число b: ");
                int b = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Числа сейчас: a = " + a + " b = " + b);
                a = a + b;
                b = a - b;
                a = a - b;

                Console.WriteLine("После обмена: a = {0} b = {1}", a, b);

                break;

            case "5":
                Console.Write("Введите длину стороны а: ");
                double a_storona = Convert.ToDouble(Console.ReadLine());
                Console.Write("Введите длину стороны b: ");
                double b_storona = Convert.ToDouble(Console.ReadLine());
                (double area, double perimeter) = HelpingFunctions.CalculateAreaAndPerimetr(a_storona, b_storona);
                Console.WriteLine("Площадь: {0}", area);
                Console.WriteLine("Периметр: {0}", perimeter);
                break;

            case "6":
                Console.Write("Введи четырёхзначное число (целое): ");
                
                int number = Convert.ToInt32(Console.ReadLine());
                if (number < 1000 || number > 9999)
                {
                    Console.WriteLine("Ввёди четырехзначное число");
                    return;
                }
                int multiplied_number = HelpingFunctions.CalculateNumberMultiple(number);
                Console.WriteLine("Умноженное число: {0}", multiplied_number);
                break;

            case "7":
                Console.Write("Введи трехзначное число (целое): ");
                int number_to_reverse = Convert.ToInt32(Console.ReadLine());
                if (number_to_reverse < 100 || number_to_reverse > 999)
                {
                    Console.WriteLine("Ввёди трехзначное число");
                    return;
                }
                int reversed_number = HelpingFunctions.ReverseThreeDigitNumber(number_to_reverse);
                Console.Write("Перевернутое число: {0} ", reversed_number);
                break;

            case "8":
                Console.Write("Введите x для поиска: ");
                double given_x = Convert.ToDouble(Console.ReadLine());
                double solved_equation = HelpingFunctions.SolveEquation(given_x);
                Console.Write("Решенный пример: {0}", solved_equation);
                break;

            case "9":
                Console.WriteLine("Введите коэффициенты для первой строки (a1, b1, c1, d1):");
                double a1 = Convert.ToDouble(Console.ReadLine());
                double b1 = Convert.ToDouble(Console.ReadLine());
                double c1 = Convert.ToDouble(Console.ReadLine());
                double d1 = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Введите коэффициенты для второй строки (a2, b2, c2, d2):");
                double a2 = Convert.ToDouble(Console.ReadLine());
                double b2 = Convert.ToDouble(Console.ReadLine());
                double c2 = Convert.ToDouble(Console.ReadLine());
                double d2 = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Введите коэффициенты для третьей строки (a3, b3, c3, d3):");
                double a3 = Convert.ToDouble(Console.ReadLine());
                double b3 = Convert.ToDouble(Console.ReadLine());
                double c3 = Convert.ToDouble(Console.ReadLine());
                double d3 = Convert.ToDouble(Console.ReadLine());

                try
                {
                    (double x, double y, double z) = HelpingFunctions.SolveLinearSystem(a1, b1, c1, d1, a2, b2, c2, d2, a3, b3, c3, d3);
                    Console.WriteLine("Решение системы: x = {0} y = {1} z = {2} ", x, y, z);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                break;

            // -------------------------------------------------------------------------------------------------------------------

            case "10":
                Console.Write("Введите количество участников: ");
                int numParticipants = int.Parse(Console.ReadLine());

                //массив
                string[] fio = new string[numParticipants];
                char[] type = new char[numParticipants]; // T - тренер, C - спортсмен
                int[] birthYear = new int[numParticipants];
                int[] experience = new int[numParticipants];

                
                for (int i = 0; i < numParticipants; i++) // ввод данных для участников сколько мы вписали
                {
                    Console.WriteLine($"\nВвод данных для участника {i + 1}:");

                    Console.Write("Введите ФИО: ");
                    fio[i] = Console.ReadLine();

                    Console.Write("Введите тип (Т - тренер, С - спортсмен): ");
                    type[i] = char.Parse(Console.ReadLine().ToUpper());

                    Console.Write("Введите год рождения: ");
                    birthYear[i] = int.Parse(Console.ReadLine());

                    Console.Write("Введите опыт (лет): ");
                    experience[i] = int.Parse(Console.ReadLine());
                }

               
                Console.WriteLine("\nСостав спортклуба:");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("| ФИО            | Тип | Год рождения | Опыт (лет) |");
                Console.WriteLine("----------------------------------------------");

                for (int i = 0; i < numParticipants; i++)
                {
                    Console.WriteLine($"| {fio[i],-14} | {type[i],-3} | {birthYear[i],-12} | {experience[i],-10} |");
                }

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Перечисляемый тип: Т - тренер, С - спортсмен");
                break;

            case "11":
                Console.Write("Введите значение a: ");
                double a_equation = double.Parse(Console.ReadLine());

                Console.Write("Введите значение b: ");
                double b_equation = double.Parse(Console.ReadLine());

                Console.Write("Введите значение x: ");
                double x_equation = double.Parse(Console.ReadLine());

                // подкоренное выражение не было отрицательным (1 формула)
                if (x_equation + b_equation <= 0)
                {
                    Console.WriteLine("Ошибка: x + b должно быть больше нуля для корректного вычисления корня.");
                    return;
                }

                // знаменатель не должен быть равен нулю (2 формула)
                if (a_equation * x_equation == 0)
                {
                    Console.WriteLine("Ошибка: ax не должно быть равно нулю для корректного вычисления знаменателя.");
                    return;
                }

                //s = x^3 * tg^2((x + b)^2) + a / sqrt(x + b)
                double s = Math.Pow(x_equation, 3) * Math.Pow(Math.Tan(Math.Pow(x_equation + b_equation, 2)), 2) + a_equation / Math.Sqrt(x_equation + b_equation);

                //Q = (bx^2 - a) / (e^(ax) - 1)
                double Q = (b_equation * Math.Pow(x_equation, 2) - a_equation) / (Math.Exp(a_equation * x_equation) - 1);

                Console.WriteLine("Значение s: {0}", s);
                Console.WriteLine("Значение Q: {0}", Q);
                break;

            default:
                Console.WriteLine("Неверный выбор. Пожалуйста, введите из списка.");
                break;
        }
        Console.WriteLine("\nНажмите любую клавишу чтобы выйти...");
        Console.ReadLine();
    }

}
 