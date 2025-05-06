using Labolatornaya10IVT1_variant7;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("1) Сортировка через слияние, пирамидальную, и быструю");
        Console.WriteLine("2) Отсортировать массив из 100 купюр");
        Console.WriteLine("3) Найти путь по графу");
        Console.WriteLine("4) Найти города в которого пути не длинее 200 км");

        int my_choice = int.Parse(Console.ReadLine());

        switch (my_choice)
        {
            case 1:
                var tester = new SortTester(descending: true);
                tester.RunTests();


                break;
            case 2:
                var rng = new Random();
                int[] bills = new int[100];
                int[] denom = { 1, 2, 5, 10, 20, 50, 100 };
                for (int i = 0; i < bills.Length; i++)
                {
                    bills[i] = denom[rng.Next(denom.Length)];
                }

                var counter = new CountingSort();
                counter.Sort(bills);

                Console.WriteLine("Сортированные купюры:");
                Console.WriteLine(string.Join(", ", bills));


                break;
            case 3:
                int[,] mat = {
                    {0,1,1,0,0,0,0,0},
                    {1,0,0,0,0,1,1,0},
                    {1,0,0,1,0,1,0,1},
                    {0,0,1,0,1,0,0,0},
                    {0,0,0,1,0,1,0,0},
                    {0,1,1,0,1,0,0,0},
                    {0,1,0,0,0,0,0,1},
                    {0,0,1,0,0,0,1,0}
                };

                var graph = new Graph(mat);
                graph.PrintIncidenceMatrix();
                graph.PrintAdjacencyList();

                Console.WriteLine("\nВведите начальную точку (1–8):");
                int x = int.Parse(Console.ReadLine()!);
                Console.WriteLine("Введите конечную точку (1–8):");
                int y = int.Parse(Console.ReadLine()!);

                var dfsPath = graph.FindPathDFS(x - 1, y - 1)
                                    .Select(v => (v + 1).ToString());
                var bfsPath = graph.FindPathBFS(x - 1, y - 1)
                                    .Select(v => (v + 1).ToString());

                Console.WriteLine("\nDFS:");
                Console.WriteLine(string.Join(" -> ", dfsPath));

                Console.WriteLine("\nBFS:");
                Console.WriteLine(string.Join(" -> ", bfsPath));
                break;
            case 4:
                int[,] A = {
                {   0,  50,  -1, 100,  -1 },
                {  50,   0,  20,  -1, 150 },
                {  -1,  20,   0,  30,  -1 },
                { 100,  -1,  30,   0,  60 },
                {  -1, 150,  -1,  60,   0 }
                };
                var net = new RoadNetwork(A);

                Console.WriteLine("Введите номер города-источника (1–5):");
                int s = int.Parse(Console.ReadLine()!) - 1;

                const int MAX_D = 200;
                var reachable = net.GetReachable(s, MAX_D);

                Console.WriteLine($"\nИз города {s + 1} можно добраться по ≤{MAX_D} км до следующих городов:");
                if (reachable.Count == 0)
                {
                    Console.WriteLine("  ни до одного.");
                }
                else
                {
                    foreach (var (vertex, distance) in reachable)
                    {
                        Console.WriteLine($"  город {vertex + 1} (расстояние {distance} км)");
                    }
                }
                break;



        }




    }
}