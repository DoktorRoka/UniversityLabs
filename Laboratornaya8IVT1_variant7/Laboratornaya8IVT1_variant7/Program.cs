using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        const string filePath = "sorted.dat";
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Ошибка: файл '{filePath}' не найден.");
            return;
        }

        Console.WriteLine("Поиск по sorted.dat через алгоритмы поиска");
        Console.Write("Введите число для поиска: ");
        if (!int.TryParse(Console.ReadLine(), out int numberToSearch))
        {
            Console.WriteLine("Ошибка: введено не числовое значение.");
            return;
        }

        List<int> data = new List<int>();
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line.Trim(), out int number))
                    {
                        data.Add(number);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            return;
        }

        var (linPos, linComp, linTime) = LinearSearch(data, numberToSearch);
        Console.WriteLine($"Линейный поиск: позиция = {linPos}, сравнений = {linComp}, время = {linTime:F4} ms");

        var (binPos, binComp, binTime) = BinarySearchDescending(data, numberToSearch);
        Console.WriteLine($"Бинарный поиск: позиция = {binPos}, сравнений = {binComp}, время = {binTime:F4} ms");

        var (intPos, intComp, intTime) = InterpolationSearchDescending(data, numberToSearch);
        Console.WriteLine($"Интерполяционный поиск: позиция = {intPos}, сравнений = {intComp}, время = {intTime:F4} ms");


        // кмп бм

        var tests = new List<(string Text, string Pattern)>
        {
            ("съешь этих мягких французских булок да выпей чаю", "мягких"),
            ("AAAAAABAAAAAABAAAAAAB", "AAAAAB"),
            ("ABCDABCABCDABCABCDABCDABDE", "ABCDABD"),
            ("xyzxyzxyzxyzxyzxyzxyzxyz", "xyzxyz"),
        };

        foreach (var (text, pattern) in tests)
        {
            Console.WriteLine($"\nТекст:    '{text}'");
            Console.WriteLine($"Шаблон:  '{pattern}'\n");

            var (kmpPos, kmpComp, kmpTime) = KMPSearch(text, pattern);
            PrintResult("КМП", kmpPos, kmpComp, kmpTime);

            var (bmPos, bmComp, bmTime) = BoyerMooreSearch(text, pattern);
            PrintResult("БМ", bmPos, bmComp, bmTime);
        }


    }

    public static (int Position, long Comparisons, double ElapsedMs) LinearSearch(List<int> data, int value)
    {
        long comparisons = 0;
        int position = -1;
        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < data.Count; i++)
        {
            comparisons++;
            if (data[i] == value)
            {
                position = i;
                break;
            }
        }

        stopwatch.Stop();
        return (position, comparisons, stopwatch.Elapsed.TotalMilliseconds);
    }

    public static (int Position, long Comparisons, double ElapsedMs) BinarySearchDescending(List<int> data, int value)
    {
        long comparisons = 0;
        int position = -1;
        int left = 0, right = data.Count - 1;
        var stopwatch = Stopwatch.StartNew();

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            comparisons++;
            if (data[mid] == value)
            {
                position = mid;
                break;
            }
            comparisons++;
            
            if (data[mid] > value)
                left = mid + 1;
            else
                right = mid - 1;
        }

        stopwatch.Stop();
        return (position, comparisons, stopwatch.Elapsed.TotalMilliseconds);
    }

    public static (int Position, long Comparisons, double ElapsedMs) InterpolationSearchDescending(List<int> data, int value)
    {
        long comparisons = 0;
        int position = -1;
        int low = 0, high = data.Count - 1;
        var stopwatch = Stopwatch.StartNew();

        
        while (low <= high && value <= data[low] && value >= data[high])
        {
            if (low == high)
            {
                comparisons++;
                if (data[low] == value)
                    position = low;
                break;
            }

            if (data[low] == data[high])
            {
                comparisons++;
                if (data[low] == value)
                    position = low;
                break;
            }

            int probe = low + (int)((double)(data[low] - value) * (high - low) / (data[low] - data[high]));

            if (probe < low) probe = low;
            if (probe > high) probe = high;

            comparisons++;
            if (data[probe] == value)
            {
                position = probe;
                break;
            }

            comparisons++;
            if (data[probe] > value)
                low = probe + 1;
            else
                high = probe - 1;
        }

        stopwatch.Stop();
        return (position, comparisons, stopwatch.Elapsed.TotalMilliseconds);
    }


    //тут кмп бм


    //специально для кмп бм
    private static void PrintResult(string name, int position, long comparisons, TimeSpan elapsed)
    {
        string posStr = position >= 0 ? position.ToString() : "Не найдено";
        Console.WriteLine($"{name}: позиция = {posStr}, время = {elapsed.Seconds}:{elapsed.Milliseconds:D3}, сравнений = {comparisons}");
    }

    //Кнут морриса прата
    public static (int Position, long Comparisons, TimeSpan Elapsed) KMPSearch(string text, string pattern)
    {
        int n = text.Length;
        int m = pattern.Length;
        var lps = new int[m];
        BuildLps(pattern, lps);

        long comparisons = 0;
        int i = 0, j = 0;
        var stopwatch = Stopwatch.StartNew();

        while (i < n)
        {
            comparisons++;
            if (pattern[j] == text[i])
            {
                i++; j++;
                if (j == m)
                {
                    stopwatch.Stop();
                    return (i - j, comparisons, stopwatch.Elapsed);
                }
            }
            else
            {
                if (j > 0)
                    j = lps[j - 1];
                else
                    i++;
            }
        }

        stopwatch.Stop();
        return (-1, comparisons, stopwatch.Elapsed);
    }

    private static void BuildLps(string pattern, int[] lps)
    {
        int length = 0;
        lps[0] = 0;
        int i = 1;
        while (i < pattern.Length)
        {
            if (pattern[i] == pattern[length])
            {
                length++;
                lps[i] = length;
                i++;
            }
            else
            {
                if (length > 0)
                {
                    length = lps[length - 1];
                }
                else
                {
                    lps[i] = 0;
                    i++;
                }
            }
        }
    }

    // Бойер мур (тут я использую плохие символы
    public static (int Position, long Comparisons, TimeSpan Elapsed) BoyerMooreSearch(string text, string pattern)
    {
        int n = text.Length;
        int m = pattern.Length;
        var badChar = new int[256];
        for (int i = 0; i < badChar.Length; i++) badChar[i] = -1;
        for (int i = 0; i < m; i++) badChar[(byte)pattern[i]] = i;

        long comparisons = 0;
        int s = 0;
        var stopwatch = Stopwatch.StartNew();

        while (s <= n - m)
        {
            int j = m - 1;
            while (j >= 0)
            {
                comparisons++;
                if (pattern[j] != text[s + j])
                    break;
                j--;
            }
            if (j < 0)
            {
                stopwatch.Stop();
                return (s, comparisons, stopwatch.Elapsed);
            }
            int shift = j - badChar[(byte)text[s + j]];
            s += Math.Max(1, shift);
        }

        stopwatch.Stop();
        return (-1, comparisons, stopwatch.Elapsed);
    }


}