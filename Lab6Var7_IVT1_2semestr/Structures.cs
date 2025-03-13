using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6Var7_IVT1_2semestr
{
    public struct Participant
    {
        public string FullName;
        public char Type; // T - тренер, C - спортсмен
        public int BirthYear;
        public int Experience;

        public override string ToString()
        {
            return $"{FullName},{Type},{BirthYear},{Experience}";
        }

        public static Participant FromString(string line)
        {
            var parts = line.Split(',');
            return new Participant
            {
                FullName = parts[0],
                Type = parts[1][0],
                BirthYear = int.Parse(parts[2]),
                Experience = int.Parse(parts[3])
            };
        }
    }
    public struct LogEntry
    {
        public DateTime Timestamp { get; set; } 
        public string Action { get; set; }      
        public string RecordInfo { get; set; } 

        public override string ToString()
        {
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss} - {Action}: {RecordInfo}";
        }

        public static LogEntry FromString(string line)
        {
            var parts = line.Split(" - ");
            var actionAndInfo = parts[1].Split(':');
            return new LogEntry
            {
                Timestamp = DateTime.Parse(parts[0]),
                Action = actionAndInfo[0].Trim(),
                RecordInfo = actionAndInfo[1].Trim()
            };
        }
    }
}
