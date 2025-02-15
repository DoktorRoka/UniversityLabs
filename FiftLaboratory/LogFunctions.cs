using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifthLaboratoryIVT7
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
                    var parts = line.Split(' ', 3);
                    log.Add(new LogEntry
                    {
                        Timestamp = DateTime.Parse(parts[0]),
                        Action = parts[2].Split(' ')[0],
                        RecordInfo = parts[2].Split('"')[1]
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
