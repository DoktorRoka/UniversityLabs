using FourthLabIVT7;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("1) Вывести символы в введеном тексте (завершать ввод точкой)");
        Console.WriteLine("2) Показать номер слова в тексте (завершать ввод точкой)");
        Console.WriteLine("3) Повернуть предложение задом на перед (завершать ввод точкой)");
        Console.WriteLine("4) Вывести все строки в которых есть слово с .com; вывести строку с наименьшим кол-во пробелов (ввод 7 строк)");
        Console.WriteLine("5) Вывести все слова которые начинаются с большой буквы и заканчиваются двумя цифрами");
        Console.WriteLine("6) Перевести str в int через мат выражение (к примеру 15 + 36  = 51");
        Console.WriteLine("7) Посчитать длину всех песен, показать самую длинную и короткую песню, показать песни у которых мин. различие в длине");
        Console.WriteLine("---------------------");
        Console.WriteLine("8) Шифратор/дешифратор");
        Console.WriteLine("9) Обработчик текста(удалить гласные буквы)");
        Console.WriteLine("10) Написать регулярное выражение(смайлики в тексте)");
        Console.Write("Выбрать: ");

        int user_choice = int.Parse(Console.ReadLine());

        switch (user_choice)
        {
            case 1:
                Console.WriteLine("Ваш текст: ");
                string task_1 = Console.ReadLine();

                lab_functions.AllSymbols allSymbols = new lab_functions.AllSymbols();

                allSymbols.PrintAllSymbols_array(task_1);
                Console.WriteLine();
                Console.WriteLine();

                allSymbols.PrintAllSymbols_methods(task_1);

                break;
            case 2:
                Console.WriteLine("Ваш текст: ");
                string task_2 = Console.ReadLine();

                lab_functions.WordOrder wordOrder = new lab_functions.WordOrder();


                wordOrder.PrintWordOrder_array(task_2);
                Console.WriteLine();
                Console.WriteLine();
                wordOrder.PrintWordOrder_methods(task_2);


                break;

            case 3:
                Console.WriteLine("Ваш текст: ");
                string task_3 = Console.ReadLine();

                lab_functions.SentenceReverse SentenceReverse = new lab_functions.SentenceReverse();

                SentenceReverse.ReverseSentence_array(task_3);
                Console.WriteLine();
                Console.WriteLine();

                SentenceReverse.ReverseSentence_methods(task_3);

                break;

            case 4:
                string[] task_4 = new string[7];
                
                for (int i = 0; i < 7; i++)
                {
                    Console.Write($"{i + 1})");
                    task_4[i] = (Console.ReadLine());
                }

                //Console.WriteLine(task_4[1]);

                lab_functions.SentenceFinder sentenceFinder = new lab_functions.SentenceFinder();

                sentenceFinder.FindDotCom_array(task_4);
                Console.WriteLine();
                Console.WriteLine();

                sentenceFinder.FindDotCom_methods(task_4);

                break;

            case 5:
                Console.WriteLine("Ваш текст: ");
                string task_5 = Console.ReadLine();

                lab_functions.UsernameFilter usernameFilter = new lab_functions.UsernameFilter();

                usernameFilter.Filter_Username_array(task_5);
                Console.WriteLine();
                Console.WriteLine();

                usernameFilter.Filter_Username_regex(task_5);

                break;

            case 6:
                Console.WriteLine("Ваше матем. выражение: ");
                string task_6 = Console.ReadLine();
                
                lab_functions.BreakDownMathematic breakDown = new lab_functions.BreakDownMathematic();

                breakDown.BreakDownMathematic_regex(task_6);


                break;

            case 7:

                string[] tracklist_task7 =
                {
                    "1. Gentle Giant – Free Hand [6:15]",
                    "2. Supertramp – Child Of Vision [07:27]",
                    "3. Camel – Lawrence [10:46]",
                    "4. Yes – Don’t Kill The Whale [3:55]",
                    "5. 10CC – Notell Hotel [04:58]",
                    "6. Nektar – King Of Twilight [4:16]",
                    "7. The Flower Kings – Monsters & Men [21:19]",
                    "8. Focus – Le Clochard [1:59]",
                    "9. Pendragon – Fallen Dream And Angel [5:23]",
                    "10. Kaipa – Remains Of The Day (08:02)"

                };


                lab_functions.SongCounter songCounter = new lab_functions.SongCounter();

                songCounter.CountTime(tracklist_task7);


                break;

            case 8:
                Console.WriteLine("Выбери шифр: ");
                Console.WriteLine("1) Шифр Полибия (только анлийский язык)");
                Console.WriteLine("2) Шифр Цезаря");
                Console.WriteLine("3) Шифр Виженера");

                int cypherChoice;
                if (!int.TryParse(Console.ReadLine(), out cypherChoice) || cypherChoice < 1 || cypherChoice > 3)
                {
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
                }

                Console.WriteLine("Введите текст для шифрования/дешифрования:");
                string inputText = Console.ReadLine();

                lab_functions.TextDeCypher textDeCypher = new lab_functions.TextDeCypher();

                switch (cypherChoice)
                {
                    case 1: // Шифр Полибия
                        Console.WriteLine("1) Шифрование");
                        Console.WriteLine("2) Дешифрование");
                        int polyChoice = int.Parse(Console.ReadLine());

                        if (polyChoice == 1)
                        {
                            Console.WriteLine("Результат шифрования (Полибий):");
                            Console.WriteLine(textDeCypher.PolibiaCypher(inputText));
                        }
                        else if (polyChoice == 2)
                        {
                            Console.WriteLine("Результат дешифрования (Полибий):");
                            Console.WriteLine(textDeCypher.PolibiaDecypher(inputText));
                        }
                        else
                        {
                            Console.WriteLine("Некорректный выбор операции.");
                        }
                        break;

                    case 2: // Шифр Цезаря
                        Console.WriteLine("Введите сдвиг:");
                        int shift = int.Parse(Console.ReadLine());

                        Console.WriteLine("1) Шифрование");
                        Console.WriteLine("2) Дешифрование");
                        int ceaserChoice = int.Parse(Console.ReadLine());

                        if (ceaserChoice == 1)
                        {
                            Console.WriteLine("Результат шифрования (Цезарь):");
                            Console.WriteLine(textDeCypher.CeaserCypher(inputText, shift));
                        }
                        else if (ceaserChoice == 2)
                        {
                            Console.WriteLine("Результат дешифрования (Цезарь):");
                            Console.WriteLine(textDeCypher.CeaserDecypher(inputText, shift));
                        }
                        else
                        {
                            Console.WriteLine("Некорректный выбор операции.");
                        }
                        break;

                    case 3: // Шифр Виженера
                        Console.WriteLine("Введите ключ:");
                        string key = Console.ReadLine();

                        Console.WriteLine("1) Шифрование");
                        Console.WriteLine("2) Дешифрование");
                        int vigenereChoice = int.Parse(Console.ReadLine());

                        if (vigenereChoice == 1)
                        {
                            Console.WriteLine("Результат шифрования (Виженер):");
                            Console.WriteLine(textDeCypher.VigenereCypher(inputText, key));
                        }
                        else if (vigenereChoice == 2)
                        {
                            Console.WriteLine("Результат дешифрования (Виженер):");
                            Console.WriteLine(textDeCypher.VigenereDecypher(inputText, key));
                        }
                        else
                        {
                            Console.WriteLine("Некорректный выбор операции.");
                        }
                        break;

                    default:
                        Console.WriteLine("Некорректный выбор шифра.");
                        break;
                }

                break;

            case 9:
                Console.WriteLine("Ваш текст: ");
                string task_9 = Console.ReadLine();

                lab_functions.TextManager textManager = new lab_functions.TextManager();

                textManager.ManageText_array(task_9);
                Console.WriteLine();
                Console.WriteLine();

                textManager.ManageText_methods(task_9);


                break;

            case 10:
                Console.WriteLine("Введите текст: ");

                string task_10 = Console.ReadLine();


                lab_functions.SmileFinder smileFinder = new lab_functions.SmileFinder();

                smileFinder.findSmiles(task_10);



                break;


        }

    }
}