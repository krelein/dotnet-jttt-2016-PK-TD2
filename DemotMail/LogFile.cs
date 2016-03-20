using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DemotMail
{
    public static class LogFile
    {
        private const string Name = "jttt.log";

        public static void StartLog()
        {
            File.WriteAllText(Name, DateTime.Now.TimeOfDay.ToString() + ": " + "Start programu" + Environment.NewLine);
        }

        public static void AddLog(string text)
        {
            File.AppendAllText(Name, DateTime.Now.TimeOfDay.ToString() + ": " + text + Environment.NewLine);
        }
    }
}
