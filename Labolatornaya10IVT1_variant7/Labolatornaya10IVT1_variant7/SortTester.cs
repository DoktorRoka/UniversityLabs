using System.Diagnostics;
using Labolatornaya10IVT1_variant7;

public class SortTester
{
    private readonly ISortingAlgorithm[] _algorithms;
    private readonly SortChecker _checker = new SortChecker();
    private readonly bool _descending;
    private const int N = 100_000;

    public SortTester(bool descending)
    {
        _descending = descending;
        _algorithms = new ISortingAlgorithm[]
        {
            new MergeSortDesc(),
            new HeapSortDesc(),
            new QuickSortDesc()
        };
    }

    public void RunTests()
    {
        var rng = new Random();
        int[] randomArray = Enumerable.Range(0, N)
                                      .Select(_ => rng.Next())
                                      .ToArray();

        int[] sortedAsc = (int[])randomArray.Clone();
        Array.Sort(sortedAsc);

        int[] sortedDesc = (int[])sortedAsc.Clone();
        Array.Reverse(sortedDesc);

        var cases = new (string Name, int[] Data)[]
        {
        ("Random",    randomArray),
        ("SortedAsc", sortedAsc),
        ("SortedDesc",sortedDesc)
        };

        using var fs = new FileStream("sorted.dat", FileMode.Create, FileAccess.Write);
        using var bw = new BinaryWriter(fs);

        foreach (var algo in _algorithms)
        {
            Console.WriteLine($"\n=== {algo.GetType().Name} ===");

            foreach (var (caseName, data) in cases)
            {
                int[] working = (int[])data.Clone();

                var sw = Stopwatch.StartNew();
                algo.Sort(working);
                sw.Stop();

                Console.WriteLine(
                    $"{caseName}: time {sw.Elapsed.Seconds}:{sw.ElapsedMilliseconds % 1000:D3}, " +
                    $"comparisons {algo.Comparisons}, swaps {algo.Swaps}"
                );

                bool ok = _checker.IsSorted(working, descending: _descending);
                if (!ok)
                    Console.WriteLine(
                        $"ERROR: {caseName} is NOT sorted in " +
                        $"{(_descending ? "descending" : "ascending")} order!"
                    );

                bw.Write(working.Length);
                foreach (int v in working)
                    bw.Write(v);
            }
        }

        Console.WriteLine("\nAll tests done. Results in sorted.dat");
    }
}
