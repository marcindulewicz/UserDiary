using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DiaryUser
{
    static class Program
    {
        public static string FilePath = Path.Combine(Environment.CurrentDirectory, "students.txt");
        public static List<string> AllClasses = new List<string>() {"1A", "1B", "1C", "2A", "2B", "2C", "3A", "3B", "3C" };
        public static string NoFilterDataWhen = "Wszyscy uczniowie";
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
