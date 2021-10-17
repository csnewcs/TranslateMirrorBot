using System;
using System.IO;

namespace Logging
{
    public class Log
    {
        public static void log(string message)
        {
            DateTime dt = DateTime.Now;
            string tolog = $"{dt.Year}.{dt.Month}.{dt.Day} {dt.Hour}:{dt.Minute}:{dt.Second}.{dt.Millisecond}\t{message}";
            string path = $"Log/{dt.Year}.{dt.Month}.{dt.Day}.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(tolog);
            }
            Console.WriteLine(tolog);
        }
    }
}