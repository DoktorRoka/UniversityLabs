using ThirdLabIVT7;
using static ThirdLabIVT7.LabFunctions;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("1) Создать случайный массив(Ввод размера)");
        Console.WriteLine("2) Поворот двумерного массива 7x7 на 90 градусов вправо(рандомный массив)");
        Console.WriteLine("3) Циклический свдиг массива влево(Ввод элементов и позиции)");
        Console.WriteLine("4) Поэлементное сложение и вычитание двумерных массивов размером 3x3 (Ввод массивов)");
        Console.WriteLine("5) Умножить две матрицы 5x5 (Ввод пользователя)");
        Console.WriteLine("6) Демонстрация функций");
        Console.WriteLine("7) Нахождение n-го члена ряда Фибоначчи");
        Console.WriteLine("8) Вычислить определить матрицы NxN");
        Console.WriteLine("9) Заполнить массив по варианту");
        Console.WriteLine("10) Проверить являются ли все суммы симметричных элементов массива равны");

        int user_choice = int.Parse(Console.ReadLine());

        switch (user_choice)
        {
            case 1:

                LabFunctions.ArrayMaker arrayMaker = new LabFunctions.ArrayMaker();
                arrayMaker.ProcessArray();

                break;

            case 2:

                LabFunctions.ArrayRotator arrayRotator = new LabFunctions.ArrayRotator();
                arrayRotator.ProcessRotation();

                break;
            case 3:

                LabFunctions.CyclicArrayRotator cyclicArrayRotator = new LabFunctions.CyclicArrayRotator();
                cyclicArrayRotator.ProcessShift();
                break;

            case 4:

                LabFunctions.MatrixCalculator calculator = new LabFunctions.MatrixCalculator();

                int[,] matrix1 = new int[3, 3];
                int[,] matrix2 = new int[3, 3];

                Console.WriteLine("Введите элементы для первого массива 3x3:");
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        matrix1[i, j] = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите элементы для второго массива 3x3:");
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        matrix2[i, j] = int.Parse(Console.ReadLine());

                double averageAdd;
                int[,] sumMatrix = calculator.AddMatrices(matrix1, matrix2, out averageAdd);

                Console.WriteLine("Результат сложения:");
                calculator.PrintMatrix(sumMatrix);
                Console.WriteLine($"Среднее значение элементов (сложение): {averageAdd}");

                double averageSubtract;
                int[,] diffMatrix = calculator.SubtractMatrices(matrix1, matrix2, out averageSubtract);

                Console.WriteLine("Результат вычитания:");
                calculator.PrintMatrix(diffMatrix);
                Console.WriteLine($"Среднее значение элементов (вычитание): {averageSubtract}");


                break;
            case 5:

                LabFunctions.MatrixMultiplier matrixMultiplier = new LabFunctions.MatrixMultiplier();
                matrixMultiplier.ProcessMultiplication();

                break;
            case 6:

                int[] array = { 4, -2, 7, 1, 9, -5, 3 };
                LabFunctions.ArrayOperations operations = new LabFunctions.ArrayOperations();

                Console.WriteLine("Массив: " + string.Join(", ", array));

                Console.WriteLine("Итеративная сумма: " + operations.SumIterative(array));
                Console.WriteLine("Рекурсивная сумма: " + operations.SumRecursive(array));

                Console.WriteLine("Итеративный минимум: " + operations.MinIterative(array));
                Console.WriteLine("Рекурсивный минимум: " + operations.MinRecursive(array));

                break;
            case 7:

                LabFunctions.FibonacciCalculator fib_calculator = new LabFunctions.FibonacciCalculator();

                Console.Write("Введите номер элемента ряда Фибоначчи: ");
                int n = int.Parse(Console.ReadLine());

                int result = fib_calculator.Fibonacci(n);
                Console.WriteLine($"{n}-й член ряда Фибоначчи: {result}");

                break;
            case 8:

                LabFunctions.DeterminantCalculator determinant_calculator = new LabFunctions.DeterminantCalculator();

                int[,] matrix = {
                        { 1, 2, 3 },
                        { 0, 4, 5 },
                        { 1, 0, 6 }
                    };

                int deter_result = determinant_calculator.CalculateDeterminant(matrix);
                Console.WriteLine("Определитель матрицы: " + deter_result);

                break;
            case 9:

                LabFunctions.MatrixTask matrix_task = new LabFunctions.MatrixTask();

                matrix_task.DrawMatrixButterfly();

                break;
            case 10:
                Console.Write("Введите четное число элементов массива: ");
                int number_of_elements;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out number_of_elements) && number_of_elements > 0 && number_of_elements % 2 == 0)
                        break;
                    Console.WriteLine("Число должно быть положительным");
                }

                int[] array_case10 = new int[number_of_elements];
                Console.WriteLine($"Введите {number_of_elements} элементов массива:");
                for (int i = 0; i < number_of_elements; i++)
                {
                    while (true)
                    {
                        Console.Write($"Элемент [{i}]: ");
                        if (int.TryParse(Console.ReadLine(), out int value))
                        {
                            array_case10[i] = value;
                            break;
                        }
                        Console.WriteLine("Число должно быть целым");
                    }
                }

                SymmetryChecker checker = new SymmetryChecker();
                bool result_case10 = checker.AreSymmetricSumsEqual(array_case10);

                Console.WriteLine(result_case10 ? "TRUE" : "FALSE");
                break;

        }
    }
}