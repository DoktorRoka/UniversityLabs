using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics.Tracing;

namespace FourthLabIVT7
{
    internal class lab_functions
    {
        public class AllSymbols
        {
            public void PrintAllSymbols_array(string input_string) // через массив
            {
                input_string = input_string.TrimEnd('.');
                // через обработку строки
                char[] characters = input_string.ToCharArray();
                int[] counts = new int[char.MaxValue];

                foreach (char c in characters)
                {
                    if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))
                    {
                        counts[c]++;
                    }
                }

                Console.WriteLine("Символы, которые встречаются ровно один раз:");
                foreach (char c in characters)
                {
                    if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c) && counts[c] == 1)
                    {
                        Console.Write(c + " ");

                    }
                }
            }
            public void PrintAllSymbols_methods(string input_string) // через функции
            {
                input_string = input_string.TrimEnd('.');

                var FilteredCharacters = input_string.Where(c => !char.IsWhiteSpace(c) && !char.IsPunctuation(c));

                var CharacterCounts = FilteredCharacters.GroupBy(c => c).Where(g => g.Count() == 1).Select(g => g.Key);


                Console.WriteLine("Через функции: ");
                foreach (char c in input_string)
                {
                    if (CharacterCounts.Contains(c) && !char.IsWhiteSpace(c) && !char.IsPunctuation(c))
                    {
                        Console.WriteLine(c + " ");
                    }
                }


            }
        }

        public class WordOrder
        {
            public void PrintWordOrder_array(string input_string) // через массивы
            {
                input_string = input_string.TrimEnd('.');

                var regex_matches = Regex.Matches(input_string, @"\w+|[^\w\s]|\s+");

                int word_counter = 0;
                string output_string = "";

                foreach (Match match in regex_matches)
                {
                    string part = match.Value;

                    if (Regex.IsMatch(part, @"\w+"))
                    {
                        word_counter++;
                        output_string += $"{part}({word_counter})";
                    }
                    else
                    {
                        output_string += part;
                    }
                }
                Console.WriteLine("Через массивы: ");
                Console.WriteLine(output_string.Trim());


            }

            public void PrintWordOrder_methods(string input_string) // через функции string
            {
                input_string = input_string.TrimEnd('.');

                int word_counter = 0;
                string output_string = "";
                string current_word = "";

                for (int i = 0; i < input_string.Length; i++)
                {
                    char current_char = input_string[i];

                    if (char.IsLetterOrDigit(current_char))
                    {
                        current_word += current_char;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(current_word))
                        {
                            word_counter++;
                            output_string += $"{current_word}({word_counter})";
                            current_word = "";
                        }

                        output_string += current_char;
                    }
                }

                if (!string.IsNullOrEmpty(current_word))
                {
                    word_counter++;
                    output_string += $"{current_word}({word_counter})";
                }

                output_string = output_string.Trim();

                Console.WriteLine("Через функции string: ");
                Console.WriteLine(output_string);

            }

        }

        public class SentenceReverse
        {
            public void ReverseSentence_array(string input_string) // массив
            {
                input_string = input_string.TrimEnd('.');

                var string_array = input_string.Split(' ');

                Array.Reverse(string_array);

                var reversed_string = string.Join(" ", string_array);

                Console.WriteLine("Через массив: ");

                Console.WriteLine(reversed_string);


            }

            public void ReverseSentence_methods(string input_string) //методы
            {
                input_string = input_string.TrimEnd('.');

                var splited_string = input_string.Split(" ");

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = splited_string.Length - 1; i >= 0; i--)
                {
                    stringBuilder.Append(splited_string[i]);
                    if (i > 0)
                    {
                        stringBuilder.Append(" ");
                    }
                }


                Console.WriteLine("Через методы: ");

                Console.WriteLine(stringBuilder.ToString());



            }
        }

        public class SentenceFinder
        {
            public void FindDotCom_array(string[] input_string) //массив
            {
                string reg_pattern = @"\.com$";
                string[] temp_list = new string[input_string.Length];
                int counter = 0;
                int minspaces = int.MaxValue;
                string sentence_with_min_spaces = "";
                int line_with_min_space = -1;



                for (int i = 0; i < input_string.Length; i++)
                {
                    string line = input_string[i];

                    if (Regex.IsMatch(line, reg_pattern, RegexOptions.IgnoreCase))
                    {
                        temp_list[counter] = line;
                        counter++;
                    }

                    int spaceCount = CountSpaces(line);
                    if (spaceCount < minspaces)
                    {
                        minspaces = spaceCount;
                        sentence_with_min_spaces = line;
                        line_with_min_space = i + 1;
                    }

                }

                Console.WriteLine("Через массив: ");
                for (int i = 0; i < counter; i++)
                {
                    Console.WriteLine($"{i + 1}) {temp_list[i]}");
                }

                Console.WriteLine($"Предложение с найменьшим кол-во пробелов: {line_with_min_space}) {sentence_with_min_spaces}");

            }

            public void FindDotCom_methods(string[] input_strings) //методы
            {
                string reg_pattern = @"\b\w+\.com\b";
                Regex regex = new Regex(reg_pattern, RegexOptions.IgnoreCase);

                string sentenceWithMinSpaces = "";
                int minSpaces = int.MaxValue;
                int lineWithMinSpaces = -1;

                Console.WriteLine("Через методы: ");

                for (int i = 0; i < input_strings.Length; i++)
                {
                    string line = input_strings[i];

                    if (regex.IsMatch(line))
                    {
                        Console.WriteLine($"{i + 1}) {line}");
                    }

                    int spaceCount = line.Split(' ').Length - 1;
                    if (spaceCount < minSpaces)
                    {
                        minSpaces = spaceCount;
                        sentenceWithMinSpaces = line;
                        lineWithMinSpaces = i + 1;
                    }
                }

                Console.WriteLine($"Предложение с наименьшим кол-во пробелов: {lineWithMinSpaces}) {sentenceWithMinSpaces}");
            }



            private int CountSpaces(string sentence)
            {
                int my_spacecounter = 0;
                foreach (char c in sentence)
                {
                    if (c == ' ')
                    {
                        my_spacecounter++;
                    }
                }
                return my_spacecounter;

            }
        }

        public class UsernameFilter
        {
            public void Filter_Username_array(string input_string) //массив
            {
                string[] words = input_string.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("Через массив: ");

                foreach (var word in words)
                {
                    if (word.Length >= 3 && char.IsUpper(word[0]) && char.IsDigit(word[word.Length - 1]) && char.IsDigit(word[word.Length - 2]))
                    {
                        Console.WriteLine(word);
                    }
                }

            }

            public void Filter_Username_regex(string input_text) //регулярка
            {
                string pattern = @"\b[A-Z][a-zA-Z]*\d{2}\b";

                Regex regex = new Regex(pattern);

                Console.WriteLine("Через регулярные выражения:");

                MatchCollection matches = regex.Matches(input_text);

                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Value);
                }


            }




        }

        public class BreakDownMathematic
        {
            public void BreakDownMathematic_regex(string inputString)
            {
                string pattern = @"\s*(-?\d+)\s*([\+\-\*/])\s*(-?\d+)\s*=\s*(-?\d+)\s*";
                /* разбор чтобы не забыть:

                \s* - не забываем про пробелы

                (-?\d+) - захватывает первое число, знак минуса что он может быть, d одна и более цифры
                */

                // ([\+\-\*/]) - захват оператора (+ - * /)

                // = - равенство в примере


                Match match = Regex.Match(inputString, pattern);

                if (match.Success)
                {
                    int operand1 = int.Parse(match.Groups[1].Value);
                    string operation = match.Groups[2].Value;
                    int operand2 = int.Parse(match.Groups[3].Value);
                    int result = int.Parse(match.Groups[4].Value);

                    Console.WriteLine($"Операнд 1: {operand1}");
                    Console.WriteLine($"Операция: {operation}");
                    Console.WriteLine($"Операнд 2: {operand2}");
                    Console.WriteLine($"результат: {result}");

                }


            }


        }

        public class SongCounter
        {
            public void CountTime(string[] inputTracklist)
            {
                var songDurations = new List<(string Title, TimeSpan Duration)>();
                string pattern = @"^.*?\[(\d+):(\d+)\]|\((\d+):(\d+)\)$";

                foreach (var track in inputTracklist)
                {
                    var match = Regex.Match(track, pattern);
                    if (match.Success)
                    {
                        int minutes = int.Parse(match.Groups[1].Value != "" ? match.Groups[1].Value : match.Groups[3].Value);
                        int seconds = int.Parse(match.Groups[2].Value != "" ? match.Groups[2].Value : match.Groups[4].Value);
                        TimeSpan duration = new TimeSpan(0, minutes, seconds);

                        string title = track.Split(new[] { " – " }, StringSplitOptions.None)[1].Split('[')[0].Trim();
                        songDurations.Add((title, duration));
                    }
                }

                TimeSpan totalDuration = songDurations.Select(s => s.Duration).Aggregate((t1, t2) => t1 + t2);

                var longestSong = songDurations.OrderByDescending(s => s.Duration).First();
                var shortestSong = songDurations.OrderBy(s => s.Duration).First();

                var minDiffPair = songDurations
                    .SelectMany((s1, i) => songDurations.Skip(i + 1), (s1, s2) => (s1, s2))
                    .OrderBy(pair => Math.Abs((pair.s1.Duration - pair.s2.Duration).TotalSeconds))
                    .First();

                Console.WriteLine($"Общее время: {totalDuration}");
                Console.WriteLine($"Самая длинная песня: {longestSong.Title} ({longestSong.Duration})");
                Console.WriteLine($"Самая короткая песня: {shortestSong.Title} ({shortestSong.Duration})");
                Console.WriteLine($"Пара с минимальной разницей: {minDiffPair.s1.Title} ({minDiffPair.s1.Duration}) и {minDiffPair.s2.Title} ({minDiffPair.s2.Duration})");

            }

        }

        public class TextDeCypher
        {



            private const string EngAlphabet = "abcdefghijklmnopqrstuvwxyz";
            private const string RusAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            public string PolibiaCypher(string inputText)
            {
                return ApplyPolibia(inputText, true);
            }

            public string PolibiaDecypher(string inputText)
            {
                return ApplyPolibia(inputText, false);
            }

            public string CeaserCypher(string inputText, int shift = 3)
            {
                return ApplyCeaser(inputText, shift);
            }

            public string CeaserDecypher(string inputText, int shift = 3)
            {
                return ApplyCeaser(inputText, -shift);
            }

            public string VigenereCypher(string inputText, string key)
            {
                return ApplyVigenere(inputText, key, true);
            }

            public string VigenereDecypher(string inputText, string key)
            {
                return ApplyVigenere(inputText, key, false);
            }


            private string ApplyCeaser(string inputText, int shift)
            {
                var result = new StringBuilder();

                foreach (var ch in inputText)
                {
                    string alphabet = char.IsLower(ch) ? EngAlphabet : EngAlphabet.ToUpper();
                    if (RusAlphabet.Contains(char.ToLower(ch)))
                        alphabet = char.IsLower(ch) ? RusAlphabet : RusAlphabet.ToUpper();

                    if (alphabet.Contains(ch))
                    {
                        int oldIndex = alphabet.IndexOf(ch);
                        int newIndex = (oldIndex + shift + alphabet.Length) % alphabet.Length;
                        result.Append(alphabet[newIndex]);
                    }
                    else
                    {
                        result.Append(ch);
                    }
                }

                return result.ToString();
            }

            private string ApplyVigenere(string inputText, string key, bool isEncrypt)
            {
                var result = new StringBuilder();
                int keyIndex = 0;

                foreach (var ch in inputText)
                {
                    string alphabet = char.IsLower(ch) ? EngAlphabet : EngAlphabet.ToUpper();
                    if (RusAlphabet.Contains(char.ToLower(ch)))
                        alphabet = char.IsLower(ch) ? RusAlphabet : RusAlphabet.ToUpper();

                    if (alphabet.Contains(ch))
                    {
                        int charIndex = alphabet.IndexOf(ch);
                        int keyCharIndex = alphabet.IndexOf(key[keyIndex % key.Length]);
                        int shift = isEncrypt ? keyCharIndex : -keyCharIndex;
                        int newIndex = (charIndex + shift + alphabet.Length) % alphabet.Length;
                        result.Append(alphabet[newIndex]);
                        keyIndex++;
                    }
                    else
                    {
                        result.Append(ch);
                    }
                }
                return result.ToString();


            }

            private string ApplyPolibia(string text, bool isEncrypt)
            {
                string alphabet = RusAlphabet.Contains(char.ToLower(text[0])) ? RusAlphabet : EngAlphabet;
                var square = CreateSquare(alphabet);
                var result = new StringBuilder();

                if (isEncrypt)
                {
                    text = text.ToUpper().Replace("Ё", "Е").Replace("J", "I");
                    foreach (char c in text)
                    {
                        if (!alphabet.Contains(char.ToLower(c))) continue;
                        for (int i = 0; i < square.GetLength(0); i++)
                        {
                            for (int j = 0; j < square.GetLength(1); j++)
                            {
                                if (square[i, j] == char.ToLower(c))
                                {
                                    result.Append($"{i + 1}{j + 1} ");
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var codes = text.Split(' ');
                    foreach (var pair in codes)
                    {
                        if (pair.Length != 2) continue;
                        int row = int.Parse(pair[0].ToString()) - 1;
                        int col = int.Parse(pair[1].ToString()) - 1;
                        result.Append(square[row, col]);
                    }
                }

                return result.ToString().Trim();
            }

            private char[,] CreateSquare(string alphabet)
            {
                int size = (int)Math.Ceiling(Math.Sqrt(alphabet.Length));
                char[,] square = new char[size, size];
                int index = 0;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size && index < alphabet.Length; j++)
                    {
                        square[i, j] = alphabet[index++];
                    }
                }

                return square;

            }
        }

        public class TextManager
        {
            public void ManageText_array(string inputText) // массив
            {
                var delimiters = new[] { ',', '.', '!', '?', ' ' };
                var words = inputText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                string result = "";

                foreach (string word in words)
                {
                    if (isGlasnaya(word[0]) && isGlasnaya(word[word.Length - 1]))
                    {
                        continue; 
                    }

                    result += word + " "; 
                }

                result.Trim();
                Console.WriteLine(result);

            }

            public void ManageText_methods(string input_text) // методы
            {
                var delimiters = new[] { ',', '.', '!', '?', ' '};
                var words = input_text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder result = new StringBuilder();

                foreach (string word in words)
                {
                    if (!(isGlasnaya(word[0]) && isGlasnaya(word[word.Length - 1])))
                    {
                        result.Append(word).Append(" ");
                    }
                }

                var my_result = result.ToString().TrimEnd();

                Console.WriteLine("Через методы: ");
                Console.WriteLine(my_result);

            }

            private bool isGlasnaya(char c)
            {
                char lowerChar = char.ToLower(c);
                return "aeiou".Contains(lowerChar);
            }


        }


        public class SmileFinder
        {
            public void findSmiles(string input_text)
            {
                string pattern = @"(:-\)|:\)|\)+)";

                MatchCollection matches = Regex.Matches(input_text, pattern);

                Console.WriteLine("Смайлики в тексте: ");
                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Value);
                }

            }
        }




    }
}
