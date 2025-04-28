using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Laba9_variant7_Part1
{
    public class LogFunctions
    {
        public void ViewLog(List<LogEntry> log, DateTime lastActionTime)
        {
            foreach (var entry in log)
            {
                Console.WriteLine(entry);
            }

            Console.WriteLine("\nСамый долгий период бездействия пользователя:");
            Console.WriteLine((DateTime.Now - lastActionTime).ToString("hh\\:mm\\:ss"));
        }

        public void LogAction(List<LogEntry> log, string action, string recordInfo, ref DateTime lastActionTime)
        {
            log.Add(new LogEntry
            {
                Action = action,
                RecordInfo = recordInfo,
                Timestamp = DateTime.Now
            });

            if (log.Count > 50) log.RemoveAt(0);
            lastActionTime = DateTime.Now;
        }

         public List<LogEntry> LoadLog(string filePath)
         {
             var log = new List<LogEntry>();
             if (File.Exists(filePath))
             {
                 var lines = File.ReadAllLines(filePath);
                 foreach (var line in lines)
                 {
                     var parts = line.Split(" - ", StringSplitOptions.None);
                     if (parts.Length != 2)
                     {
                         Console.WriteLine("Неверный формат строки: " + line);
                         continue;
                     }
        
                     if (!DateTime.TryParse(parts[0], out DateTime timestamp))
                     {
                         Console.WriteLine("Неверный формат даты: " + parts[0]);
                         continue;
                     }
        
                     var subParts = parts[1].Split(": ", StringSplitOptions.None);
                     if (subParts.Length != 2)
                     {
                         Console.WriteLine("Неверный формат действия и данных: " + parts[1]);
                         continue;
                     }
        
                     string action = subParts[0];      
                     string recordInfo = subParts[1];    
        
                     log.Add(new LogEntry
                     {
                         Timestamp = timestamp,
                         Action = action,
                         RecordInfo = recordInfo
                     });
                 }
             }
             return log;
         }

        public void SaveLog(List<LogEntry> log, string filePath)
        {
            File.WriteAllLines(filePath, log.Select(l => l.ToString()));
        }
    }
}
