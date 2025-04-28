using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba9_variant7_Part3
{
    internal class LabFunc
    {
        public void MatVirazh()
        {
            Console.Write("Введите математическое выражение: ");
            string expression = Console.ReadLine();

            var stack = new Stack<char>();
            bool isValid = true;
            foreach (char c in expression)
            {
                if (c == '(')
                {
                    stack.Push(c);
                }
                else if (c == ')')
                {
                    if (stack.Count == 0)
                    {
                        isValid = false;  // лишняя закрывающая
                        break;
                    }
                    stack.Pop();
                }
            }
            if (stack.Count > 0) // остались незакрытые
                isValid = false;

            if (isValid)
                Console.WriteLine($"\"{expression}\" — корректно.");
            else
                Console.WriteLine($"\"{expression}\" — некорректно.");
        }
                        
    }

    internal class CountingGame
    {
        private class Node
        {
            public string Name;
            public Node Next;
            public Node(string name) => Name = name;
        }

        public void Run()
        {
            string[] names = new[]
            {
                "Иван",
                "Мария",
                "Сергей",
                "Елена",
                "Дмитрий",
                "Ольга"
            };
            Node head = null, prev = null;
            foreach (var n in names)
            {
                var node = new Node(n);
                if (head == null)
                    head = node;
                else
                    prev.Next = node;

                prev = node;
            }
            prev.Next = head;

            Console.Write("Введите строку-считалку (через пробел): ");
            var rhyme = Console.ReadLine()
                              .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (rhyme.Length == 0)
            {
                Console.WriteLine("Пустая считалка. Выход.");
                return;
            }

            Console.Write("Введите имя того, с кого начинаем: ");
            string startName = Console.ReadLine();

            Node current = head;
            bool found = false;
            do
            {
                if (string.Equals(current.Name, startName, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
                current = current.Next;
            } while (current != head);

            if (!found)
            {
                Console.WriteLine($"Имя \"{startName}\" не найдено — начнём с \"{head.Name}\".");
                current = head;
            }

            int steps = rhyme.Length;
            for (int i = 1; i < steps; i++)
            {
                current = current.Next;
            }

            Console.WriteLine();
            Console.WriteLine($"По слову \"{rhyme[^1]}\" последний выпадет: {current.Name}");
        }
    }

    internal class CombinationFinder
    {
        public void Run()
        {
            const int maxN = 50000;
            int maxA = (int)Math.Ceiling(Math.Pow(maxN, 1.0 / 3));

            // Словарь: ключ = сумма кубов, значение = число способов её получить
            var combinations = new Dictionary<int, int>();

            for (int x = 0; x <= maxA; x++)
            {
                int x3 = x * x * x;
                for (int y = 0; y <= maxA; y++)
                {
                    int sum2 = x3 + y * y * y;
                    for (int z = 0; z <= maxA; z++)
                    {
                        int sum = sum2 + z * z * z;
                        if (sum < 1 || sum > maxN)
                            continue;

                        if (combinations.ContainsKey(sum))
                            combinations[sum]++;
                        else
                            combinations[sum] = 1;
                    }
                }
            }

            Console.WriteLine("Числа N от 1 до 50000, имеющие ≥3 комбинаций суммы кубов:");
            foreach (var kv in combinations)
            {
                if (kv.Value >= 3)
                    Console.WriteLine($"{kv.Key} -> {kv.Value} комбинаций");
            }
        }
    }

    internal class WordFrequencyAnalyzer
    {
       
        public void Run()
        {
            string path = "task3.txt";

            

            string text = File.ReadAllText(path);
            var words = text
                .Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLowerInvariant());

            var freq = new Dictionary<string, int>();
            foreach (var w in words)
            {
                if (freq.ContainsKey(w))
                    freq[w]++;
                else
                    freq[w] = 1;
            }

            var top10 = freq
                .OrderByDescending(kv => kv.Value)
                .ThenBy(kv => kv.Key)
                .Take(10);

            Console.WriteLine();
            Console.WriteLine("10 наиболее часто встречаемых слов:");
            int rank = 1;
            foreach (var kv in top10)
            {
                Console.WriteLine($"{rank,2}. {kv.Key} — {kv.Value} раз");
                rank++;
            }
        }
    }


    internal class PlayoffBracket
    {
        private class MatchNode
        {
            public string TeamA;
            public string TeamB;
            public int ScoreA;
            public int ScoreB;
            public MatchNode Left;   // предыдущий раунд, левая пара
            public MatchNode Right;  // предыдущий раунд, правая пара

            public string Winner => ScoreA > ScoreB ? TeamA : TeamB;
        }

        private readonly Random _rnd = new Random();

        public void Run()
        {
            string[] teams = new[]
            {
                "BRA","CHI","URU","COL",
                "FRA","NIG","ALG","GER",
                "CRC","GRE","NED","MEX",
                "ARG","SWI","BEL","USA"
            };

            MatchNode[] roundOf16 = new MatchNode[8];
            for (int i = 0; i < 8; i++)
            {
                var m = new MatchNode
                {
                    TeamA = teams[2 * i],
                    TeamB = teams[2 * i + 1]
                };
                Simulate(m);
                roundOf16[i] = m;
            }

            MatchNode[] quarters = new MatchNode[4];
            for (int i = 0; i < 4; i++)
            {
                var m = new MatchNode
                {
                    Left = roundOf16[2 * i],
                    Right = roundOf16[2 * i + 1],
                    TeamA = roundOf16[2 * i].Winner,
                    TeamB = roundOf16[2 * i + 1].Winner
                };
                Simulate(m);
                quarters[i] = m;
            }

            MatchNode[] semis = new MatchNode[2];
            for (int i = 0; i < 2; i++)
            {
                var m = new MatchNode
                {
                    Left = quarters[2 * i],
                    Right = quarters[2 * i + 1],
                    TeamA = quarters[2 * i].Winner,
                    TeamB = quarters[2 * i + 1].Winner
                };
                Simulate(m);
                semis[i] = m;
            }

            var final = new MatchNode
            {
                Left = semis[0],
                Right = semis[1],
                TeamA = semis[0].Winner,
                TeamB = semis[1].Winner
            };
            Simulate(final);

            PrintMatch(final, "");
        }

        private void Simulate(MatchNode m)
        {
            do
            {
                m.ScoreA = _rnd.Next(0, 5);
                m.ScoreB = _rnd.Next(0, 5);
            } while (m.ScoreA == m.ScoreB);
        }

        private void PrintMatch(MatchNode m, string indent)
        {
            Console.WriteLine($"{indent}{m.TeamA} - {m.TeamB} : {m.ScoreA} - {m.ScoreB}");
            if (m.Left != null && m.Right != null)
            {
                PrintMatch(m.Left, indent + "    ");
                PrintMatch(m.Right, indent + "    ");
            }
        }
    }

    internal class CustomListReverser
    {
        public void Run()
        {
            var source = new DoublyLinkedList<int>();
            var reversed = new DoublyLinkedList<int>();

            Console.Write("Введите количество элементов исходного списка: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n < 1)
            {
                Console.WriteLine("Некорректное число.");
                return;
            }

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Элемент #{i + 1}: ");
                if (int.TryParse(Console.ReadLine(), out int x))
                    source.AddLast(x);
                else
                {
                    Console.WriteLine("Некорректный ввод, считаем 0.");
                    source.AddLast(0);
                }
            }

            for (int i = source.Count - 1; i >= 0; i--)
            {
                reversed.AddLast(source.GetAt(i));
            }

            Console.WriteLine("\nИсходный список:");
            foreach (var x in source)
                Console.Write(x + " ");
            Console.WriteLine("\nСписок в обратном порядке:");
            foreach (var x in reversed)
                Console.Write(x + " ");
            Console.WriteLine();
        }
    }

    internal class DotNetListReverser
    {
        public void Run()
        {
            var source = new List<int>();
            var reversed = new List<int>();

            Console.Write("Введите количество элементов исходного списка: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n < 1)
            {
                Console.WriteLine("Некорректное число.");
                return;
            }

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Элемент #{i + 1}: ");
                if (int.TryParse(Console.ReadLine(), out int x))
                    source.Add(x);
                else
                {
                    Console.WriteLine("Некорректный ввод, считаем 0.");
                    source.Add(0);
                }
            }

            for (int i = source.Count - 1; i >= 0; i--)
            {
                reversed.Add(source[i]);
            }

            Console.WriteLine("\nИсходный список:");
            Console.WriteLine(string.Join(" ", source));
            Console.WriteLine("Список в обратном порядке:");
            Console.WriteLine(string.Join(" ", reversed));
        }
    }


    internal class DenseMatrix
    {
        private int[,] _data;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public void FillFromConsole()
        {
            Console.Write("Число строк: ");
            Rows = int.Parse(Console.ReadLine());
            Console.Write("Число столбцов: ");
            Cols = int.Parse(Console.ReadLine());

            _data = new int[Rows, Cols];
            Console.WriteLine("Введите элементы матрицы построчно (в основном нули):");
            for (int i = 0; i < Rows; i++)
            {
                var parts = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < Math.Min(Cols, parts.Length); j++)
                    _data[i, j] = int.Parse(parts[j]);
            }
        }

        public void Print()
        {
            Console.WriteLine("Матрица:");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                    Console.Write($"{_data[i, j],4}");
                Console.WriteLine();
            }
        }

        public DenseMatrix Transpose()
        {
            var t = new DenseMatrix
            {
                Rows = Cols,
                Cols = Rows,
                _data = new int[Cols, Rows]
            };
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    t._data[j, i] = _data[i, j];
            return t;
        }

        public DenseMatrix Add(DenseMatrix other)
        {
            if (Rows != other.Rows || Cols != other.Cols)
                throw new InvalidOperationException("Размеры не совпадают");
            var sum = new DenseMatrix
            {
                Rows = Rows,
                Cols = Cols,
                _data = new int[Rows, Cols]
            };
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    sum._data[i, j] = _data[i, j] + other._data[i, j];
            return sum;
        }

        public DenseMatrix Multiply(DenseMatrix other)
        {
            if (Cols != other.Rows)
                throw new InvalidOperationException("Нельзя умножить: inner dimensions mismatch");
            var prod = new DenseMatrix
            {
                Rows = Rows,
                Cols = other.Cols,
                _data = new int[Rows, other.Cols]
            };
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < other.Cols; j++)
                    for (int k = 0; k < Cols; k++)
                        prod._data[i, j] += _data[i, k] * other._data[k, j];
            return prod;
        }
    }


    internal class SparseMatrix
    {
        private class Node
        {
            public int Row, Col, Value;
            public Node Next;
            public Node(int r, int c, int v) { Row = r; Col = c; Value = v; }
        }

        private Node _head;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public void FillFromConsole()
        {
            Console.Write("Число строк: ");
            Rows = int.Parse(Console.ReadLine());
            Console.Write("Число столбцов: ");
            Cols = int.Parse(Console.ReadLine());

            Console.Write("Сколько ненулевых элементов вводить? ");
            int cnt = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите тройки (строка столбец значение):");
            for (int i = 0; i < cnt; i++)
            {
                var parts = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int r = int.Parse(parts[0]),
                    c = int.Parse(parts[1]),
                    v = int.Parse(parts[2]);
                AddNode(r, c, v);
            }
        }

        private void AddNode(int r, int c, int v)
        {
            var n = new Node(r, c, v);
            n.Next = _head;
            _head = n;
        }

        public void Print()
        {
            Console.WriteLine("Разреженная матрица (только ненулевые):");
            for (var p = _head; p != null; p = p.Next)
                Console.WriteLine($"[{p.Row},{p.Col}] = {p.Value}");
        }

        public SparseMatrix Transpose()
        {
            var t = new SparseMatrix { Rows = Cols, Cols = Rows };
            for (var p = _head; p != null; p = p.Next)
                t.AddNode(p.Col, p.Row, p.Value);
            return t;
        }

        public SparseMatrix Add(SparseMatrix other)
        {
            if (Rows != other.Rows || Cols != other.Cols)
                throw new InvalidOperationException("Размеры не совпадают");

            var sum = new SparseMatrix { Rows = Rows, Cols = Cols };
            for (var p = _head; p != null; p = p.Next)
                sum.AddNode(p.Row, p.Col, p.Value);
            for (var p = other._head; p != null; p = p.Next)
                sum.AddNode(p.Row, p.Col, p.Value);
            return sum;
        }

        public SparseMatrix Multiply(SparseMatrix other)
        {
            if (Cols != other.Rows)
                throw new InvalidOperationException("Нельзя умножить");

            var prod = new SparseMatrix { Rows = Rows, Cols = other.Cols };
            // O(n в кубе) если спросят
            
            for (var a = _head; a != null; a = a.Next)
                for (var b = other._head; b != null; b = b.Next)
                    if (a.Col == b.Row)
                        prod.AddNode(a.Row, b.Col, a.Value * b.Value);
            return prod;
        }
    }


}
