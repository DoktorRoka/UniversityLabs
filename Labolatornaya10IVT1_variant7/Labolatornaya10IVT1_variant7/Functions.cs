namespace Labolatornaya10IVT1_variant7
{

    public interface ISortingAlgorithm
    {
        void Sort(int[] array);
        long Comparisons { get; }
        long Swaps { get; }
    }


    

    public class MergeSortDesc : ISortingAlgorithm
    {
        public long Comparisons { get; private set; }
        public long Swaps { get; private set; }

        public void Sort(int[] array)
        {
            Comparisons = 0;
            Swaps = 0;
            MergeSortRec(array, 0, array.Length - 1);
        }

        private void MergeSortRec(int[] a, int l, int r)
        {
            if (l >= r) return;
            int m = (l + r) / 2;
            MergeSortRec(a, l, m);
            MergeSortRec(a, m + 1, r);
            Merge(a, l, m, r);
        }

        private void Merge(int[] a, int l, int m, int r)
        {
            int[] L = a[l..(m + 1)];
            int[] R = a[(m + 1)..(r + 1)];
            int i = 0, j = 0, k = l;

            while (i < L.Length && j < R.Length)
            {
                Comparisons++;
                if (L[i] >= R[j])
                    a[k++] = L[i++];
                else
                    a[k++] = R[j++];
                Swaps++;
            }
            while (i < L.Length) { a[k++] = L[i++]; Swaps++; }
            while (j < R.Length) { a[k++] = R[j++]; Swaps++; }
        }
    }

    public class HeapSortDesc : ISortingAlgorithm
    {
        public long Comparisons { get; private set; }
        public long Swaps { get; private set; }

        public void Sort(int[] array)
        {
            Comparisons = 0;
            Swaps = 0;
            int n = array.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                Swap(array, 0, i);
                Heapify(array, i, 0);
            }
        }

        private void Heapify(int[] a, int size, int i)
        {
            int smallest = i;
            int l = 2 * i + 1, r = 2 * i + 2;

            if (l < size)
            {
                Comparisons++;
                if (a[l] < a[smallest])
                    smallest = l;
            }
            if (r < size)
            {
                Comparisons++;
                if (a[r] < a[smallest])
                    smallest = r;
            }

            if (smallest != i)
            {
                Swap(a, i, smallest);
                Heapify(a, size, smallest);
            }
        }

        private void Swap(int[] a, int i, int j)
        {
            Swaps++;
            (a[i], a[j]) = (a[j], a[i]);
        }
    }


    public class QuickSortDesc : ISortingAlgorithm
    {
        private readonly Random _rng = new Random();

        public long Comparisons { get; private set; }
        public long Swaps { get; private set; }

        public void Sort(int[] array)
        {
            Comparisons = 0;
            Swaps = 0;
            QuickRec(array, 0, array.Length - 1);
        }

        private void QuickRec(int[] a, int lo, int hi)
        {
            while (lo < hi)
            {
                int p = Partition(a, lo, hi);

                if (p - lo < hi - p)
                {
                    QuickRec(a, lo, p - 1);
                    lo = p + 1;     
                }
                else
                {
                    QuickRec(a, p + 1, hi);
                    hi = p - 1;      
                }
            }
        }

        private int Partition(int[] a, int lo, int hi)
        {
            int pivotIndex = _rng.Next(lo, hi + 1);
            Swap(a, pivotIndex, hi);

            int pivot = a[hi];
            int i = lo - 1;

            for (int j = lo; j < hi; j++)
            {
                Comparisons++;
                if (a[j] >= pivot)
                {
                    i++;
                    Swap(a, i, j);
                }
            }

            Swap(a, i + 1, hi);
            return i + 1;
        }

        private void Swap(int[] a, int i, int j)
        {
            Swaps++;
            (a[i], a[j]) = (a[j], a[i]);
        }
    }

    public class SortChecker
    {
        public bool IsSorted(int[] array, bool descending = false)
        {
            if (!descending)
            {
                for (int i = 1; i < array.Length; i++)
                    if (array[i - 1] > array[i])
                        return false;
            }
            else
            {
                for (int i = 1; i < array.Length; i++)
                    if (array[i - 1] < array[i])
                        return false;
            }
            return true;
        }

    }

    public class CountingSort
    {
        private static readonly int[] Denominations = { 1, 2, 5, 10, 20, 50, 100 };

        
        public void Sort(int[] array)
        {
            if (array == null || array.Length == 0)
                return;

            int max = 0;
            foreach (int v in array)
                if (v > max) max = v;

            int[] count = new int[max + 1];

            foreach (int v in array)
                count[v]++;

            for (int i = 1; i < count.Length; i++)
                count[i] += count[i - 1];

            int[] output = new int[array.Length];

            for (int i = array.Length - 1; i >= 0; i--)
            {
                int v = array[i];
                int pos = --count[v];
                output[pos] = v;
            }

            Array.Copy(output, array, array.Length);
        }
    }


    public class Graph
    {
        private readonly int n;              
        private readonly int[,] adjMatrix;    
        private readonly int[,] incMatrix;    
        private readonly List<int>[] adjList; 

        public Graph(int[,] adjacencyMatrix)
        {
            adjMatrix = adjacencyMatrix;
            n = adjMatrix.GetLength(0);
            incMatrix = BuildIncidenceMatrix();
            adjList = BuildAdjacencyList();
        }

        private int[,] BuildIncidenceMatrix()
        {
            var edges = new List<(int u, int v)>();
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                    if (adjMatrix[i, j] == 1)
                        edges.Add((i, j));

            int m = edges.Count;
            var inc = new int[n, m];
            for (int e = 0; e < m; e++)
            {
                var (u, v) = edges[e];
                inc[u, e] = 1;
                inc[v, e] = 1;
            }
            return inc;
        }

        private List<int>[] BuildAdjacencyList()
        {
            var list = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = new List<int>();
                for (int j = 0; j < n; j++)
                    if (adjMatrix[i, j] == 1)
                        list[i].Add(j);
            }
            return list;
        }

        public void PrintIncidenceMatrix()
        {
            Console.WriteLine("Матрица инцидентности:");
            int m = incMatrix.GetLength(1);
            Console.Write("    ");
            for (int e = 0; e < m; e++)
                Console.Write($"{e + 1,2} ");
            Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                Console.Write($"v{i + 1,2}: ");
                for (int e = 0; e < m; e++)
                    Console.Write($" {incMatrix[i, e]} ");
                Console.WriteLine();
            }
        }

        public void PrintAdjacencyList()
        {
            Console.WriteLine("\nСвязанные списки:");
            for (int i = 0; i < n; i++)
            {
                var nbrs = adjList[i].Select(v => (v + 1).ToString());
                Console.WriteLine($"v{i + 1}: {{ {string.Join(", ", nbrs)} }}");
            }
        }

        public List<int> FindPathDFS(int start, int end)
        {
            var visited = new bool[n];
            var path = new List<int>();
            bool found = DfsRec(start, end, visited, path);
            return found ? path : new List<int>();
        }

        private bool DfsRec(int u, int target, bool[] vis, List<int> path)
        {
            vis[u] = true;
            path.Add(u);
            if (u == target) return true;
            foreach (int v in adjList[u])
            {
                if (!vis[v] && DfsRec(v, target, vis, path))
                    return true;
            }
            path.RemoveAt(path.Count - 1);
            return false;
        }

        public List<int> FindPathBFS(int start, int end)
        {
            var queue = new Queue<int>();
            var prev = new int?[n];
            var visited = new bool[n];

            queue.Enqueue(start);
            visited[start] = true;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                if (u == end) break;
                foreach (int v in adjList[u])
                {
                    if (!visited[v])
                    {
                        visited[v] = true;
                        prev[v] = u;
                        queue.Enqueue(v);
                    }
                }
            }

            var path = new List<int>();
            int? cur = end;
            while (cur != null)
            {
                path.Add(cur.Value);
                cur = prev[cur.Value];
            }
            path.Reverse();
            if (path.Count > 0 && path[0] == start)
                return path;
            return new List<int>();
        }
    }

    public class RoadNetwork
    {
        private readonly int n;
        private readonly List<(int to, int weight)>[] adjList;

        public RoadNetwork(int[,] matrix)
        {
            n = matrix.GetLength(0);
            adjList = new List<(int to, int weight)>[n];
            for (int i = 0; i < n; i++)
            {
                adjList[i] = new List<(int, int)>();
                for (int j = 0; j < n; j++)
                {
                    int w = matrix[i, j];
                    if (w >= 0)
                        adjList[i].Add((j, w));
                }
            }
        }

        public List<(int vertex, int distance)> GetReachable(int start, int maxDistance)
        {
            const int INF = int.MaxValue / 2;
            var dist = new int[n];
            var used = new bool[n];
            for (int i = 0; i < n; i++) dist[i] = INF;
            dist[start] = 0;

            var pq = new PriorityQueue<(int v, int d), int>();
            pq.Enqueue((start, 0), 0);

            while (pq.Count > 0)
            {
                var (u, dU) = pq.Dequeue();
                if (used[u]) continue;
                used[u] = true;
                foreach (var (v, w) in adjList[u])
                {
                    int nd = dU + w;
                    if (nd < dist[v])
                    {
                        dist[v] = nd;
                        pq.Enqueue((v, nd), nd);
                    }
                }
            }

            var result = new List<(int, int)>();
            for (int i = 0; i < n; i++)
            {
                if (i == start) continue;
                if (dist[i] <= maxDistance)
                    result.Add((i, dist[i]));
            }
            return result;
        }
    }



}
