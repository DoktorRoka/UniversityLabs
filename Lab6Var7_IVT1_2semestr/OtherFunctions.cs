using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6Var7_IVT1_2semestr
{
    internal class OtherFunctions
    {
        public class BinaryFileHandler
        {
            public void ProcessBinaryFiles(string file1, string file2)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(file1, FileMode.Create)))
                {
                    for (int M = 1; M <= 5; M++)
                    {
                        for (int N = 1; N <= 5; N++)
                        {
                            int result = (int)Math.Pow(M, N);
                            writer.Write(M);
                            writer.Write(N);
                            writer.Write(result);
                        }
                    }
                }

                using (BinaryReader reader = new BinaryReader(File.Open(file1, FileMode.Open)))
                using (BinaryWriter writer = new BinaryWriter(File.Open(file2, FileMode.Create)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        reader.ReadInt32(); 
                        reader.ReadInt32(); 
                        int result = reader.ReadInt32(); 
                        writer.Write(result); 
                    }
                }
            }

            public void ViewFileContents(string file1, string file2)
            {
                Console.WriteLine($"Содержимое файла {file1}:");
                using (BinaryReader reader = new BinaryReader(File.Open(file1, FileMode.Open)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        int M = reader.ReadInt32();
                        int N = reader.ReadInt32();
                        int power = reader.ReadInt32();
                        Console.WriteLine($"M: {M}, N: {N}, M^N: {power}");
                    }
                }

                Console.WriteLine(); 

                Console.WriteLine($"Содержимое файла {file2}:");
                using (BinaryReader reader = new BinaryReader(File.Open(file2, FileMode.Open)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        int power = reader.ReadInt32();
                        Console.WriteLine($"M^N: {power}");
                    }
                }
            }
        }


        public class TextFileProcessor
        {
            public void ProcessTextFile(string inputFilePath, string outputFilePath)
            {
                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine("Входной файл не найден.");
                    return;
                }

                string[] lines = File.ReadAllLines(inputFilePath);
                int emptyLineCount = 0;
                string copyrightText = "(c) Student";

                List<string> processedLines = new List<string>();

                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        emptyLineCount++;
                        processedLines.Add(line); 
                    }
                    else
                    {
                        processedLines.Add(line + " " + copyrightText);
                    }
                }

                File.WriteAllLines(outputFilePath, processedLines);

                Console.WriteLine("Количество пустых строк в файле: " + emptyLineCount);
            }
        }

        public class LabFileProcessor
        {
            public void ProcessLabFile()
            {
                string currentDir = Directory.GetCurrentDirectory();

                string tempDir = Path.Combine(currentDir, "Lab6_Temp");
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                string sourceFile = Path.Combine(currentDir, "lab.dat");
                string destFile = Path.Combine(tempDir, "lab.dat");
                string backupFile = Path.Combine(tempDir, "lab_backup.dat");

                if (!File.Exists(sourceFile))
                {
                    Console.WriteLine("Файл lab.dat не найден в текущей директории.");
                    return;
                }

                File.Copy(sourceFile, destFile, true);

                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                using (FileStream destStream = new FileStream(backupFile, FileMode.Create, FileAccess.Write))
                {
                    int byteValue;
                    while ((byteValue = sourceStream.ReadByte()) != -1)
                    {
                        destStream.WriteByte((byte)byteValue);
                    }
                }

                FileInfo fileInfo = new FileInfo(sourceFile);
                Console.WriteLine($"Файл: {sourceFile}");
                Console.WriteLine($"Размер: {fileInfo.Length} байт");
                Console.WriteLine($"Время последнего изменения: {fileInfo.LastWriteTime}");
                Console.WriteLine($"Время последнего доступа: {fileInfo.LastAccessTime}");
            }
        }

        public class BmpFileInfo
        {
            public void ReadBmpHeader(string fileName)
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Файл не найден.");
                    return;
                }

                try
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        char b = reader.ReadChar();
                        char m = reader.ReadChar();
                        string signature = $"{b}{m}";
                        if (signature != "BM")
                        {
                            Console.WriteLine("Это не BMP файл.");
                            return;
                        }

                        uint fileSize = reader.ReadUInt32();

                        reader.ReadUInt16();
                        reader.ReadUInt16();

                        uint dataOffset = reader.ReadUInt32();

                        uint headerSize = reader.ReadUInt32();

                        int width = reader.ReadInt32();

                        int height = reader.ReadInt32();

                        ushort planes = reader.ReadUInt16();

                        ushort bitsPerPixel = reader.ReadUInt16();

                        uint compression = reader.ReadUInt32();

                        uint imageSize = reader.ReadUInt32();

                        int hResolution = reader.ReadInt32();

                        int vResolution = reader.ReadInt32();

                        uint colorsUsed = reader.ReadUInt32();

                        uint importantColors = reader.ReadUInt32();

                        Console.WriteLine($"\nИнформация о файле: {fileName}");
                        Console.WriteLine($"Размер файла: {fileSize} байт");
                        Console.WriteLine($"Ширина: {width} пикселей");
                        Console.WriteLine($"Высота: {height} пикселей");
                        Console.WriteLine($"Бит на пиксель: {bitsPerPixel}");
                        Console.WriteLine($"Горизонтальное разрешение: {hResolution} пикселей/м");
                        Console.WriteLine($"Вертикальное разрешение: {vResolution} пикселей/м");

                        string compressionType;
                        switch (compression)
                        {
                            case 0:
                                compressionType = "без сжатия";
                                break;
                            case 1:
                                compressionType = "8бит RLE";
                                break;
                            case 2:
                                compressionType = "4бит RLE";
                                break;
                            default:
                                compressionType = $"неизвестный ({compression})";
                                break;
                        }
                        Console.WriteLine($"Тип сжатия: {compressionType}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при чтении файла: " + ex.Message);
                }
            }
        }

    }
}
