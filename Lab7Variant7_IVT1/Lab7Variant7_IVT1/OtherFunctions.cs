using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab7Variant7_IVT7;

namespace Lab7Variant7_IVT1
{
    public class OtherFunctions
    {
        public int[] GenerateArray(int size, bool sorted = false, bool reverseSorted = false)
        {
            int[] arr = new int[size];
            Random random = new Random();

            if (!sorted && !reverseSorted)
            {
                // Случайный массив
                for (int i = 0; i < size; i++)
                {
                    arr[i] = random.Next();
                }
            }
            else if (sorted)
            {
                for (int i = 0; i < size; i++)
                {
                    arr[i] = i;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    arr[i] = size - i - 1;
                }
            }

            return arr;
        }

        public void SelectionSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    comparisons++;
                    if (arr[j] > arr[minIndex])
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    // Swap
                    int temp = arr[i];
                    arr[i] = arr[minIndex];
                    arr[minIndex] = temp;
                    swaps++;
                }
            }
        }

        public void InsertionSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] < key)
                {
                    comparisons++;
                    arr[j + 1] = arr[j];
                    swaps++;
                    j--;
                }

                if (j >= 0)
                {
                    comparisons++;
                }

                if (j + 1 != i)
                {
                    arr[j + 1] = key;
                    swaps++;
                }
            }
        }

        public void BubbleSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    comparisons++;
                    if (arr[j] < arr[j + 1])
                    {
                        // Swap
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swaps++;
                    }
                }
            }
        }

        public void CocktailSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            bool swapped;
            int start = 0;
            int end = arr.Length;

            do
            {
                swapped = false;

                for (int i = start; i < end - 1; i++)
                {
                    comparisons++;
                    if (arr[i] < arr[i + 1])
                    {
                        // Swap
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        swaps++;
                        swapped = true;
                    }
                }

                if (!swapped)
                    break;

                swapped = false;
                end--;

                for (int i = end - 1; i >= start; i--)
                {
                    comparisons++;
                    if (arr[i] > arr[i + 1])
                    {
                        // Swap
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        swaps++;
                        swapped = true;
                    }
                }

                start++;
            } while (swapped);
        }

        public void ShellSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;

            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = arr[i];
                    int j;

                    for (j = i; j >= gap; j -= gap)
                    {
                        comparisons++;
                        if (temp > arr[j - gap])
                        {
                            arr[j] = arr[j - gap];
                            swaps++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    arr[j] = temp;
                }
            }
        }

        public void SortAndMeasureTime(string sortName, int[] arr)
        {
            int[] arrCopy = (int[])arr.Clone();

            Console.WriteLine($"Метод сортировки: {sortName}");

            DateTime startTime = DateTime.Now;
            long comparisons, swaps;

            switch (sortName)
            {
                case "Выбором":
                    SelectionSort(arrCopy, out comparisons, out swaps);
                    break;
                case "Вставками":
                    InsertionSort(arrCopy, out comparisons, out swaps);
                    break;
                case "Пузырьком":
                    BubbleSort(arrCopy, out comparisons, out swaps);
                    break;
                case "Шейкер":
                    CocktailSort(arrCopy, out comparisons, out swaps);
                    break;
                case "Шелла":
                    ShellSort(arrCopy, out comparisons, out swaps);
                    break;
                default:
                    throw new ArgumentException("Неизвестный метод сортировки");
            }

            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;

            Console.WriteLine($"Время: {duration.Seconds} : {duration.Milliseconds}");
            Console.WriteLine($"Сравнения: {comparisons}");
            Console.WriteLine($"Перестановки: {swaps}\n");

            File.WriteAllLines("sorted.dat", arrCopy.Select(x => x.ToString()));
        }

        public bool CheckSortedFile()
        {
            int[] sortedData = File.ReadAllLines("sorted.dat")
                                   .Select(int.Parse)
                                   .ToArray();

            for (int i = 0; i < sortedData.Length - 1; i++)
            {
                if (sortedData[i] < sortedData[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        public void PerformSorting()
        {
            int[] randomArray = GenerateArray(100_000);
            int[] sortedArray = GenerateArray(100_000, sorted: true);
            int[] reverseSortedArray = GenerateArray(100_000, reverseSorted: true);

            string[] sortMethods = { "Выбором", "Вставками", "Пузырьком", "Шейкер", "Шелла" };

            foreach (string method in sortMethods)
            {
                Console.WriteLine("Случайный массив:");
                SortAndMeasureTime(method, randomArray);

                Console.WriteLine("Отсортированный массив:");
                SortAndMeasureTime(method, sortedArray);

                Console.WriteLine("Обратно отсортированный массив:");
                SortAndMeasureTime(method, reverseSortedArray);
            }

            bool isSorted = CheckSortedFile();
            Console.WriteLine($"Файл корректно отсортирован: {isSorted}");
        }
    }
}